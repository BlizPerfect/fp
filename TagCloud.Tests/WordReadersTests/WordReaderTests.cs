using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.OptionsTests;
using TagCloud.Tests.Utilities;
using TagCloud.WordReaders;

namespace TagCloud.Tests.WordReadersTests
{
    [TestFixture]
    internal class WordReaderTests
    {
        private readonly string directoryPath = "TempFilesForWordReaderTests";

        private readonly string fileWithCorrectValuesPath = "CorrectFile.txt";

        private readonly string fileWithMoreThanOneWordInLinePath
            = "InvalidFile_MoreThanOneWordInLine.txt";
        private readonly string[] moreThanOneWordInLineValues = new string[]
        {
            "One",
            "Two",
            "Three Three",
            "Four"
        };

        private readonly string fileEmptyPath = "InvalidFile_Empty.txt";

        [OneTimeSetUp]
        public void Init()
        {
            FileUtilities
                .CreateDataFile(
                    directoryPath,
                    Path.Combine(directoryPath, fileWithCorrectValuesPath),
                    ValidValues.ValidDataFileContent);

            FileUtilities
                .CreateDataFile(
                    directoryPath,
                    Path.Combine(directoryPath, fileWithMoreThanOneWordInLinePath),
                    moreThanOneWordInLineValues);

            FileUtilities
                .CreateDataFile(
                    directoryPath,
                    Path.Combine(directoryPath, fileEmptyPath),
                    Array.Empty<string>());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("NonExistingFile.txt")]
        public void WordReader_ThrowsFileNotFoundException_WithInvalidFilename(string filename)
        {
            var wordReader = new WordReader();
            var path = Path.Combine(directoryPath, filename);
            var expected = Result.Fail<IEnumerable<string>>($"Файл \"{path}\" не существует");
            var actual = wordReader.ReadByLines(path);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WordReader_ThrowsException_WithTwoWordsInOneLine()
        {
            var wordReader = new WordReader();
            var path = Path.Combine(directoryPath, fileWithMoreThanOneWordInLinePath);
            var expected = Result.Fail<IEnumerable<string>>($"Файл \"{path}\" содержит строку с двумя и более словами");
            var actual = wordReader.ReadByLines(path);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WordReader_ThrowsException_WithEmpty()
        {
            var wordReader = new WordReader();
            var path = Path.Combine(directoryPath, fileEmptyPath);
            var expected = Result.Fail<IEnumerable<string>>($"Файл \"{path}\" пустой");
            var actual = wordReader.ReadByLines(path);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("FileDoc.doc")]
        [TestCase("FileImg.png")]
        public void WordReader_ThrowsException_WithNonTxt(string filename)
        {
            var wordReader = new WordReader();
            FileUtilities
                .CreateDataFile(
                    directoryPath,
                    Path.Combine(directoryPath, filename),
                    Array.Empty<string>());

            var path = Path.Combine(directoryPath, filename);
            var expected = Result.Fail<IEnumerable<string>>(
                $"Файл \"{path}\" должен иметь расширение \"txt\"");
            var actual = wordReader.ReadByLines(path);
            actual.Should().BeEquivalentTo(expected);
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
