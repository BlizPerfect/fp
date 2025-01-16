using FileSenderRailway;
using System.Drawing;

namespace TagCloud.Parsers
{
    internal static class SizeParser
    {
        private static Result<Size> GetErrorInvalidSize(string size)
            => Result.Fail<Size>($"Некорректный формат размера изображения \"{size}\", используйте формат \"Ширина:Высота\"");

        public static Result<Size> ParseImageSize(string size)
        {
            if (string.IsNullOrWhiteSpace(size))
            {
                return GetErrorInvalidSize(size);
            }

            var dimensions = size.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (dimensions.Length != 2)
            {
                return GetErrorInvalidSize(size);
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
            if (string.IsNullOrWhiteSpace(dimension) || !int.TryParse(dimension, out var result))
            {
                return Result.Fail<int>($"Передано не число \"{dimension}\"");
            }
            return ParseSizeDimension(result);
        }

        public static Result<int> ParseSizeDimension(int dimension)
        {
            if (dimension <= 0)
            {
                return Result.Fail<int>($"Переданное числовое значение должно быть больше 0: \"{dimension}\"");
            }
            return dimension.AsResult();
        }
    }
}
