using System;
using System.Collections.Generic;

public class BoyerMoore
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

    public List<int> BM(string text, string pattern)
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

    public void PrintBadCharacterTable(string pattern)
    {
        int[] badCharTable = BuildBadCharacterTable(pattern);
        Console.WriteLine("Bad Character Table:");
        for (int i = 0; i < badCharTable.Length; i++)
        {
            if (badCharTable[i] != -1)
            {
                Console.WriteLine($"Character '{(char)i}' -> {badCharTable[i]}");
            }
        }
    }

    // static void Main(string[] args)
    // {
    //     string text = "feliciadenisetiowanni";
    //     string pattern = "denise";
        
    //     BoyerMoore bm = new BoyerMoore();
    //     List<int> matches = bm.BM(text, pattern);
        
    //     if (matches.Count > 0)
    //     {
    //         Console.WriteLine("Pattern found at positions:");
    //         foreach (int match in matches)
    //         {
    //             Console.WriteLine(match);
    //         }
    //         bm.PrintBadCharacterTable(pattern);
    //     }
    //     else
    //     {
    //         Console.WriteLine("Pattern not found in the text.");
    //     }
    // }
}
