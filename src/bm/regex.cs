using System;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string realName = "bintang";
        string weirdNameRegex = CreateWeirdNameRegex(realName);

        Console.WriteLine($"Regex for {realName}: {weirdNameRegex}");
        
        string[] weirdNames = { "bintanG DwI mArthen", "B1nt4n6 Dw1 M4rthen", "Bntng Dw Mrthen", "b1ntN6 Dw mrthn", "D3N1s3", "dnse", "b1N74N6", "bintng", "bintang", "Bintang", "n0p4l" };
        foreach (var weirdName in weirdNames)
        {
            bool match = Regex.IsMatch(weirdName, weirdNameRegex, RegexOptions.IgnoreCase);
            Console.WriteLine($"{weirdName} matches: {match}");
        }
    }

    public static string CreateWeirdNameRegex(string realName)
    {
        // create regex pattern
        string pattern = "";
        foreach (char c in realName)
        {
            pattern += GetCharPattern(c);
        }

        // tutup dengan ^ dan $ supaya dia harus match seluruh string (bukan partial)
        return $"^{pattern}$";
    }

    private static string GetCharPattern(char c)
    {
        return c switch
        {
            'a' => "[aA4]?",
            'e' => "[eE3]?",
            'i' => "[iI1]?",
            'o' => "[oO0]?",
            'u' => "[uU]?",
            'b' => "[bB8]",
            'c' => "[cC]",
            'd' => "[dD]",
            'f' => "[fF]",
            'g' => "[gG6]",
            'h' => "[hH]",
            'j' => "[jJ]",
            'k' => "[kK]",
            'l' => "[lL]",
            'm' => "[mM]",
            'n' => "[nN]",
            'p' => "[pP]",
            'q' => "[qQ]",
            'r' => "[rR]",
            's' => "[sS5]",
            't' => "[tT7]",
            'v' => "[vV]",
            'w' => "[wW]",
            'x' => "[xX]",
            'y' => "[yY]",
            'z' => "[zZ]",
            'A' => "[aA4]?",
            'E' => "[eE3]?",
            'I' => "[iI1]?",
            'O' => "[oO0]?",
            'U' => "[uU]?",
            'B' => "[bB8]",
            'C' => "[cC]",
            'D' => "[dD]",
            'F' => "[fF]",
            'G' => "[gG6]",
            'H' => "[hH]",
            'J' => "[jJ]",
            'K' => "[kK]",
            'L' => "[lL]",
            'M' => "[mM]",
            'N' => "[nN]",
            'P' => "[pP]",
            'Q' => "[qQ]",
            'R' => "[rR]",
            'S' => "[sS5]",
            'T' => "[tT7]",
            'V' => "[vV]",
            'W' => "[wW]",
            'X' => "[xX]",
            'Y' => "[yY]",
            'Z' => "[zZ]",
            _ => c.ToString()
        };
    }
}
