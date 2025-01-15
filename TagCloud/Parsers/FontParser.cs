using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class FontParser
    {
        public static Result<FontFamily> ParseFont(string font)
        {
            if (string.IsNullOrWhiteSpace(font))
            {
                return Result.Fail<FontFamily>($"Некорректная строка {font}");
            }
            if (!FontFamily.Families.Any(
                x => x.Name.Equals(font, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Fail<FontFamily>($"Неизвестный шрифт {font}");
            }
            return new FontFamily(font).AsResult();
        }
    }
}
