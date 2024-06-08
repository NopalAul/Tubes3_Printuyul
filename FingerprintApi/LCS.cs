using System;

class LCS
{
    public static int FindLCSLength(string str1, string str2)
    {
        int m = str1.Length;
        int n = str2.Length;
        int[,] lcsTable = new int[m + 1, n + 1];
        for (int i = 0; i <= m; i++)
        {
            for (int j = 0; j <= n; j++)
            {
                if (i == 0 || j == 0)
                {
                    lcsTable[i, j] = 0;
                }
                else if (str1[i - 1] == str2[j - 1])
                {
                    lcsTable[i, j] = lcsTable[i - 1, j - 1] + 1;
                }
                else
                {
                    lcsTable[i, j] = Math.Max(lcsTable[i - 1, j], lcsTable[i, j - 1]);
                }
            }
        }

        return lcsTable[m, n];
    }

    // print lcs yang paling besar
    public static string GetLCS(string str1, string str2)
    {
        int m = str1.Length;
        int n = str2.Length;
        int[,] lcsTable = new int[m + 1, n + 1];
        
        for (int i = 0; i <= m; i++)
        {
            for (int j = 0; j <= n; j++)
            {
                if (i == 0 || j == 0)
                {
                    lcsTable[i, j] = 0;
                }
                else if (str1[i - 1] == str2[j - 1])
                {
                    lcsTable[i, j] = lcsTable[i - 1, j - 1] + 1;
                }
                else
                {
                    lcsTable[i, j] = Math.Max(lcsTable[i - 1, j], lcsTable[i, j - 1]);
                }
            }
        }

        // Following code is used to print LCS
        int index = lcsTable[m, n];
        char[] lcs = new char[index];
        int iIndex = m, jIndex = n;

        while (iIndex > 0 && jIndex > 0)
        {
            if (str1[iIndex - 1] == str2[jIndex - 1])
            {
                lcs[index - 1] = str1[iIndex - 1];
                iIndex--;
                jIndex--;
                index--;
            }
            else if (lcsTable[iIndex - 1, jIndex] > lcsTable[iIndex, jIndex - 1])
            {
                iIndex--;
            }
            else
            {
                jIndex--;
            }
        }

        return new string(lcs);
    }

    public static void Main()
    {
        string str1 = "aku mau makan pisang";
        string str2 = "pisang dimakan";

        Console.WriteLine("Length of LCS is " + FindLCSLength(str1, str2));
        Console.WriteLine("LCS is " + GetLCS(str1, str2));
    }
}
