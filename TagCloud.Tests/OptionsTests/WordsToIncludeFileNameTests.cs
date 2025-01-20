using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class WordsToIncludeFileNameTests() : BaseOptionTest("WordsToIncludeFileName")
    {
        [Test]
        public void Program_WorksCorrectly_WithNonExistingFilename()
        {
            var wordsToIncludeFileName = "NonExistingFile.txt";
            options.WordsToIncludeFileName = wordsToIncludeFileName;
            var expected = Result.Fail<None>($"Файл \"{wordsToIncludeFileName}\" не существует");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        public void Program_WorksCorrectly_WithFileWithMoreThanOneWordInLine()
        {
            var path = Path.Combine(directoryPath, "InvalidFile_MoreThanOneWordInLine.txt");
            var invalidContent = new string[]
            {
                "one",
                "two",
                "three three three",
                "four"
            };
            FileUtilities.CreateDataFile(directoryPath, path, invalidContent);

            options.WordsToIncludeFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" содержит строку с двумя и более словами");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Program_WorksCorrectly_WithEmptyFile()
        {
            var path = Path.Combine(directoryPath, "InvalidFile_MoreThanOneWordInLine.txt");
            FileUtilities.CreateDataFile(directoryPath, path, Array.Empty<string>());

            options.WordsToIncludeFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" пустой");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("FileDoc.doc")]
        [TestCase("FileImg.png")]
        public void Program_WorksCorrectly_WithNonTxtFile(string wordsToIncludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToIncludeFileName);
            FileUtilities.CreateDataFile(directoryPath, path, Array.Empty<string>());

            options.WordsToIncludeFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" должен иметь расширение \"txt\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
