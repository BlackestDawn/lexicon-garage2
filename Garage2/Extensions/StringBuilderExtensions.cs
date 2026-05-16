using System.Text;

namespace Garage2.Extensions;

public static class StringBuilderExtensions
{
    private static readonly string _newLine = Environment.NewLine;

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

        int newLineLength = _newLine.Length;

        if (lineIndex == 0)
        {
            return sb.PrependLine(content);
        }

        int currentLine = 0;
        for (int i = 0; i < sb.Length; i++)
        {
            if (
                sb[i] == _newLine[0] &&
                (newLineLength == 1 || (i + 1 < sb.Length && sb[i + 1] == _newLine[1]))
                )
            {
                currentLine++;
                if (currentLine == lineIndex)
                {
                    return sb.Insert(i + newLineLength, content + _newLine);
                }
            }
        }

        if (sb.Length > 0 && !EndsWithNewline(sb, _newLine))
        {
            sb.Append(_newLine);
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

    public static StringBuilder AppendToLine(this StringBuilder sb, int lineIndex, string content)
    {
        if (lineIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lineIndex));
        }

        int newLineLength = _newLine.Length;
        int currentLine = 0;

        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] == _newLine[0] && (newLineLength == 1 || (i + 1 < sb.Length && sb[i + 1] == _newLine[1])))
            {
                if (currentLine == lineIndex)
                {
                    return sb.Insert(i, content);
                }
                currentLine++;
                i += newLineLength - 1;
            }
        }

        if (currentLine == lineIndex)
        {
            return sb.Append(content);
        }

        throw new ArgumentOutOfRangeException(nameof(lineIndex), "Line index not found.");
    }
}
