using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class SizeParser
    {
        public static Result<Size> ParseImageSize(string size)
        {
            if (string.IsNullOrWhiteSpace(size))
            {
                return Result.Fail<Size>($"Некорректная строка {size}");
            }

            var dimensions = size.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (dimensions.Length != 2)
            {
                return Result.Fail<Size>($"Некорректный формат размера изображения: \"{size}\", используйте формат \"Ширина:Высота\", например 5000:5000");
            }

            var width = ParseSizeDimension(dimensions[0]);
            if (!width.IsSuccess)
            {
                return Result.Fail<Size>(width.Error);
            }

            var height = ParseSizeDimension(dimensions[1]);
            if (!height.IsSuccess)
            {
                return Result.Fail<Size>(height.Error);
            }

            return new Size(width.Value, height.Value).AsResult();
        }

        public static Result<int> ParseSizeDimension(string dimension)
        {
            if (string.IsNullOrWhiteSpace(dimension))
            {
                return Result.Fail<int>($"Некорректная строка {dimension}");
            }
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
