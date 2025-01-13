using FileSenderRailway;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.CloudLayouterWorkers
{
    // Класс, со старого задания TagCloud,
    // выдающий случайный размер прямоугольника
    // Оставил его для пары тестов.
    internal class RandomCloudLayouterWorker : ICloudLayouterWorker
    {
        private Random random = new Random();
        public readonly int MinRectangleWidth;
        public readonly int MaxRectangleWidth;
        public readonly int MinRectangleHeight;
        public readonly int MaxRectangleHeight;

        public RandomCloudLayouterWorker(
            int minRectangleWidth,
            int maxRectangleWidth,
            int minRectangleHeight,
            int maxRectangleHeight)
        {
            if (AreMinAndMaxSizesAppropriate(minRectangleWidth, maxRectangleWidth).GetValueOrThrow()
                && AreMinAndMaxSizesAppropriate(minRectangleHeight, maxRectangleHeight).GetValueOrThrow())
            {
                MinRectangleWidth = SizeParser.ParseSizeDimension(minRectangleWidth).GetValueOrThrow();
                MaxRectangleWidth = SizeParser.ParseSizeDimension(maxRectangleWidth).GetValueOrThrow();
                MinRectangleHeight = SizeParser.ParseSizeDimension(minRectangleHeight).GetValueOrThrow();
                MaxRectangleHeight = SizeParser.ParseSizeDimension(maxRectangleHeight).GetValueOrThrow();
            }
        }

        private Result<bool> AreMinAndMaxSizesAppropriate(int min, int max)
        {
            if (min > max)
            {
                return Result.Fail<bool>("Минимальное значение не может быть больше максимального");
            }
            return true.AsResult();
        }

        public IEnumerable<(string word, Size size)> GetNextRectangleProperties()
        {
            while (true)
            {
                var width = random.Next(MinRectangleWidth, MaxRectangleWidth);
                var height = random.Next(MinRectangleHeight, MaxRectangleHeight);
                yield return (string.Empty, new Size(width, height));
            }
        }
    }
}
