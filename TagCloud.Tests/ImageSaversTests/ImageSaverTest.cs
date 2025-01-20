using FileSenderRailway;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.ImageSavers;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.ImageSaversTests
{
    [TestFixture]
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
    internal class ImageSaverTest
    {
        private readonly string directoryPath = "TempFilesForImageSaverTests";
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
        public void SaveFile_ThrowsException_WithNullBitmap(string filename)
        {
            var path = Path.Combine(directoryPath, filename);
            var expected = Result.Fail<None>("Передаваемое изображение не должно быть null");
            var actual = imageSaver.SaveFile(null!, path);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SaveFile_ThrowsException_WithInvalidFilename(string? filename)
        {
            var dummyImage = new Bitmap(1, 1);
            var expected = Result.Fail<None>($"Некорректное имя файла для создания \"{filename}\"");
            var actual = imageSaver.SaveFile(dummyImage, filename!);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("abc")]
        public void SaveFile_ThrowsException_WithInvalidFormat(string? format)
        {
            var dummyImage = new Bitmap(1, 1);
            var filename = "Test";
            var expected = Result.Fail<None>($"Формат \"{format}\" не поддерживается");
            var actual = imageSaver.SaveFile(dummyImage, filename, format!);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Test", "png", ExpectedResult = true)]
        [TestCase("Test", "jpg", ExpectedResult = true)]
        [TestCase("Test", "jpeg", ExpectedResult = true)]
        [TestCase("Test", "bmp", ExpectedResult = true)]
        [TestCase("Test", "gif", ExpectedResult = true)]
        [TestCase("Test", "tiff", ExpectedResult = true)]
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
            FileUtilities.DeleteDirectory(directoryPath);
        }
    }
}
