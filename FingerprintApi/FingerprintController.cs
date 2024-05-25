using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace FingerprintApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FingerprintController : ControllerBase
    {
        private static readonly Dictionary<string, string> referenceImagesMap = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> croppedReferenceImagesMap = new Dictionary<string, string>();

        

        public FingerprintController()
        {
            Controller data = new Controller("MainData.db");
            var fingerDataList = data.TraverseSidikJari();

            foreach (var fingerData in fingerDataList)
            {
                string filePath = fingerData.getPath(); // Assuming getPath method returns the file path

                using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
                {
                    int[,] binaryArray = ImageConverter.ConvertToBinary(image);
                    string asciiString = ImageConverter.ConvertBinaryArrayToAsciiString(binaryArray);
                    referenceImagesMap[filePath] = asciiString;

                    using (Image<Rgba32> croppedImage = ImageConverter.CropImageTo1x64(image))
                    {
                        int[,] croppedBinaryArray = ImageConverter.ConvertToBinary(croppedImage);
                        string croppedAsciiString = ImageConverter.ConvertBinaryArrayToAsciiString(croppedBinaryArray);
                        croppedReferenceImagesMap[filePath] = croppedAsciiString;
                    }
                }
            }
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessImage([FromForm] IFormFile image, [FromForm] string algorithm)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded.");

            string tempFilePath = Path.GetTempFileName();
            var stopwatch = new System.Diagnostics.Stopwatch();

            try
            {
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    await image.CopyToAsync(stream);
                }

                string pattern;

                stopwatch.Start();

                using (Image<Rgba32> uploadedImage = Image.Load<Rgba32>(tempFilePath))
                {
                    int[,] patternBinaryArray = ImageConverter.ConvertToBinary(uploadedImage);
                    // ImageConverter.PrintBinaryArray(patternBinaryArray);

                    using (Image<Rgba32> croppedPatternImage = ImageConverter.CropImageTo1x64(uploadedImage))
                    {
                        int[,] croppedPatternBinaryArray = ImageConverter.ConvertToBinary(croppedPatternImage);
                        // ImageConverter.PrintBinaryArray(croppedPatternBinaryArray);
                        pattern = ImageConverter.ConvertBinaryArrayToAsciiString(croppedPatternBinaryArray);
                    }
                }

                FingerprintMatcher matcher = new FingerprintMatcher(algorithm);
                var result = matcher.FindMostSimilarFingerprint(pattern, referenceImagesMap, croppedReferenceImagesMap);
                string similarImage = result.mostSimilarImage;
                double percentage = result.maxSimilarity;
                bool exactMatchFound = result.exactMatchFound;

                stopwatch.Stop();

                var executionTime = stopwatch.ElapsedMilliseconds;

                return Ok(new { similarImage = Path.GetFileName(similarImage), percentage, executionTime, exactMatchFound });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }

        [HttpGet("image/{filename}")]
        public IActionResult GetImage(string filename)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "../test");
            string filePath = Path.Combine(folderPath, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, "image/bmp");
        }

        [HttpGet("biodata/{filename}")]
        public IActionResult GetBiodata(string filename)
        {
            try
            {
                Console.WriteLine("masuk");
                string imagePath = "../test/" + filename;
                Console.WriteLine("Image path: " + imagePath);

                Controller dataController = new Controller("MainData.db");
                string query = "SELECT nama FROM sidik_jari WHERE berkas_citra = @imagePath;";
                string ownerName = "";
                
                using (var command = new SqliteCommand(query, dataController.sql_conn))
                {
                    command.Parameters.AddWithValue("@imagePath", imagePath);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ownerName = reader["nama"].ToString();
                            Console.WriteLine("Owner name: " + ownerName);
                        }
                        else
                        {
                            Console.WriteLine("No owner found for image path: " + imagePath);
                        }
                    }
                }

                if (string.IsNullOrEmpty(ownerName))
                {
                    return NotFound("Owner not found.");
                }

                // regex pattern utk nama owner
                string namePattern = Regex.CreateWeirdNameRegex(ownerName);
                Console.WriteLine("Regex pattern: " + namePattern);

                query = "SELECT * FROM biodata;";
                List<KTPData> biodataList = new List<KTPData>();

                using (var command = new SqliteCommand(query, dataController.sql_conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string biodataName = reader["nama"].ToString();
                            // Console.WriteLine("Checking biodata name: " + biodataName);

                            if (System.Text.RegularExpressions.Regex.IsMatch(biodataName, namePattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                Console.WriteLine("Match found: " + biodataName);
                                var ktpData = new KTPData(
                                    reader["NIK"].ToString(),
                                    reader["nama"].ToString(),
                                    reader["tempat_lahir"].ToString(),
                                    reader["tanggal_lahir"].ToString(),
                                    reader["jenis_kelamin"].ToString(),
                                    reader["golongan_darah"].ToString(),
                                    reader["alamat"].ToString(),
                                    reader["agama"].ToString(),
                                    reader["status_perkawinan"].ToString(),
                                    reader["pekerjaan"].ToString(),
                                    reader["kewarganegaraan"].ToString()
                                );
                                biodataList.Add(ktpData);
                            }
                        }
                    }
                }

                if (biodataList.Count == 0)
                {
                    Console.WriteLine("No matching biodata found for owner name: " + ownerName);
                    return NotFound("No matching biodata found.");
                }

                // return yang pertama
                return Ok(biodataList.First());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
