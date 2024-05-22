using System;
using System.Collections.Generic;
using System.IO;

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
        int distance = 0;
        int len = Math.Min(s1.Length, s2.Length);
        for (int i = 0; i < len; i++)
        {
            if (s1[i] != s2[i])
            {
                distance++;
            }
        }
        return distance;
        // return distance + Math.Abs(s1.Length - s2.Length);
    }

    public (List<int> matches, Dictionary<string, double> similarityPercentages) Search(string pattern, string[] referenceImages)
    {
        List<int> matches = new List<int>();
        Dictionary<string, double> similarityPercentages = new Dictionary<string, double>();

        foreach (string image in referenceImages)
        {   
            // bikin 2 collection hasil proses gambar database ke map dengan key nama image, valuenya ascii string <string, string>:
                // 1. full image ascii
                // 2. cropped image ascii sama kaya croppednya uploaded image
            // 
            // lanjut lempar map tadi ke search, ke parameter referenceImages.. jadi ubah tipe referenceImages jadi map. Search ini nangkep 3 parameter: pattern, referenceImages, referenceImagesCrop
            string referenceText = File.ReadAllText(image); // nanti ga perlu File.read....

            // Perform exact matching
            List<int> algorithmMatches = algorithm == "KMP" ? kmp.KMP(referenceText, pattern) : bm.BM(referenceText, pattern);
            matches.AddRange(algorithmMatches);
            if (matches.Count > 0)
            {
                similarityPercentages[image] = 1.0;
                continue;
            }

            // Perform Hamming Distance calculation
            int distance = HammingDistance(pattern, referenceText);
            double similarity = 1.0 - (double)distance / pattern.Length;
            // double similarity = 1.0 - (double)distance / Math.Max(pattern.Length, referenceText.Length);
            similarityPercentages[image] = similarity;
        }

        return (matches, similarityPercentages);
    }

    public string FindMostSimilarFingerprint(string pattern, string[] referenceImages)
    {
        var result = Search(pattern, referenceImages);
        List<int> matches = result.matches;
        Dictionary<string, double> similarityPercentages = result.similarityPercentages;

        if (matches.Count > 0)
        {
            Console.WriteLine("Exact matches found:");
            
            foreach (int match in matches)
            {
                Console.WriteLine($"Pattern found at index: {match}");
            }
            // Find the image(s) where the exact match(es) were found
            string exactMatchImage = null;
            foreach (var kvp in similarityPercentages)
            {
                if (kvp.Value == 1.0)
                {
                    exactMatchImage = kvp.Key;
                    break;
                }
            }
            return exactMatchImage;
            // return path to image
            // return null;
        }
        else
        {
            double maxSimilarity = 0;
            string mostSimilarImage = null;

            // print similarityPercentages.Count
            Console.WriteLine(similarityPercentages.Count);
            foreach (var kvp in similarityPercentages)
            {   
                // print kvp.Value
                Console.WriteLine($"kvp.Value is {kvp.Value}");
                if (kvp.Value >= maxSimilarity)
                {
                    // print kvp.Value
                    Console.WriteLine(kvp.Value);
                    maxSimilarity = kvp.Value;
                    mostSimilarImage = kvp.Key;
                }
            }

            Console.WriteLine($"No exact match found. Most similar fingerprint is in image: {Path.GetFileName(mostSimilarImage)} with similarity {maxSimilarity * 100}%");
            return mostSimilarImage;
        }
    }
}
