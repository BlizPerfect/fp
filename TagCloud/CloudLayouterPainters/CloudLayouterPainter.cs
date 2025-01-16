using FileSenderRailway;
using System.Drawing;

namespace TagCloud.CloudLayouterPainters
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
    internal class CloudLayouterPainter(
        Size imageSize,
        Color? backgroundColor = null,
        Color? textColor = null,
        FontFamily? fontName = null) : ICloudLayouterPainter
    {
        private readonly Color backgroundColor = backgroundColor ?? Color.White;
        private readonly Color textColor = textColor ?? Color.Black;
        private readonly FontFamily fontName = fontName ?? new FontFamily("Arial");

        public Result<Bitmap> Draw(IList<Tag> tags)
            => ValidateTags(tags)
                .Then(validTags => CreateBitmap(validTags))
                .OnFail(error => Result.Fail<Bitmap>(error));

        private Result<IList<Tag>> ValidateTags(IList<Tag> tags)
        {
            if (tags is null)
            {
                return Result.Fail<IList<Tag>>("Tags передан как null");
            }

            if (tags.Count == 0)
            {
                return Result.Fail<IList<Tag>>("Список тегов пуст");
            }

            if (!DoRectanglesFit(tags))
            {
                return Result
                    .Fail<IList<Tag>>("Все прямоугольники не помещаются на изображение");
            }

            return tags.AsResult();
        }

        private Result<Bitmap> CreateBitmap(IList<Tag> tags)
            => Result.Of(() =>
            {
                var bitmap = new Bitmap(imageSize.Width, imageSize.Height);

                using var graphics = Graphics.FromImage(bitmap);
                graphics.Clear(backgroundColor);

                foreach (var tag in tags)
                {
                    var positionOnCanvas = GetPositionOnCanvas(tag.Rectangle);
                    var rectOnCanvas = new Rectangle(
                        positionOnCanvas.X,
                        positionOnCanvas.Y,
                        tag.Rectangle.Width,
                        tag.Rectangle.Height);

                    DrawText(graphics, rectOnCanvas, tag.Text);
                }

                return bitmap;
            });

        private bool DoRectanglesFit(IList<Tag> tags)
        {
            var minimums = new Point(
                tags.Min(t => t.Rectangle.Left),
                tags.Min(t => t.Rectangle.Top));

            var maximums = new Point(
                tags.Max(t => t.Rectangle.Right),
                tags.Max(t => t.Rectangle.Bottom));

            var actualWidth = maximums.X - minimums.X;
            var actualHeight = maximums.Y - minimums.Y;
            return actualWidth < imageSize.Width && actualHeight < imageSize.Height;
        }

        private Point GetPositionOnCanvas(Rectangle rectangle)
            => new Point(rectangle.X + imageSize.Width / 2, rectangle.Y + imageSize.Height / 2);

        private void DrawText(Graphics graphics, Rectangle rectangle, string text)
        {
            var fontSize = FindFittingFontSize(graphics, text, rectangle);
            using var fittingFont = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            using var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using var brush = new SolidBrush(textColor);

            graphics.DrawString(text, fittingFont, brush, rectangle, stringFormat);
        }

        private int FindFittingFontSize(Graphics graphics, string text, Rectangle rectangle)
        {
            var minSize = 1;
            var maxSize = Math.Min(rectangle.Width, rectangle.Height);
            var result = minSize;

            while (minSize <= maxSize)
            {
                var midSize = (minSize + maxSize) / 2;
                using var font = new Font(fontName, midSize, FontStyle.Regular, GraphicsUnit.Pixel);

                var textSize = graphics.MeasureString(text, font);
                if (textSize.Width <= rectangle.Width && textSize.Height <= rectangle.Height)
                {
                    result = midSize;
                    minSize = midSize + 1;
                }
                else
                {
                    maxSize = midSize - 1;
                }
            }

            return result;
        }
    }
}