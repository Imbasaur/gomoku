using System.Text.RegularExpressions;

namespace Gomoku.Core.Extensions;
public static class StringExtensions
{
    public static IEnumerable<ReadOnlyMemory<char>> SplitInParts(this string s, int partLength)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (partLength <= 0)
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));

        for (var i = 0; i < s.Length; i += partLength)
            yield return s.AsMemory().Slice(i, Math.Min(partLength, s.Length - i));
    }

    public static IEnumerable<string> SplitMoves(this string moves) => Regex.Split(moves, "([a-o][0-9]+)").Where(s => s.Length > 0);
    public static IEnumerable<string> GetLastPlayerMoves(this string moves)
    {
        var movesList = Regex.Split(moves, "([a-o][0-9]+)").Where(s => s.Length > 0);

        return movesList.Where((v, i) => i % 2 == (movesList.Count()-1) % 2);
    }
}
