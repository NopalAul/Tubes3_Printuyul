using System;
using System.Collections.Generic;

public class KnuthMorrisPratt
{
    private static int[] computeBorder(string pattern)
    {
        int j = 0;
        int i = 1;
        int[] fail = new int[pattern.Length];
        fail[0] = 0;

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

    public List<int> KMP(string text, string pattern)
    {
        int M = pattern.Length;
        int N = text.Length;
        List<int> matches = new List<int>();

        int[] fail = computeBorder(pattern);

        int i = 0;
        int j = 0;
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
}
