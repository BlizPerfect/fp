using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class WordsToExcludeFileNameTests() : BaseOptionTest("WordsToExcludeFileName")
    {
        [Test]
        public void Program_WorksCorrectly_WithNonExistingFilename()
        {
            var wordsToExcludeFileName = "NonExistingFile.txt";
            options.WordsToExcludeFileName = wordsToExcludeFileName;
            var expected = Result.Fail<None>($"Файл \"{wordsToExcludeFileName}\" не существует");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
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

            options.WordsToExcludeFileName = path;
            var expected = Result.Fail<None>(
                $"Файл \"{path}\" содержит строку с двумя и более словами");

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

            options.WordsToExcludeFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" пустой");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("FileDoc.doc")]
        [TestCase("FileImg.png")]
        public void Program_WorksCorrectly_WithNonTxtFile(string wordsToExcludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToExcludeFileName);
            FileUtilities.CreateDataFile(directoryPath, path, Array.Empty<string>());

            options.WordsToExcludeFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" должен иметь расширение \"txt\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
