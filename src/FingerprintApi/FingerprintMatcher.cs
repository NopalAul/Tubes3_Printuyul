using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FingerprintMatcher
{
    private BoyerMoore bm;
    private KnuthMorrisPratt kmp;
    private string algorithm;

    public FingerprintMatcher(string algorithm)
    {
        bm = new BoyerMoore();
        kmp = new KnuthMorrisPratt();
        this.algorithm = algorithm;
    }

    private int HammingDistance(string s1, string s2)
    {
        // Normalize both strings to FormC (composed form)
        s1 = s1.Normalize(NormalizationForm.FormC);
        s2 = s2.Normalize(NormalizationForm.FormC);

        int distance = 0;
        int len = Math.Min(s1.Length, s2.Length);

        // // Print the length of the strings being compared
        // Console.WriteLine($"len: {len}");

        for (int i = 0; i < len; i++)
        {
            // // Print the characters and their corresponding Unicode code points
            // Console.WriteLine($"s1[{i}]: '{s1[i]}' (U+{(int)s1[i]:X4})");
            // Console.WriteLine($"s2[{i}]: '{s2[i]}' (U+{(int)s2[i]:X4})");

            if (s1[i] != s2[i])
            {
                distance++;
            }
        }

        // // Print the final distance
        // Console.WriteLine($"distance: {distance}");
        return distance;
    }

    private int Levenshtein(string str1, string str2, int m, int n)
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
            return  Levenshtein(str1, str2, m - 1, n - 1);
        }

        return 1 + Math.Min(
                        Levenshtein(str1, str2, m, n - 1),    // insert
                    Math.Min(
                            Levenshtein(str1, str2, m - 1, n),    // remove
                            Levenshtein(str1, str2, m - 1, n - 1) // replace
                    )
                );
    }

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


    public (List<int> matches, Dictionary<string, double> similarityPercentages) Search(
        string pattern, 
        Dictionary<string, string> referenceImagesMap,
        Dictionary<string, string> croppedReferenceImagesMap)
    {
        List<int> matches = new List<int>();
        Dictionary<string, double> similarityPercentages = new Dictionary<string, double>();

        // Iterate through the reference images map
        foreach (var kvp in referenceImagesMap)
        {
            string imagePath = kvp.Key;
            string referenceText = kvp.Value;

            // Perform exact matching using the chosen algorithm
            List<int> algorithmMatches = algorithm == "KMP" ? kmp.KMP(referenceText, pattern) : bm.BM(referenceText, pattern);
            if (algorithmMatches.Count > 0)
            {
                matches.AddRange(algorithmMatches);
                similarityPercentages[imagePath] = 1.0;
                continue;
            }
        }

        // If no exact matches are found, use Hamming Distance on the cropped images map
        if (matches.Count == 0)
        {
            Console.WriteLine("No exact matches found. Using Hamming Distance on cropped images.");

            // Iterate through the cropped reference images map
            foreach (var kvp in croppedReferenceImagesMap)
            {
                string imagePath = kvp.Key;
                string croppedReferenceText = kvp.Value;

                // Perform Hamming Distance calculation
                // Console.WriteLine($"pattern: {pattern}");
                // Console.WriteLine($"croppedReferenceText: {croppedReferenceText}");
                // int distance = Levenshtein(pattern, croppedReferenceText, pattern.Length, croppedReferenceText.Length);
                int distance = HammingDistance(pattern, croppedReferenceText);
                // int distance = FindLCSLength(pattern, croppedReferenceText);
                Console.WriteLine($"distance: {distance}");
                // // print pattern and croppedReferenceText
                // Console.WriteLine($"pattern: {pattern}");
                // Console.WriteLine($"croppedReferenceText: {croppedReferenceText}");
                double similarity = 1.0 - (double)distance / pattern.Length;
                Console.WriteLine($"similarity: {similarity}");
                similarityPercentages[imagePath] = similarity;
            }
        }

        return (matches, similarityPercentages);
    }

    public (string mostSimilarImage, double maxSimilarity, bool exactMatchFound) FindMostSimilarFingerprint(
        string pattern, 
        Dictionary<string, string> referenceImagesMap,
        Dictionary<string, string> croppedReferenceImagesMap)
    {
        var result = Search(pattern, referenceImagesMap, croppedReferenceImagesMap);
        List<int> matches = result.matches;
        Dictionary<string, double> similarityPercentages = result.similarityPercentages;

        if (matches.Count > 0)
        {
            Console.WriteLine("Exact matches found:");

            foreach (int match in matches)
            {   
                Console.WriteLine($"Pattern found at index: {match}");
            }
            string exactMatchImage = null;
            foreach (var kvp in similarityPercentages)
            {
                if (kvp.Value == 1.0)
                {   
                    exactMatchImage = kvp.Key;
                    break;
                }
            }

            double maxSimilarity = 100;
            return (exactMatchImage ?? string.Empty, maxSimilarity, true);
        }
        else
        {
            double maxSimilarity = 0;
            string mostSimilarImage = null;

            foreach (var kvp in similarityPercentages)
            {
                if (kvp.Value >= maxSimilarity)
                {
                    maxSimilarity = kvp.Value;
                    mostSimilarImage = kvp.Key;
                }
            }

            Console.WriteLine($"maxSimilarity: {maxSimilarity}");

            maxSimilarity *= 100;
            if (maxSimilarity >= 50)
            {
                Console.WriteLine($"Most similar fingerprint found in image: {Path.GetFileName(mostSimilarImage)} with similarity {maxSimilarity}%");
                return (mostSimilarImage ?? string.Empty, maxSimilarity, false);
            }
            else
            {
                Console.WriteLine("No similar fingerprint found.");
                return (null, 0, false);
            }
        }
    }
}