using System;

class Program
{
    static int LevenshteinRecursive(string str1, string str2, int m, int n)
    {
        // str1 is empty
        if (m == 0)
        {
            return n;
        }
        // str2 is empty
        if (n == 0)
        {
            return m;
        }

        if (str1[m - 1] == str2[n - 1])
        {
            return LevenshteinRecursive(str1, str2, m - 1, n - 1);
        }

        return 1 + Math.Min(
                    LevenshteinRecursive(str1, str2, m, n - 1),    // insert
                    Math.Min(
                        LevenshteinRecursive(str1, str2, m - 1, n),    // remove
                        LevenshteinRecursive(str1, str2, m - 1, n - 1) // replace
                    )
                );
    }

    static void Main()
    {
        string str1 = "densu";
        string str2 = "densus";

        int distance = LevenshteinRecursive(str1, str2, str1.Length, str2.Length);
        Console.WriteLine("Levenshtein Distance: " + distance);
    }
}
