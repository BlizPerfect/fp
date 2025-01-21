using FileSenderRailway;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.CloudLayouterPainters;
using TagCloud.CloudLayouters.CircularCloudLayouter;
using TagCloud.ImageSavers;
using TagCloud.Tests.Extensions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.CloudLayoutersTests.CircularCloudLayouterTests
{
    [TestFixture]
    [SuppressMessage(
        "Interoperability",
        "CA1416:Проверка совместимости платформы",
        Justification = "Код предназначен для выполнения только на Windows 6.1 и новее")]
    internal class CircularCloudLayouterTests
    {
        private RectangleSetupper? rectangleSetupper;
        private Point center = new Point();
        private readonly string failedTestsDirectory = "FailedTest";

        private readonly ImageSaver imageSaver = new ImageSaver();
        private readonly CloudLayouterPainter cloudLayouterPainter
            = new CloudLayouterPainter(new Size(3000, 3000));

        [OneTimeSetUp]
        public void Init()
            => Directory.CreateDirectory(failedTestsDirectory);

        [Test]
        [Repeat(10)]
        public void ShouldPlaceRectanglesInCircle()
        {
            rectangleSetupper = new RectangleSetupper();
            var rectangles = rectangleSetupper.Rectangles();
            var expectedCoverageRatio = 0.7;
            var gridSize = 1000;

            var maxRadius = rectangles.Max(
                x => x.GetMaxDistanceFromPointToRectangleAngles(center));
            var step = 2 * maxRadius / gridSize;

            var occupancyGrid = GetOccupancyGrid(gridSize, maxRadius, step, rectangles);

            var actualCoverageRatio = GetOccupancyGridRatio(occupancyGrid, maxRadius, step);
            actualCoverageRatio.Should().BeGreaterThanOrEqualTo(expectedCoverageRatio);
        }

        [Test]
        [Repeat(10)]
        public void ShouldPlaceCenterOfMassOfRectanglesNearCenter()
        {
            rectangleSetupper = new RectangleSetupper();
            var rectangles = rectangleSetupper.Rectangles();
            var tolerance = 15;

            var centerX = rectangles.Average(r => r.Left + r.Width / 2.0);
            var centerY = rectangles.Average(r => r.Top + r.Height / 2.0);
            var actualCenter = new Point((int)centerX, (int)centerY);

            var distance = Math.Sqrt(Math.Pow(actualCenter.X - center.X, 2)
                                     + Math.Pow(actualCenter.Y - center.Y, 2));

            distance.Should().BeLessThanOrEqualTo(tolerance);
        }

        [Test]
        [Repeat(10)]
        public void ShouldPlaceRectanglesWithoutOverlap()
        {
            rectangleSetupper = new RectangleSetupper();
            var rectangles = rectangleSetupper.Rectangles();
            for (var i = 0; i < rectangles.Length; i++)
            {
                for (var j = i + 1; j < rectangles.Length; j++)
                {
                    Assert.That(
                        rectangles[i].IntersectsWith(rectangles[j]),
                        Is.EqualTo(false),
                        $"Прямоугольники пересекаются:\n" +
                        $"{rectangles[i]}\n" +
                        $"{rectangles[j]}");
                }
            }
        }

        [TestCase(0, 100)]
        [TestCase(-1, 100)]
        [TestCase(100, 0)]
        [TestCase(100, -1)]
        public void PutNextRectangle_ThrowsException_OnAnyNegativeOrZeroSize(
            int width,
            int height)
        {
            rectangleSetupper = null;
            var size = new Size(width, height);
            var expected = Result.Fail<Rectangle>(
                "Размеры прямоугольника не могут быть меньше либо равны нуля");
            var actual = new CircularCloudLayouter().PutNextRectangle(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TearDown]
        public void Cleanup()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0
                || rectangleSetupper is null)
            {
                return;
            }

            var name = $"{TestContext.CurrentContext.Test.Name}.png";
            var path = Path.Combine(failedTestsDirectory, name);
            imageSaver.SaveFile(
                cloudLayouterPainter.Draw(rectangleSetupper.Tags).GetValueOrThrow(), path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            if (Directory.Exists(failedTestsDirectory)
                && Directory.GetFiles(failedTestsDirectory).Length == 0)
            {
                Directory.Delete(failedTestsDirectory);
            }
        }

        private (int start, int end) GetGridIndexesInterval(
            int rectangleStartValue,
            int rectangleCorrespondingSize,
            double maxRadius,
            double step)
        {
            var start = (int)((rectangleStartValue - center.X + maxRadius) / step);
            var end = (int)((rectangleStartValue
                + rectangleCorrespondingSize - center.X + maxRadius) / step);
            return (start, end);
        }

        private bool[,] GetOccupancyGrid(
            int gridSize,
            double maxRadius,
            double step,
            Rectangle[] rectangles)
        {
            var result = new bool[gridSize, gridSize];
            foreach (var rect in rectangles)
            {
                var xInterval = GetGridIndexesInterval(rect.X, rect.Width, maxRadius, step);
                var yInterval = GetGridIndexesInterval(rect.Y, rect.Height, maxRadius, step);
                for (var x = xInterval.start; x <= xInterval.end; x++)
                {
                    for (var y = yInterval.start; y <= yInterval.end; y++)
                    {
                        result[x, y] = true;
                    }
                }
            }
            return result;
        }

        private double GetOccupancyGridRatio(bool[,] occupancyGrid, double maxRadius, double step)
        {
            var totalCellsInsideCircle = 0;
            var coveredCellsInsideCircle = 0;
            for (var x = 0; x < occupancyGrid.GetLength(0); x++)
            {
                for (var y = 0; y < occupancyGrid.GetLength(0); y++)
                {
                    var cellCenterX = x * step - maxRadius + center.X;
                    var cellCenterY = y * step - maxRadius + center.Y;

                    var distance = Math.Sqrt(
                        Math.Pow(cellCenterX - center.X, 2) + Math.Pow(cellCenterY - center.Y, 2));

                    if (distance > maxRadius)
                    {
                        continue;
                    }

                    totalCellsInsideCircle += 1;
                    if (occupancyGrid[x, y])
                    {
                        coveredCellsInsideCircle += 1;
                    }
                }
            }
            return (double)coveredCellsInsideCircle / totalCellsInsideCircle;
        }
    }
}
