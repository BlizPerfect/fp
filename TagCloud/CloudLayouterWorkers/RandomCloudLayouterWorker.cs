using FileSenderRailway;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.CloudLayouterWorkers
{
    internal class RandomCloudLayouterWorker(
        int minRectangleWidth,
        int maxRectangleWidth,
        int minRectangleHeight,
        int maxRectangleHeight) : ICloudLayouterWorker
    {
        private readonly Random random = new Random();
        public Result<IEnumerable<(string word, Size size)>> GetNextRectangleProperties()
            => ValidateDimensions()
                .Then(_ => GenerateRectangles())
                .OnFail(error => Result.Fail<IEnumerable<(string word, Size size)>>(error));

        private Result<None> ValidateDimensions()
            => AreMinAndMaxSizesAppropriate(minRectangleWidth, maxRectangleWidth)
                .Then(_ => AreMinAndMaxSizesAppropriate(minRectangleHeight, maxRectangleHeight))
                .Then(_ => ParseSizes())
                .OnFail(error => Result.Fail<None>(error));

        private Result<None> ParseSizes()
            => SizeParser.ParseSizeDimension(minRectangleWidth)
                .Then(_ => SizeParser.ParseSizeDimension(maxRectangleWidth)
                .Then(_ => SizeParser.ParseSizeDimension(minRectangleHeight)
                .Then(_ => SizeParser.ParseSizeDimension(maxRectangleHeight)
                .Then(_ => Result.Ok()))))
                .OnFail(error => Result.Fail<None>(error));

        private static Result<bool> AreMinAndMaxSizesAppropriate(int min, int max)
        {
            if (min > max)
            {
                return Result.Fail<bool>("Минимальное значение не может быть больше максимального");
            }
            return true.AsResult();
        }

        private IEnumerable<(string word, Size size)> GenerateRectangles()
        {
            while (true)
            {
                var width = random.Next(minRectangleWidth, maxRectangleWidth);
                var height = random.Next(minRectangleHeight, maxRectangleHeight);
                yield return (string.Empty, new Size(width, height));
            }
        }
    }
}