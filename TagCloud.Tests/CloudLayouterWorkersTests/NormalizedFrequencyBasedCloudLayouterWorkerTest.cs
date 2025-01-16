using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouterWorkers;

namespace TagCloud.Tests.CloudLayouterWorkersTests
{
    internal class NormalizedFrequencyBasedCloudLayouterWorkerTest
    {
        private readonly Dictionary<string, double> normalizedValues
            = new Dictionary<string, double>
            {
                { "three", 0.625 },
                { "one", 0.25 },
                { "two", 0.2917 },
                { "four", 1.0 },
            };

        [TestCase(0, 100)]
        [TestCase(-1, 100)]
        [TestCase(100, 0)]
        [TestCase(100, -1)]
        public void GetNextRectangleSize_ThrowsException_OnAnyNegativeOrZeroSize(
            int width,
            int height)
        {
            var expected = Result
                .Fail<IEnumerable<(string word, Size size)>>
                    ($"Переданное числовое значение должно быть больше 0: \"{(width <= 0 ? width : height)}\"");

            var worker = new NormalizedFrequencyBasedCloudLayouterWorker(
                width,
                height,
                normalizedValues);
            var actual = worker.GetNextRectangleProperties();
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(100, 25, false)]
        [TestCase(100, 25, true)]
        public void GetNextRectangleSize_WorksCorrectly(int width, int height, bool isSortedOrder)
        {
            var index = 0;
            string[]? keys = null;
            if (isSortedOrder)
            {
                keys = normalizedValues
                    .OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            }
            else
            {
                keys = normalizedValues.Keys.ToArray();
            }

            var worker = new NormalizedFrequencyBasedCloudLayouterWorker(
                width,
                height,
                normalizedValues,
                isSortedOrder);
            foreach (var rectangleSize in worker
                .GetNextRectangleProperties().GetValueOrThrow())
            {
                var currentValue = normalizedValues[keys[index]];
                var expected = new Size((int)(currentValue * width), (int)(currentValue * height));
                index += 1;
                rectangleSize.size.Should().BeEquivalentTo(expected);
            }
        }
    }
}
