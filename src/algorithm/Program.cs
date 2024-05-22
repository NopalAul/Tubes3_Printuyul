using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // Example usage
        // string dummyAsciiString = "iÿÿÿÿÿÿÿÿdajdoeiaoidjeaisdmasdmsadsaifjiweab";
        string pattern = "inilobener";

        // Simulate reference images with dummy ASCII strings
        string[] referenceImages = { "reference1.txt", "reference2.txt", "reference3.txt" };

        // Write dummy ASCII strings to reference files for simulation
        File.WriteAllText("reference1.txt", "iÿÿÿÿÿÿÿÿÿÿÌÿÿû:Ãÿÿ<Ìÿÿuwûÿkÿý,Pþÿ¿/ÿÿ");
        File.WriteAllText("reference2.txt", "iÿÿÿasdiajsudheurfheuafbeiybfiayebfiayef");
        File.WriteAllText("reference3.txt", "iniloÿÿÿÿÿÿÿÿdajdoeiaoidjeinilobeneaisdmas");
        // File.WriteAllText("reference4.txt", "inilobener");

        // Choose algorithm: "KMP" or "BM"
        string algorithmChoice = "KMP";

        FingerprintMatcher matcher = new FingerprintMatcher(algorithmChoice);
        string result = matcher.FindMostSimilarFingerprint(pattern, referenceImages);   
        Console.WriteLine(result);

    }
}
