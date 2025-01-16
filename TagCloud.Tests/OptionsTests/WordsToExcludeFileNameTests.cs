using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    internal class WordsToExcludeFileNameTests() : BaseOptionTest("WordsToExcludeFileName")
    {
        [TestCase("NonExistingFile.txt")]
        public void Program_WorksCorrectly_WithNonExistingFilename(string wordsToExcludeFileName)
        {
            options.WordsToExcludeFileName = wordsToExcludeFileName;
            var expected = Result.Fail<None>($"Файл \"{wordsToExcludeFileName}\" не существует");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithFileWithMoreThanOneWordInLine(
            string wordsToExcludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToExcludeFileName);
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

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithEmptyFile(string wordsToExcludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToExcludeFileName);
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
