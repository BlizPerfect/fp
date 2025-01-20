using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouterWorkers;

namespace TagCloud.Tests.CloudLayouterWorkersTests
{
    [TestFixture]
    internal class CircularCloudLayouterWorkerTests
    {
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

            var worker = new RandomCloudLayouterWorker(width, width, height, height);
            var actual = worker.GetNextRectangleProperties();
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(50, 25, 25, 50)]
        [TestCase(25, 50, 50, 25)]
        public void GetNextRectangleSize_ThrowsException_OnNonConsecutiveSizeValues(
            int minWidth,
            int maxWidth,
            int minHeight,
            int maxHeight)
        {
            var expected = Result
                .Fail<IEnumerable<(string word, Size size)>>
                    ("Минимальное значение не может быть больше максимального");

            var worker = new RandomCloudLayouterWorker(minWidth, maxWidth, minHeight, maxHeight);
            var actual = worker.GetNextRectangleProperties();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
