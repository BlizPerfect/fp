using FileSenderRailway;
using FluentAssertions;
using TagCloud.WordReaders;

namespace TagCloud.Tests.WordReadersTests
{
    [TestFixture]
    internal class WordReaderTest
    {
        private readonly string directoryPath = "TempFilesForWordReaderTests";

        private readonly string fileWithCorrectValuesPath = "correctFile.txt";
        private readonly string[] correctValues = new string[]
        {
            "One",
            "One",
            "Two",
            "Three",
            "Four",
            "Four",
            "Four",
            "Four"
        };

        private readonly string fileWithIncorrectValuesPath = "incorrectFile.txt";
        private readonly string[] incorrectValues = new string[]
        {
            "One",
            "Two",
            "Three Three",
            "Four"
        };

        private WordReader wordReader;

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(directoryPath);
            File.WriteAllLines(
                Path.Combine(
                    directoryPath,
                    fileWithCorrectValuesPath),
                    correctValues);
            File.WriteAllLines
                (Path.Combine(
                    directoryPath,
                    fileWithIncorrectValuesPath),
                    incorrectValues);
        }

        [SetUp]
        public void SetUp()
        {
            wordReader = new WordReader();
        }

        [TestCase(" ")]
        [TestCase("ThisFileDoesNotExist.txt")]
        public void WordReader_ThrowsFileNotFoundException_WithInvalidFilename(string filename)
        {
            var path = Path.Combine(directoryPath, filename);
            var expected = Result.Fail<string>($"Файл {path} не существует");
            var actual = wordReader.ReadByLines(path).ToArray();
            actual.Should().Contain(expected).And.HaveCount(1);
        }

        [Test]
        public void WordReader_ThrowsException_WithTwoWordsInOneLine()
        {
            var path = Path.Combine(directoryPath, fileWithIncorrectValuesPath);
            var expected = Result.Fail<string>($"Файл {path} содержит строку с двумя и более словами");
            var actual = wordReader.ReadByLines(path).ToArray();
            actual.Should().Contain(expected);
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
