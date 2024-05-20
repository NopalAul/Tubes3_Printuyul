using System;
using System.Text;
using System.Collections.Generic;
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

            // MAC TESTING STARTS HERE

            // using (Image<Rgba32> originalImage = Image.Load<Rgba32>("jari2.png"))
            // {
            //     // Convert to binary and get the binary array
            //     int[,] binaryArray = ConvertToBinary2(originalImage);

            //     // Print the binary image
            //     PrintBinaryImage2(binaryArray);

            //     Console.WriteLine("beres.");

            //     string asciiString = ConvertBinaryArrayToAsciiString(binaryArray);
            //     Console.WriteLine(asciiString);
            //     Console.WriteLine("beres 2.");
            // }

            // TES BOYER MOORE

            BoyerMoore bm = new BoyerMoore();
            // string text = "a pattern matching algorithm";
            // string pattern = "rithm";

            // string text = "THIS IS A SIMPLE EXAMPLE";
            // string pattern = "EXAMPLE";

            // string text = "denise felicia tiowanni";
            // string pattern = "ici";

            string text = "aku suka aku kamu aku lagi aku";
            string pattern = "aku";

            List<int> result = bm.Search(text, pattern);

            Console.WriteLine("yey ketemu, ni indeksnya:");
            foreach (int index in result)
            {
                Console.WriteLine(index);
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

        class BoyerMoore
        {
            private int[] BuildBadCharacterTable(string pattern)
            {
                int[] badCharTable = new int[256];
                int patternLength = pattern.Length;

                for (int i = 0; i < 256; i++)
                {
                    badCharTable[i] = -1;
                }

                for (int i = 0; i < patternLength; i++)
                {
                    badCharTable[(int)pattern[i]] = i;
                }

                return badCharTable;
            }

            private int[] BuildGoodSuffixTable(string pattern)
            {
                int m = pattern.Length;
                int[] goodSuffixTable = new int[m];
                int[] suffixes = new int[m];
                
                for (int i = 0; i < m; i++)
                {
                    suffixes[i] = -1;
                    goodSuffixTable[i] = m;
                }

                int f = 0;
                int g = m - 1;

                for (int i = m - 2; i >= 0; --i)
                {
                    if (i > g && suffixes[i + m - 1 - f] < i - g)
                    {
                        suffixes[i] = suffixes[i + m - 1 - f];
                    }
                    else
                    {
                        if (i < g)
                        {
                            g = i;
                        }
                        f = i;
                        while (g >= 0 && pattern[g] == pattern[g + m - 1 - f])
                        {
                            --g;
                        }
                        suffixes[i] = f - g;
                    }
                }

                for (int i = 0; i < m - 1; ++i)
                {
                    goodSuffixTable[m - 1 - suffixes[i]] = m - 1 - i;
                }

                for (int i = 0; i <= m - 2; ++i)
                {
                    goodSuffixTable[m - 1 - suffixes[i]] = m - 1 - i;
                }

                return goodSuffixTable;
            }

            public List<int> Search(string text, string pattern)
            {
                List<int> matches = new List<int>();
                int[] badCharTable = BuildBadCharacterTable(pattern);
                int[] goodSuffixTable = BuildGoodSuffixTable(pattern);
                int textLength = text.Length;
                int patternLength = pattern.Length;
                int s = 0;

                while (s <= (textLength - patternLength))
                {
                    int j = patternLength - 1;

                    while (j >= 0 && pattern[j] == text[s + j])
                    {
                        j--;
                    }

                    if (j < 0)
                    {
                        matches.Add(s);
                        s += (s + patternLength < textLength) ? patternLength - badCharTable[text[s + patternLength]] : 1;
                    }
                    else
                    {
                        s += Math.Max(goodSuffixTable[j], j - badCharTable[text[s + j]]);
                    }
                }

                return matches;
            }
        }
    }
}
