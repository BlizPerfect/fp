using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouters.CircularCloudLayouter;

namespace TagCloud.Tests.CloudLayoutersTests.CircularCloudLayouterTests
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        [TestCase(0, 100)]
        [TestCase(-1, 100)]
        [TestCase(100, 0)]
        [TestCase(100, -1)]
        public void PutNextRectangle_ThrowsException_OnAnyNegativeOrZeroSize(
            int width,
            int height)
        {
            var size = new Size(width, height);
            var expected = Result.Fail<Rectangle>(
                "Размеры прямоугольника не могут быть меньше либо равны нуля");
            var actual = new CircularCloudLayouter().PutNextRectangle(size);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
