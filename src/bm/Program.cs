using System;
using System.Text;
// using System;
// using System.Drawing;
// using System.Drawing.Imaging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace bm
{
    class BM
    {
        static void Main(string[] args)
        {
            // // Load the image
            // Bitmap originalImage = new Bitmap("jari2.png");

            // // Convert to binary and get the binary array
            // int[,] binaryArray = ConvertToBinary(originalImage);

            // // Print the binary image
            // PrintBinaryImage(binaryArray);

            // Console.WriteLine("beres.");

            using (Image<Rgba32> originalImage = Image.Load<Rgba32>("jari2.png"))
            {
                // Convert to binary and get the binary array
                int[,] binaryArray = ConvertToBinary2(originalImage);

                // Print the binary image
                PrintBinaryImage2(binaryArray);

                Console.WriteLine("beres.");

                string asciiString = ConvertBinaryArrayToAsciiString(binaryArray);
                Console.WriteLine(asciiString);
                Console.WriteLine("beres 2.");
            }

        }

        // public static int[,] ConvertToBinary(Bitmap original, byte threshold = 128)
        // {
        //     // Create a 2D array to store binary values
        //     int[,] binaryArray = new int[original.Width, original.Height];

        //     for (int y = 0; y < original.Height; y++)
        //     {
        //         for (int x = 0; x < original.Width; x++)
        //         {
        //             // Get the pixel color
        //             Color pixelColor = original.GetPixel(x, y);

        //             // Convert the pixel to grayscale
        //             byte grayValue = (byte)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);

        //             // Convert to binary based on the threshold
        //             binaryArray[x, y] = grayValue < threshold ? 0 : 1;
        //         }
        //     }

        //     return binaryArray;
        // }

        // public static void PrintBinaryImage(int[,] binaryArray)
        // {
        //     int width = binaryArray.GetLength(0);
        //     int height = binaryArray.GetLength(1);

        //     for (int y = 0; y < height; y++)
        //     {
        //         for (int x = 0; x < width; x++)
        //         {
        //             // Print 0 or 1 based on the binary value
        //             Console.Write(binaryArray[x, y]);
        //         }
        //         Console.WriteLine();
        //     }
        // }

        public static int[,] ConvertToBinary2(Image<Rgba32> original, byte threshold = 128)
        {
            int width = original.Width;
            int height = original.Height;
            int[,] binaryArray = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the pixel color
                    Rgba32 pixelColor = original[x, y];

                    // Convert the pixel to grayscale
                    byte grayValue = (byte)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);

                    // Convert to binary based on the threshold
                    binaryArray[x, y] = grayValue < threshold ? 0 : 1;
                }
            }

            return binaryArray;
        }

        public static void PrintBinaryImage2(int[,] binaryArray)
        {
            int width = binaryArray.GetLength(0);
            int height = binaryArray.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Print 0 or 1 based on the binary value
                    Console.Write(binaryArray[x, y]);
                }
                Console.WriteLine();
            }
        }

        static string ConvertBinaryArrayToAsciiString(int[,] binaryArray)
        {
            int rows = binaryArray.GetLength(0);
            int columns = binaryArray.GetLength(1);
            
            StringBuilder asciiString = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                StringBuilder binaryString = new StringBuilder();
                for (int j = 0; j < columns; j++)
                {
                    binaryString.Append(binaryArray[i, j]);
                }

                string binary = binaryString.ToString();
                // Ensure the binary string has a length that is a multiple of 8
                int remainder = binary.Length % 8;
                if (remainder != 0)
                {
                    binary = binary.PadRight(binary.Length + (8 - remainder), '0');
                }

                for (int k = 0; k < binary.Length; k += 8)
                {
                    string byteString = binary.Substring(k, 8);
                    int asciiCode = Convert.ToInt32(byteString, 2);
                    char character = (char)asciiCode;
                    asciiString.Append(character);
                }
            }

            return asciiString.ToString();
        }

    // public class BoyerMoore
    // {
    //     private int[] badCharShift;
    //     private int[] goodSuffixShift;
    //     private string pattern;

    //     public BoyerMoore(string pattern)
    //     {
    //         this.pattern = pattern;
    //         Preprocess();
    //     }

    //     private void Preprocess()
    //     {
    //         int m = pattern.Length;
    //         badCharShift = new int[256];
    //         goodSuffixShift = new int[m];

    //         for (int i = 0; i < 256; i++)
    //             badCharShift[i] = m;

    //         for (int i = 0; i < m - 1; i++)
    //             badCharShift[pattern[i]] = m - i - 1;

    //         int[] suffixes = new int[m];
    //         suffixes[m - 1] = m;
    //         for (int i = m - 2, j = m - 1; i >= 0; i--)
    //         {
    //             if (i > j && suffixes[i + m - 1 - j] < i - j)
    //                 suffixes[i] = suffixes[i + m - 1 - j];
    //             else
    //             {
    //                 if (i < j)
    //                     j = i;
    //                 while (j >= 0 && pattern[j] == pattern[j + m - 1 - i])
    //                     j--;
    //                 suffixes[i] = i - j;
    //             }
    //         }

    //         for (int i = 0; i < m; i++)
    //             goodSuffixShift[i] = m;

    //         for (int i = m - 1, j = 0; i >= 0; i--)
    //             if (suffixes[i] == i + 1)
    //                 for (; j < m - 1 - i; j++)
    //                     if (goodSuffixShift[j] == m)
    //                         goodSuffixShift[j] = m - 1 - i;

    //         for (int i = 0; i <= m - 2; i++)
    //             goodSuffixShift[m - 1 - suffixes[i]] = m - 1 - i;
    //     }

    //     public int Search(string text)
    //     {
    //         int n = text.Length;
    //         int m = pattern.Length;
    //         int skip;
    //         for (int i = 0; i <= n - m; i += skip)
    //         {
    //             skip = 0;
    //             for (int j = m - 1; j >= 0; j--)
    //             {
    //                 if (pattern[j] != text[i + j])
    //                 {
    //                     skip = Math.Max(1, j - badCharShift[text[i + j]]);
    //                     break;
    //                 }
    //             }
    //             if (skip == 0)
    //                 return i;
    //         }
    //         return -1;
    //     }
    }
}
