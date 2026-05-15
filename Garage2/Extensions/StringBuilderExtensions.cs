using System.Text;

namespace Garage2.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder PrependLine(this StringBuilder sb, string content)
    {
        return sb.Insert(0, $"{content}{Environment.NewLine}");
    }

    public static StringBuilder InsertLine(this StringBuilder sb, int lineIndex, string content)
    {
        if (lineIndex < 0)
        {
            throw new ArgumentOutOfRangeException($"lineIndex can't be negative: {nameof(lineIndex)}");
        }

        string newLine = Environment.NewLine;
        int newLineLength = newLine.Length;

        if (lineIndex == 0)
        {
            return sb.PrependLine(content);
        }

        int currentLine = 0;
        for (int i = 0; i < sb.Length; i++)
        {
            if (
                sb[i] == newLine[0] &&
                (newLineLength == 1 || (i + 1 < sb.Length && sb[i + 1] == newLine[1]))
                )
            {
                currentLine++;
                if (currentLine == lineIndex)
                {
                    return sb.Insert(i + newLineLength, content + newLine);
                }
            }
        }

        if (sb.Length > 0 && !EndsWithNewline(sb, newLine))
        {
            sb.Append(newLine);
        }
        return sb.AppendLine(content);
    }

    private static bool EndsWithNewline(StringBuilder sb, string newLine)
    {
        if (sb.Length < newLine.Length)
        {
            return false;
        }

        for (int i = 0; i < newLine.Length; i++)
        {
            if (sb[sb.Length - newLine.Length + i] != newLine[i])
            {
                return false;
            }
        }

        return true;
    }
}
