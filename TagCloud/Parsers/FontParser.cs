using FileSenderRailway;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagCloud.Parsers
{
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
    internal static class FontParser
    {
        public static Result<FontFamily> ParseFont(string font)
        {
            if (string.IsNullOrWhiteSpace(font))
            {
                return GetError(font);
            }
            if (!FontFamily.Families.Any(
                x => x.Name.Equals(font, StringComparison.OrdinalIgnoreCase)))
            {
                return GetError(font);
            }
            return new FontFamily(font).AsResult();
        }

        private static Result<FontFamily> GetError(string font)
            => Result.Fail<FontFamily>($"Неизвестный шрифт \"{font}\"");
    }
}
