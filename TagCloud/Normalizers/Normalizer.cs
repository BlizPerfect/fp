using FileSenderRailway;

namespace TagCloud.Normalizers
{
    // Слово, которое встречается чаще всего, будет иметь вес 1.0.
    // Это означает, что оно в дальнейшем будет иметь прямоугольник
    // с максимальным размером.
    // Слово с минимальной частотой будет иметь
    // minCoefficient *  максимальный размеро прямоугольника.
    internal class Normalizer : INormalizer
    {
        public Result<Dictionary<string, double>> Normalize(
            Dictionary<string, uint> values,
            double minCoefficient = 0.25,
            int decimalPlaces = 4)
            => ValidateInput(values, minCoefficient, decimalPlaces)
                .Then(_ => CalculateNormalizedValues(values, minCoefficient, decimalPlaces))
                .OnFail(error => Result.Fail<Dictionary<string, double>>(error));

        private static Result<None> ValidateInput(
            Dictionary<string, uint> values,
            double minCoefficient,
            int decimalPlaces)
        {
            if (values is null || values.Count == 0)
            {
                return Result.Fail<None>("Словарь значений не может быть пустым");
            }

            if (minCoefficient < 0.0 || minCoefficient > 1.0)
            {
                return Result.Fail<None>(
                    "Минимальный коэффициент нормализации должен быть в диапазоне от 0 до 1");
            }

            if (decimalPlaces < 0)
            {
                return Result.Fail<None>(
                    "Количество знаков после запятой не может быть отрицательным");
            }

            return Result.Ok();
        }

        private Result<Dictionary<string, double>> CalculateNormalizedValues(
            Dictionary<string, uint> values,
            double minCoefficient,
            int decimalPlaces)
            => Result.Of(() =>
            {
                var result = new Dictionary<string, double>();

                var maxValue = values.Values.Max();
                var minValue = values.Values.Min();

                var scale = 1.0 - minCoefficient;

                foreach (var pair in values)
                {
                    result[pair.Key] = CalculateNormalizedValue(
                        minCoefficient,
                        scale,
                        pair.Value,
                        minValue,
                        maxValue,
                        decimalPlaces);
                }

                return result;
            });


        private static double CalculateNormalizedValue(
            double minCoefficient,
            double scale,
            uint value,
            uint minValue,
            uint maxValue,
            int decimalPlaces)
        {
            if (minValue == maxValue)
            {
                return 1.0;
            }

            return Math.Round(
                minCoefficient + scale * ((double)(value - minValue) / (maxValue - minValue)),
                decimalPlaces);
        }
    }
}