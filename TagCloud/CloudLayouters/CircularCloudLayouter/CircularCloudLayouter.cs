using FileSenderRailway;
using System;
using System.Drawing;

namespace TagCloud.CloudLayouters.CircularCloudLayouter
{
    // Класс, со старого задания TagCloud,
    // который расставляет прямоугольники по окружности
    // с постепенно увеличивающимся радиусом.
    // Прямоугольники расставляются вокруг точки с координатой (0, 0),
    // Затем, в CloudLayouterPainter координат пересчитываются таким образом,
    // что бы расположить первый прямоугольник в центре холста.

    internal class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Circle arrangementСircle = new Circle();
        private readonly Random random = new Random();
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
            => ValidateRectangleSize(rectangleSize)
                .Then(size => PlaceRectangle(size))
                .OnFail(error => Result.Fail<Rectangle>(error));

        private Result<Size> ValidateRectangleSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                return Result.Fail<Size>("Размеры прямоугольника не могут быть меньше либо равны нуля");
            }

            return rectangleSize.AsResult();
        }

        private Result<Rectangle> PlaceRectangle(Size rectangleSize)
        {
            return Result.Of(() =>
            {
                var result = new Rectangle();
                arrangementСircle.Radius -= 1.0f;

                var isPlaced = false;
                while (!isPlaced)
                {
                    var startAngle = random.Next(360);
                    foreach (var coordinate in arrangementСircle.GetCoordinatesOnCircle(startAngle))
                    {
                        var location = GetRectangleLocation(coordinate, rectangleSize);
                        var nextRectangle = new Rectangle(location, rectangleSize);
                        if (!IsIntersectionWithAlreadyPlaced(nextRectangle))
                        {
                            rectangles.Add(nextRectangle);
                            isPlaced = true;
                            result = nextRectangle;
                            break;
                        }
                    }

                    arrangementСircle.Radius += 1.0f;
                }

                return result;
            });
        }

        private bool IsIntersectionWithAlreadyPlaced(Rectangle rectangle)
            => rectangles.Any(rect => rect.IntersectsWith(rectangle));

        private Point GetRectangleLocation(Point pointOnCircle, Size rectangleSize)
        {
            var x = pointOnCircle.X - rectangleSize.Width / 2;
            var y = pointOnCircle.Y - rectangleSize.Height / 2;
            return new Point(x, y);
        }
    }
}
