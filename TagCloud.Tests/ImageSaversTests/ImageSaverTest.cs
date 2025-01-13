using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.ImageSavers;
using TagCloud.WordReaders;

namespace TagCloud.Tests.ImageSaversTests
{
    [TestFixture]
    internal class ImageSaverTest
    {
        private string directoryPath = "TempFilesForImageSaverTests";
        private ImageSaver imageSaver;

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(directoryPath);
        }

        [SetUp]
        public void SetUp()
        {
            imageSaver = new ImageSaver();
        }

        [TestCase("Test")]
        public void SaveFile_ArgumentNullException_WithNullBitmap(string filename)
        {
            var path = Path.Combine(directoryPath, filename);
            var expected = Result.Fail<None>("Передаваемое изображение не должно быть null");
            var actual = imageSaver.SaveFile(null!, path);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SaveFile_ThrowsArgumentException_WithInvalidFilename(string? filename)
        {
            var dummyImage = new Bitmap(1, 1);
            var expected = Result.Fail<None>("Некорректное имя файла для создания");
            var actual = imageSaver.SaveFile(dummyImage, filename!);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Test", "png", ExpectedResult = true)]
        [TestCase("Test", "bmp", ExpectedResult = true)]
        public bool SaveFile_SavesFile(string filename, string format)
        {
            var dummyImage = new Bitmap(1, 1);
            var path = Path.Combine(directoryPath, filename);

            File.Delete($"{path}.{format}");
            imageSaver.SaveFile(dummyImage, path, format);
            return File.Exists($"{path}.{format}");
        }


        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}
