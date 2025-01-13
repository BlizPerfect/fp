using FluentAssertions;
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
        public void GetNextRectangleSize_ThrowsArgumentException_OnAnyNegativeOrZeroSize(
            int width,
            int height)
        {
            var message = $"Переданное числовое значение должно быть больше 0: {(width <= 0 ? width : height)}";
            var exception = Assert.Throws<InvalidOperationException>(
                () => new RandomCloudLayouterWorker(width, width, height, height));
            exception.Message.Should().Contain(message);
        }

        [TestCase(50, 25, 25, 50)]
        [TestCase(25, 50, 50, 25)]
        public void GetNextRectangleSize_ThrowsArgumentException_OnNonConsecutiveSizeValues(
            int minWidth,
            int maxWidth,
            int minHeight,
            int maxHeight)
        {
            var message = "Минимальное значение не может быть больше максимального";
            var exception = Assert.Throws<InvalidOperationException>(
                () => new RandomCloudLayouterWorker(minWidth, maxWidth, minHeight, maxHeight));
            exception.Message.Should().Contain(message);
        }
    }
}
