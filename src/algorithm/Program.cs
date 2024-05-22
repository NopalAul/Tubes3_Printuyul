using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        // // Example usage
        // // string dummyAsciiString = "iÿÿÿÿÿÿÿÿdajdoeiaoidjeaisdmasdmsadsaifjiweab";
        // string pattern = "inilobener";

        // // Simulate reference images with dummy ASCII strings
        // string[] referenceImages = { "reference1.txt", "reference2.txt", "reference3.txt" };

        // // Write dummy ASCII strings to reference files for simulation
        // File.WriteAllText("reference1.txt", "iÿÿÿÿÿÿÿÿÿÿÌÿÿû:Ãÿÿ<Ìÿÿuwûÿkÿý,Pþÿ¿/ÿÿ");
        // File.WriteAllText("reference2.txt", "iÿÿÿasdiajsudheurfheuafbeiybfiayebfiayef");
        // File.WriteAllText("reference3.txt", "iniloÿÿÿÿÿÿÿÿdajdoeiaoidjeinilobeneaisdmas");
        // // File.WriteAllText("reference4.txt", "inilobener");

        // // Choose algorithm: "KMP" or "BM"
        // string algorithmChoice = "KMP";

        // FingerprintMatcher matcher = new FingerprintMatcher(algorithmChoice);
        // string result = matcher.FindMostSimilarFingerprint(pattern, referenceImages);   
        // Console.WriteLine(result);

    // INI BUAT TES SMUAMUAMUANYA GAMBAR

    //     string folderPath = "../../test"; 
    //     string[] filePaths = Directory.GetFiles(folderPath, "*.BMP");

    //     Dictionary<string, string> referenceImagesMap = new Dictionary<string, string>();
    //     Dictionary<string, string> croppedReferenceImagesMap = new Dictionary<string, string>();

    //     foreach (string filePath in filePaths)
    //     {
    //         using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
    //         {
    //             int[,] binaryArray = ImageConverter.ConvertToBinary(image);
    //             string asciiString = ImageConverter.ConvertBinaryArrayToAsciiString(binaryArray);
    //             referenceImagesMap[filePath] = asciiString;

    //             using (Image<Rgba32> croppedImage = ImageConverter.CropImageTo1x32(image))
    //             {
    //                 int[,] croppedBinaryArray = ImageConverter.ConvertToBinary(croppedImage);
    //                 string croppedAsciiString = ImageConverter.ConvertBinaryArrayToAsciiString(croppedBinaryArray);
    //                 croppedReferenceImagesMap[filePath] = croppedAsciiString;
    //             }
    //         }
    //     }

    //     // Example of printing the results
    //     Console.WriteLine("Full Image HashMap:");
    //     foreach (var kvp in referenceImagesMap)
    //     {
    //         Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
    //     }

    //     Console.WriteLine("\nCropped Image HashMap:");
    //     foreach (var kvp in croppedReferenceImagesMap)
    //     {
    //         Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
    //     }

    // }

    // TES 1 GAMBAR AJAA
        string folderPath = "../../test"; 
        string[] filePaths = Directory.GetFiles(folderPath, "*.BMP");

        Dictionary<string, string> referenceImagesMap = new Dictionary<string, string>();
        Dictionary<string, string> croppedReferenceImagesMap = new Dictionary<string, string>();

        if (filePaths.Length > 0)
        {
            string filePath = filePaths[3];
            using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
            {
                int[,] binaryArray = ImageConverter.ConvertToBinary(image);
                string asciiString = ImageConverter.ConvertBinaryArrayToAsciiString(binaryArray);
                referenceImagesMap[filePath] = asciiString;

                Console.WriteLine("binary full image:");
                ImageConverter.PrintBinaryArray(binaryArray);

                using (Image<Rgba32> croppedImage = ImageConverter.CropImageTo1x32(image))
                {
                    int[,] croppedBinaryArray = ImageConverter.ConvertToBinary(croppedImage);
                    string croppedAsciiString = ImageConverter.ConvertBinaryArrayToAsciiString(croppedBinaryArray);
                    croppedReferenceImagesMap[filePath] = croppedAsciiString;

                    Console.WriteLine("binary cropped image:");
                    ImageConverter.PrintBinaryArray(croppedBinaryArray);
                }
            }

            Console.WriteLine("full hashmap:");
            foreach (var kvp in referenceImagesMap)
            {
                Console.WriteLine($"k: {kvp.Key}, v: {kvp.Value}");
            }

            Console.WriteLine("\ncropped hashmap:");
            foreach (var kvp in croppedReferenceImagesMap)
            {
                Console.WriteLine($"k: {kvp.Key}, v: {kvp.Value}");
            }
        }
        else
        {
            Console.WriteLine("Foldernya kosong, sayangku.");
        }
    }
}
