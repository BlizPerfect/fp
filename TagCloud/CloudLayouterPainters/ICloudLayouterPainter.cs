using FileSenderRailway;
using System.Drawing;

namespace TagCloud.CloudLayouterPainters
{
    // Интерфейс отрисовки прямоугольников
    internal interface ICloudLayouterPainter
    {
        public Result<Bitmap> Draw(IList<Tag> tags);
    }
}
