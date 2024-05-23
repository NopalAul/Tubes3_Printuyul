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
        s1 = s1.Normalize(NormalizationForm.FormC);
        s2 = s2.Normalize(NormalizationForm.FormC);

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
    }

    public (List<int> matches, Dictionary<string, double> similarityPercentages) Search(
        string pattern1, 
        string pattern2, 
        Dictionary<string, (string, string)> referenceImagesMap,
        Dictionary<string, (string, string)> croppedReferenceImagesMap)
    {
        List<int> matches = new List<int>();
        Dictionary<string, double> similarityPercentages = new Dictionary<string, double>();

        // Iterate through the reference images map
        foreach (var kvp in referenceImagesMap)
        {
            string imagePath = kvp.Key;
            var (referenceText1, referenceText2) = kvp.Value;

            // Perform exact matching using the chosen algorithm on both patterns
            List<int> algorithmMatches1 = algorithm == "KMP" ? kmp.KMP(referenceText1, pattern1) : bm.BM(referenceText1, pattern1);
            List<int> algorithmMatches2 = algorithm == "KMP" ? kmp.KMP(referenceText2, pattern2) : bm.BM(referenceText2, pattern2);

            if (algorithmMatches1.Count > 0 && algorithmMatches2.Count > 0)
            {
                matches.AddRange(algorithmMatches1);
                matches.AddRange(algorithmMatches2);
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
                var (croppedReferenceText1, croppedReferenceText2) = kvp.Value;

                // Perform Hamming Distance calculation on both patterns
                int distance1 = HammingDistance(pattern1, croppedReferenceText1);
                int distance2 = HammingDistance(pattern2, croppedReferenceText2);
                double similarity1 = 1.0 - (double)distance1 / pattern1.Length;
                double similarity2 = 1.0 - (double)distance2 / pattern2.Length;
                double similarity = (similarity1 + similarity2) / 2;
                similarityPercentages[imagePath] = similarity;
            }
        }

        return (matches, similarityPercentages);
    }

    public (string mostSimilarImage, double maxSimilarity) FindMostSimilarFingerprint(
        string pattern1, 
        string pattern2, 
        Dictionary<string, (string, string)> referenceImagesMap,
        Dictionary<string, (string, string)> croppedReferenceImagesMap)
    {
        var result = Search(pattern1, pattern2, referenceImagesMap, croppedReferenceImagesMap);
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
                if (kvp.Value >= maxSimilarity)
                {
                    maxSimilarity = kvp.Value;
                    mostSimilarImage = kvp.Key;
                }
            }

            maxSimilarity *= 100;
            Console.WriteLine($"No exact match found. Most similar fingerprint is in image: {Path.GetFileName(mostSimilarImage)} with similarity {maxSimilarity}%");
            return (mostSimilarImage, maxSimilarity);
        }
    }
}
