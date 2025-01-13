using FileSenderRailway;
using System.Drawing;

namespace TagCloud.CloudLayouters
{
    // Интерфейс расстановки прямоугольников
    internal interface ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}
