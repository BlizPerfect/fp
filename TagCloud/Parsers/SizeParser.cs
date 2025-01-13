using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class SizeParser
    {
        public static Result<Size> ParseImageSize(string size)
        {
            var dimensions = size.Split(':');
            if (dimensions.Length != 2)
            {
                return Result.Fail<Size>($"Некорректный формат размера изображения: {size}, используйте формат Ширина:Высота, например 5000:5000");
            }

            var width = ParseSizeDimension(dimensions[0]).GetValueOrThrow();
            var height = ParseSizeDimension(dimensions[1]).GetValueOrThrow();

            return new Size(width, height).AsResult();
        }

        public static Result<int> ParseSizeDimension(string dimension)
        {
            if (!int.TryParse(dimension, out var result))
            {
                return Result.Fail<int>($"Передано не число: {dimension}");
            }
            return ParseSizeDimension(result);
        }

        public static Result<int> ParseSizeDimension(int dimension)
        {
            if (dimension <= 0)
            {
                return Result.Fail<int>($"Переданное числовое значение должно быть больше 0: {dimension}");
            }
            return dimension.AsResult();
        }
    }
}
