using System.Drawing;

namespace TagCloud.CloudLayouters
{
    // Интерфейс расстановки прямоугольников
    internal interface ICloudLayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}
