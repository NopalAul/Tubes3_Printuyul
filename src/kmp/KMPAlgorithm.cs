using System;
using System.Collections.Generic;

public class KMPAlgorithm
{
    // Method to compute the LPS (Longest Prefix Suffix) array
    private static int[] computeBorder(string pattern)
    {
        int j = 0; // length of the previous longest prefix suffix
        int i = 1;
        int[] fail = new int[pattern.Length];
        fail[0] = 0; // fail[0] is always 0

        // The loop calculates fail[i] for i = 1 to M-1
        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[j])
            {
                j++;
                fail[i] = j;
                i++;
            }
            else
            {
                if (j != 0)
                {
                    j = fail[j - 1];
                }
                else
                {
                    fail[i] = 0;
                    i++;
                }
            }
        }
        return fail;
    }

    // Method to perform KMP algorithm
    public static List<int> KMP(string text, string pattern)
    {
        int M = pattern.Length;
        int N = text.Length;
        List<int> matches = new List<int>();

        // Create fail[] that will hold the longest prefix suffix values for pattern
        int[] fail = computeBorder(pattern);

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
                j = fail[j - 1];
            }
            else if (i < N && pattern[j] != text[i])
            {
                if (j != 0)
                {
                    j = fail[j - 1];
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
        string text = "ini ucok ucok ucok ucok";
        string pattern = "ko";
        List<int> matchIndexes = KMP(text, pattern);

        if (matchIndexes.Count != 0) {
            for (int index = 0; index < matchIndexes.Count; index++)
            {
                Console.WriteLine("Pattern found at index: " + matchIndexes[index]);
            }
        }
        else {
            Console.WriteLine("Pattern not found");
        }

        // Console.WriteLine("Pattern found at indexes: " + string.Join(", ", matchIndexes));
    }
}
