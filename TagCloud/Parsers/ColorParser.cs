using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class ColorParser
    {
        public static Result<Color> ParseColor(string color)
        {
            var result = Color.FromName(color);
            if (!result.IsKnownColor)
            {
                return Result.Fail<Color>($"Неизвестный цвет {color}");
            }
            return result.AsResult();
        }
    }
}
