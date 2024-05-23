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

    public static (string asciiString1, string asciiString2) ConvertBinaryArraysToAsciiStrings(List<int[,]> binaryArrays)
    {
        StringBuilder asciiString1 = new StringBuilder();
        StringBuilder asciiString2 = new StringBuilder();

        foreach (var binaryArray in binaryArrays)
        {
            int rows = binaryArray.GetLength(0);
            int columns = binaryArray.GetLength(1);
            // Append the pixels of the first row to asciiString1
            for (int j = 0; j < 64; j++)
            {
                asciiString1.Append(binaryArray[0, j]);
            }

            // Append the pixels of the second row to asciiString2
            for (int j = 0; j < 64; j++)
            {
                asciiString2.Append(binaryArray[1, j]);
            }
        }

        string asciiStr1 = ConvertBinaryArrayToAsciiString(asciiString1.ToString());
        string asciiStr2 = ConvertBinaryArrayToAsciiString(asciiString2.ToString());

        return (asciiStr1, asciiStr2);
    }


    public static Image<Rgba32> CropImageTo2x64(Image<Rgba32> image)
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