using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class FontParser
    {
        public static Result<string> ParseFont(string font)
        {
            if (!FontFamily.Families.Any(
                x => x.Name.Equals(font, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Fail<string>($"Неизвестный шрифт {font}");
            }
            return font.AsResult();
        }

    }
}
