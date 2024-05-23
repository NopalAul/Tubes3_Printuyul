using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // INI BUAT TES SMUAMUAMUANYA GAMBAR

        string folderPath = "../../test"; 
        string[] filePaths = Directory.GetFiles(folderPath, "*.BMP");

        Dictionary<string, (string, string)> referenceImagesMap = new Dictionary<string, (string, string)>();
        Dictionary<string, (string, string)> croppedReferenceImagesMap = new Dictionary<string, (string, string)>();

        // Start the timer
        Stopwatch stopwatch = Stopwatch.StartNew();

        foreach (string filePath in filePaths)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
            {
                int[,] binaryArray = ImageConverter.ConvertToBinary(image);
                string asciiString = ImageConverter.ConvertBinaryArrayToAsciiString(binaryArray);
                referenceImagesMap[filePath] = (asciiString, asciiString);

                using (Image<Rgba32> croppedImage = ImageConverter.CropImageTo1x64(image))
                {
                    int[,] croppedBinaryArray = ImageConverter.ConvertToBinary(croppedImage);
                    var (asciiString1, asciiString2) = ImageConverter.ConvertBinaryArraysToAsciiStrings(new List<int[,]> { croppedBinaryArray });
                    croppedReferenceImagesMap[filePath] = (asciiString1, asciiString2);
                }
            }
        }

        // Print length of filePaths
        Console.WriteLine($"len filePaths: {filePaths.Length}");

        // TEST PENCOCOKAN GAMBAR
        string patternPath = "../../test/590__M_Left_little_finger.BMP";
        string pattern1 = "";
        string pattern2 = "";

        // Crop the patternPath image with ImageConverter.CropImageTo1x64 then convert to ascii strings
        using (Image<Rgba32> patternImage = Image.Load<Rgba32>(patternPath))
        {   
            using (Image<Rgba32> croppedPatternImage = ImageConverter.CropImageTo1x64(patternImage))
            {
                int[,] croppedPatternBinaryArray = ImageConverter.ConvertToBinary(croppedPatternImage);
                // Print cropped pattern binary array
                ImageConverter.PrintBinaryArray(croppedPatternBinaryArray);
                var (asciiString1, asciiString2) = ImageConverter.ConvertBinaryArraysToAsciiStrings(new List<int[,]> { croppedPatternBinaryArray });
                pattern1 = asciiString1;
                pattern2 = asciiString2;
            }
        }
        
        // Print patterns
        Console.WriteLine($"Pattern 1 uploaded image: {pattern1}");
        Console.WriteLine($"Pattern 2 uploaded image: {pattern2}");

        // Choose algorithm: "KMP" or "BM"
        string algorithmChoice = "KMP";

        FingerprintMatcher matcher = new FingerprintMatcher(algorithmChoice);
        var result = matcher.FindMostSimilarFingerprint(pattern1, pattern2, referenceImagesMap, croppedReferenceImagesMap);   
        string similarImage = result.mostSimilarImage;
        double percentage = result.maxSimilarity;

        // Print result
        Console.WriteLine($"Similar image: {similarImage}");
        Console.WriteLine($"Similarity percentage: {percentage}");

        // Stop the timer
        stopwatch.Stop();
        // Print the elapsed time
        Console.WriteLine($"\nTime elapsed: {stopwatch.Elapsed}");
    }
}
