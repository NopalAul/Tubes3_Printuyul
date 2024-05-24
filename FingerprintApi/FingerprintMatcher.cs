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
                int distance = HammingDistance(pattern, croppedReferenceText);
                // // print pattern and croppedReferenceText
                // Console.WriteLine($"pattern: {pattern}");
                // Console.WriteLine($"croppedReferenceText: {croppedReferenceText}");
                double similarity = 1.0 - (double)distance / pattern.Length;
                similarityPercentages[imagePath] = similarity;
            }
        }

        return (matches, similarityPercentages);
    }

    public (string mostSimilarImage, double maxSimilarity) FindMostSimilarFingerprint(
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
                // print index found
                Console.WriteLine($"Pattern found at index: {match}");
            }
            // Find the image(s) where the exact match(es) were found
            string exactMatchImage = null;
            foreach (var kvp in similarityPercentages)
            {
                if (kvp.Value == 1.0)
                {   
                    exactMatchImage = kvp.Key;
                    Console.WriteLine($"Exact match found in image: {exactMatchImage}");
                    break;
                }
            }

            double maxSimilarity = 100;
            return (exactMatchImage, maxSimilarity);
        }
        else
        {
            double maxSimilarity = 0;
            string mostSimilarImage = null;

            Console.WriteLine(similarityPercentages.Count);
            foreach (var kvp in similarityPercentages)
            {
                // Console.WriteLine($"kvp.Value is {kvp.Value}");
                if (kvp.Value >= maxSimilarity)
                {
                    // Console.WriteLine(kvp.Value);
                    maxSimilarity = kvp.Value;
                    mostSimilarImage = kvp.Key;
                }
            }

            maxSimilarity *= 100;
            Console.WriteLine($"No exact match found. Most similar fingerprint is in image: {Path.GetFileName(mostSimilarImage)} with similarity {maxSimilarity * 100}%");
            return (mostSimilarImage, maxSimilarity);
        }
    }
}
