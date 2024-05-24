using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "../test");
            string[] filePaths = Directory.GetFiles(folderPath, "*.BMP");

            foreach (string filePath in filePaths)
            {
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

            try
            {
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    await image.CopyToAsync(stream);
                }

                string pattern;

                using (Image<Rgba32> uploadedImage = Image.Load<Rgba32>(tempFilePath))
                {
                    int[,] patternBinaryArray = ImageConverter.ConvertToBinary(uploadedImage);
                    ImageConverter.PrintBinaryArray(patternBinaryArray);

                    using (Image<Rgba32> croppedPatternImage = ImageConverter.CropImageTo1x64(uploadedImage))
                    {
                        int[,] croppedPatternBinaryArray = ImageConverter.ConvertToBinary(croppedPatternImage);
                        ImageConverter.PrintBinaryArray(croppedPatternBinaryArray);
                        pattern = ImageConverter.ConvertBinaryArrayToAsciiString(croppedPatternBinaryArray);
                    }
                }

                FingerprintMatcher matcher = new FingerprintMatcher(algorithm);
                var result = matcher.FindMostSimilarFingerprint(pattern, referenceImagesMap, croppedReferenceImagesMap);
                string similarImage = result.mostSimilarImage;
                double percentage = result.maxSimilarity;

                return Ok(new { similarImage, percentage });
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
    }
}
