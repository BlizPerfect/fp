using FileSenderRailway;
using System.Drawing;
using TagCloud.CloudLayouterPainters;


namespace TagCloud.Factories
{
    internal class CloudLayouterPainterFactory : ICloudLayouterPainterFactory
    {
        public Result<ICloudLayouterPainter> Create(
            Size imageSize,
            Color? backgroundColor = null,
            Color? textColor = null,
            FontFamily? fontName = null)
            => Result.Ok<ICloudLayouterPainter>(
                new CloudLayouterPainter(imageSize, backgroundColor, textColor, fontName));

    }
}
