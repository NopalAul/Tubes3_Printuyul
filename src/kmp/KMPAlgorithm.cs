using System;
using System.Collections.Generic;

public class KMPAlgorithm
{
    // Method to compute the LPS (Longest Prefix Suffix) array
    private static int[] ComputeLPSArray(string pattern)
    {
        int length = 0; // length of the previous longest prefix suffix
        int i = 1;
        int[] lps = new int[pattern.Length];
        lps[0] = 0; // lps[0] is always 0

        // The loop calculates lps[i] for i = 1 to M-1
        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[length])
            {
                length++;
                lps[i] = length;
                i++;
            }
            else
            {
                if (length != 0)
                {
                    length = lps[length - 1];
                }
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }
        }
        return lps;
    }

    // Method to perform KMP algorithm
    public static List<int> KMPSearch(string text, string pattern)
    {
        int M = pattern.Length;
        int N = text.Length;
        List<int> matches = new List<int>();

        // Create lps[] that will hold the longest prefix suffix values for pattern
        int[] lps = ComputeLPSArray(pattern);

        int i = 0; // index for text[]
        int j = 0; // index for pattern[]
        while (i < N)
        {
            if (pattern[j] == text[i])
            {
                j++;
                i++;
            }

            if (j == M)
            {
                matches.Add(i - j);
                j = lps[j - 1];
            }
            else if (i < N && pattern[j] != text[i])
            {
                if (j != 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    i++;
                }
            }
        }

        return matches;
    }

    // Main method to test the KMP algorithm
    public static void Main()
    {
        // string text = "ABABDABACDABABCABAB";
        // string pattern = "ABABCABAB";
        string text = "ini ucok";
        string pattern = "ucok";
        List<int> matchIndexes = KMPSearch(text, pattern);

        for (int index = 0; index < matchIndexes.Count; index++)
        {
            Console.WriteLine("Pattern found at index: " + matchIndexes[index]);
        }

        // Console.WriteLine("Pattern found at indexes: " + string.Join(", ", matchIndexes));
    }
}
