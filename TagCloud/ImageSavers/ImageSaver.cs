using FileSenderRailway;
using System.Drawing;
using System.IO;

namespace TagCloud.ImageSavers
{
    // Реализован пункт на перспективу:
    // Формат результата.
    // Поддерживать разные форматы изображений.
    internal class ImageSaver : IImageSaver
    {
        public Result<None> SaveFile(Bitmap image, string fileName, string format = "png")
        {
            if (image is null)
            {
                return Result.Fail<None>("Передаваемое изображение не должно быть null");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.Fail<None>("Некорректное имя файла для создания");
            }

            image.Save($"{fileName}.{format}");
            return Result.Ok();
        }
    }
}
