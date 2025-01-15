using FileSenderRailway;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloud.ImageSavers
{
    // Реализован пункт на перспективу:
    // Формат результата.
    // Поддерживать разные форматы изображений.
    internal class ImageSaver : IImageSaver
    {
        private readonly Dictionary<string, ImageFormat> supportedFormats =
            new Dictionary<string, ImageFormat>()
        {
            {"png" , ImageFormat.Png },
            {"jpg" , ImageFormat.Jpeg },
            {"jpeg" , ImageFormat.Jpeg },
            {"bmp" , ImageFormat.Bmp },
            {"gif" , ImageFormat.Gif },
            {"tiff" , ImageFormat.Tiff }
        };

        public Result<None> SaveFile(Bitmap image, string fileName, string format = "png")
            => ValidateInput(image, fileName, format)
                .Then(_ => SaveImage(image, fileName, format))
                .OnFail(error => Result.Fail<None>(error));

        private Result<None> ValidateInput(Bitmap image, string fileName, string format)
        {
            if (image is null)
            {
                return Result.Fail<None>("Передаваемое изображение не должно быть null");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.Fail<None>("Некорректное имя файла для создания");
            }

            if (string.IsNullOrWhiteSpace(format) || !IsSupportedFormat(format))
            {
                return Result.Fail<None>($"Формат \"{format}\" не поддерживается");
            }

            return Result.Ok();
        }

        private Result<None> SaveImage(Bitmap image, string fileName, string format)
        {
            var imageFormat = supportedFormats[format];
            image.Save($"{fileName}.{format}", imageFormat);
            return Result.Ok();
        }

        private bool IsSupportedFormat(string format)
            => supportedFormats.ContainsKey(format);
    }
}