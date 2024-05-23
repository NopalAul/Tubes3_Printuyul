using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ImageConverter
{
    // public static void Main(string[] args)
    // {
    //     string folderPath = "test"; 
    //     string[] filePaths = Directory.GetFiles(folderPath, "*.BMP");

    //     Dictionary<string, string> fullImageMap = new Dictionary<string, string>();
    //     Dictionary<string, string> croppedImageMap = new Dictionary<string, string>();

    //     foreach (string filePath in filePaths)
    //     {
    //         using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
    //         {
    //             int[,] binaryArray = ConvertToBinary(image);
    //             string asciiString = ConvertBinaryArrayToAsciiString(binaryArray);
    //             fullImageMap[filePath] = asciiString;

    //             using (Image<Rgba32> croppedImage = CropImageTo1x32(image))
    //             {
    //                 int[,] croppedBinaryArray = ConvertToBinary(croppedImage);
    //                 string croppedAsciiString = ConvertBinaryArrayToAsciiString(croppedBinaryArray);
    //                 croppedImageMap[filePath] = croppedAsciiString;
    //             }
    //         }
    //     }
    // }

    public static int[,] ConvertToBinary(Image<Rgba32> original, byte threshold = 128)
    {
        int width = original.Width;
        int height = original.Height;
        int[,] binaryArray = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // ambil pixel color
                Rgba32 pixelColor = original[x, y];
                // convert pixelnya ke grayscale
                byte grayValue = (byte)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                // convert ke binary berdasarkan threshold
                binaryArray[x, y] = grayValue < threshold ? 0 : 1;
            }
        }

        return binaryArray;
    }

    public static string ConvertBinaryArrayToAsciiString(int[,] binaryArray)
    {
        int rows = binaryArray.GetLength(0);
        int columns = binaryArray.GetLength(1);
        
        StringBuilder asciiString = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                asciiString.Append(binaryArray[i, j]);
            }
        }

        string binary = asciiString.ToString();
        // Ensure the binary string has a length that is a multiple of 8
        int remainder = binary.Length % 8;
        if (remainder != 0)
        {
            binary = binary.PadRight(binary.Length + (8 - remainder), '0');
        }

        StringBuilder finalAsciiString = new StringBuilder();
        for (int k = 0; k < binary.Length; k += 8)
        {
            string byteString = binary.Substring(k, 8);
            int asciiCode = Convert.ToInt32(byteString, 2);
            char character = (char)asciiCode;
            finalAsciiString.Append(character);
        }

        return finalAsciiString.ToString();
    }

    // public static Image<Rgba32> CropImageTo1x32(Image<Rgba32> image)
    // {
    //     int centerX = image.Width / 2;
    //     int startY = Math.Max(0, image.Height / 2 - 16);
    //     return image.Clone(ctx => ctx.Crop(new Rectangle(centerX, startY, 1, 32)));
    // }

    public static Image<Rgba32> CropImageTo1x32(Image<Rgba32> image)
    {
        // Calculate the total number of pixels in the image
        int totalPixels = image.Width * image.Height;

        // Find the middle pixel position
        int middlePixel = totalPixels / 2;

        // Calculate the starting pixel position, ensuring it is a multiple of 8
        int startPixel = middlePixel - 16;
        startPixel = (startPixel / 8) * 8;

        // Calculate the corresponding x and y coordinates from the starting pixel position
        int startX = startPixel % image.Width;
        int startY = startPixel / image.Width;

        // Ensure that we do not go out of bounds
        if (startY + 32 > image.Height)
        {
            startY = image.Height - 32;
        }

        // Crop the image
        return image.Clone(ctx => ctx.Crop(new Rectangle(startX, startY, 1, 32)));
    }

    // public static Image<Rgba32> CropImageTo1x64(Image<Rgba32> image)
    // {
    //     int startingX = (image.Width - 64) / 2;
    //     int startingY = (image.Height - 1) / 2;
        
    //     // Ensure startingX and startingY are multiples of 8
    //     startingX -= startingX % 8;
    //     startingY -= startingY % 8;
        
    //     var croppedImage = image.Clone(ctx => ctx.Crop(new Rectangle(startingX, startingY, 64, 1)));
        
    //     return croppedImage;
    // }

        
    public static void PrintBinaryArray(int[,] binaryArray)
    {
        int rows = binaryArray.GetLength(0);
        int columns = binaryArray.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Console.Write(binaryArray[i, j]);
            }
            Console.WriteLine();
        }
    }
}