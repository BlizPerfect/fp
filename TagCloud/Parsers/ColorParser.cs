using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class ColorParser
    {
        public static Result<Color> ParseColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return GetError(color);
            }
            var result = Color.FromName(color);
            if (!result.IsKnownColor)
            {
                return GetError(color);
            }
            return result.AsResult();
        }

        private static Result<Color> GetError(string color)
            => Result.Fail<Color>($"Неизвестный цвет \"{color}\"");
    }
}
