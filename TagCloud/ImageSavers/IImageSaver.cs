using FileSenderRailway;
using System.Drawing;

namespace TagCloud.ImageSavers
{
    // Интерфейс сохранения изображения в файл
    internal interface IImageSaver
    {
        public Result<None> SaveFile(Bitmap image, string fileName, string format = "png");
    }
}
