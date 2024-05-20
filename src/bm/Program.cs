using System;
using System.Drawing;
using System.Text;

namespace FingerprintIdentification
{
    class BM
    {
        static void Main(string[] args)
        {
            string filePath = "src/bm/jari2.png"; // Ganti dengan path gambar sidik jari Anda
            Bitmap image = new Bitmap(filePath);
            byte[,] binaryArray = ConvertToBinaryArray(image);

            // Untuk keperluan testing, konversi segmen 30x30 pixel menjadi ASCII dan cari menggunakan Boyer-Moore
            string binaryString = ConvertToBinaryString(binaryArray, 0, 0, 30, 30);
            string asciiString = ConvertBinaryToASCII(binaryString);

            // Asumsikan referenceFingerprint adalah data sidik jari referensi yang telah diproses dalam format ASCII
            string referenceASCIIString = LoadReferenceFingerprint();

            BoyerMoore bm = new BoyerMoore(referenceASCIIString);
            int matchIndex = bm.Search(asciiString);

            if (matchIndex != -1)
            {
                Console.WriteLine("Fingerprint matched!");
            }
            else
            {
                Console.WriteLine("Fingerprint did not match.");
            }
        }

        static byte[,] ConvertToBinaryArray(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            byte[,] binaryArray = new byte[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    binaryArray[y, x] = (byte)(pixelColor.R > 128 ? 1 : 0);
                }
            }
            return binaryArray;
        }

        static string ConvertToBinaryString(byte[,] data, int startX, int startY, int width, int height)
        {
            StringBuilder binaryString = new StringBuilder();
            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    binaryString.Append(data[y, x]);
                }
            }
            return binaryString.ToString();
        }

        static string ConvertBinaryToASCII(string binaryString)
        {
            StringBuilder asciiString = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 8)
            {
                string byteString = binaryString.Substring(i, 8);
                char asciiChar = (char)Convert.ToByte(byteString, 2);
                asciiString.Append(asciiChar);
            }
            return asciiString.ToString();
        }

        static string LoadReferenceFingerprint()
        {
            // Load dan kembalikan data sidik jari referensi dalam format ASCII
            // Untuk contoh ini, kita mengembalikan string dummy
            return "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00¨\x00\x00%P`\x00À\x89\x82\x03õÿà\x0füÿà?¾ÿ\x80ÿÿû\x03ÿüü\x0fÿÿð\x1fÿÿÀÿÿÿ\x01ÿÿü\x07ÿÿð\x1fÿÿÀ\x7fÿÿ\x01ÿÿü\x07ÿÿð\x0fÿÿÀ\x1fÿÿ\x00?ÿø\x00\x7fÿ\x08";
        }
    }

    public class BoyerMoore
    {
        private int[] badCharShift;
        private int[] goodSuffixShift;
        private string pattern;

        public BoyerMoore(string pattern)
        {
            this.pattern = pattern;
            Preprocess();
        }

        private void Preprocess()
        {
            int m = pattern.Length;
            badCharShift = new int[256];
            goodSuffixShift = new int[m];

            for (int i = 0; i < 256; i++)
                badCharShift[i] = m;

            for (int i = 0; i < m - 1; i++)
                badCharShift[pattern[i]] = m - i - 1;

            int[] suffixes = new int[m];
            suffixes[m - 1] = m;
            for (int i = m - 2, j = m - 1; i >= 0; i--)
            {
                if (i > j && suffixes[i + m - 1 - j] < i - j)
                    suffixes[i] = suffixes[i + m - 1 - j];
                else
                {
                    if (i < j)
                        j = i;
                    while (j >= 0 && pattern[j] == pattern[j + m - 1 - i])
                        j--;
                    suffixes[i] = i - j;
                }
            }

            for (int i = 0; i < m; i++)
                goodSuffixShift[i] = m;

            for (int i = m - 1, j = 0; i >= 0; i--)
                if (suffixes[i] == i + 1)
                    for (; j < m - 1 - i; j++)
                        if (goodSuffixShift[j] == m)
                            goodSuffixShift[j] = m - 1 - i;

            for (int i = 0; i <= m - 2; i++)
                goodSuffixShift[m - 1 - suffixes[i]] = m - 1 - i;
        }

        public int Search(string text)
        {
            int n = text.Length;
            int m = pattern.Length;
            int skip;
            for (int i = 0; i <= n - m; i += skip)
            {
                skip = 0;
                for (int j = m - 1; j >= 0; j--)
                {
                    if (pattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - badCharShift[text[i + j]]);
                        break;
                    }
                }
                if (skip == 0)
                    return i;
            }
            return -1;
        }
    }
}
