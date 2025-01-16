using FileSenderRailway;
using System.Drawing;
using TagCloud.CloudLayouterPainters;

namespace TagCloud.Factories
{
    internal interface ICloudLayouterPainterFactory
    {
        public Result<ICloudLayouterPainter> Create(
            Size imageSize,
            Color? backgroundColor = null,
            Color? textColor = null,
            FontFamily? fontName = null);
    }
}
