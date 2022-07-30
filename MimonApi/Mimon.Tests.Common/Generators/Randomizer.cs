using System.Text;

namespace Mimon.Tests.Common.Generators;

public class Randomizer
{
    public static int GenerateNumber(int from, int to, bool includeBorder = false)
    {
        return Random.Next(from, to + (includeBorder ? 1 : 0));
    }

    public static string GenerateString(int length, bool includeCapitalLetters = false)
    {
        var alphabet = (includeCapitalLetters
                ? GenerateLowerAlphabet().Concat(GenerateUpperAlphabet())
                : GenerateLowerAlphabet())
            .ToArray();

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            stringBuilder.Append(alphabet[GenerateNumber(0, alphabet.Length)]);
        }

        return stringBuilder.ToString();
    }

    public static bool Yep()
    {
        return GenerateNumber(0, 2) == 0;
    }

    private static readonly Random Random = new();
    private static IEnumerable<char> GenerateLowerAlphabet() => Enumerable.Range('a', 26).Select(x => (char)x);
    private static IEnumerable<char> GenerateUpperAlphabet() => Enumerable.Range('A', 26).Select(x => (char)x);
}