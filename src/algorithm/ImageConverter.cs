using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ImageConverter
{

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
    
    public static (string asciiString1, string asciiString2) ConvertBinaryArraysToAsciiStrings(List<int[,]> binaryArrays)
    {
        StringBuilder binaryString1 = new StringBuilder();
        StringBuilder binaryString2 = new StringBuilder();

        foreach (var binaryArray in binaryArrays)
        {
            int rows = binaryArray.GetLength(0);
            int columns = binaryArray.GetLength(1);

            // Ensure the binary array has enough columns
            if (columns >= 64)
            {
                // Append the pixels of the first row to binaryString1
                for (int j = 0; j < 64; j++)
                {
                    binaryString1.Append(binaryArray[0, j]);
                }

                // Append the pixels of the second row to binaryString2
                for (int j = 0; j < 64; j++)
                {
                    binaryString2.Append(binaryArray[1, j]);
                }
            }
        }

        // Convert binary strings to ASCII strings
        string asciiStr1 = ConvertBinaryStringToAsciiString(binaryString1.ToString());
        string asciiStr2 = ConvertBinaryStringToAsciiString(binaryString2.ToString());

        return (asciiStr1, asciiStr2);
    }

    private static string ConvertBinaryStringToAsciiString(string binary)
    {
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



    public static Image<Rgba32> CropImageTo1x32(Image<Rgba32> image)
    {
        // Calculate the middle pixel position, ensuring it starts at a multiple of 8
        int startX = (image.Width / 2) / 8 * 8;
        int startY = (image.Height / 2) - 16;

        // Ensure startY is a multiple of 8
        startY = (startY / 8) * 8;

        // Crop the image, ensuring dimensions are within bounds
        if (startY + 32 > image.Height)
        {
            startY = image.Height - 32;
        }

        return image.Clone(ctx => ctx.Crop(new Rectangle(startX, startY, 1, 32)));
    }

    public static Image<Rgba32> CropImageTo1x64(Image<Rgba32> image)
    {
        // Calculate the middle pixel position, ensuring it starts at a multiple of 8
        int startX = (image.Width / 2) / 8 * 8;
        int startY = (image.Height / 2) - 32;

        // Ensure startY is a multiple of 8
        startY = (startY / 8) * 8;

        // Crop the image, ensuring dimensions are within bounds
        if (startY + 64 > image.Height)
        {
            startY = image.Height - 64;
        }

        return image.Clone(ctx => ctx.Crop(new Rectangle(startX, startY, 2, 64)));
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