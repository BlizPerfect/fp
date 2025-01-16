using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    internal class WordsToIncludeFileNameTests() : BaseOptionTest("WordsToIncludeFileName")
    {
        [TestCase("NonExistingFile.txt")]
        public void Program_WorksCorrectly_WithNonExistingFilename(string wordsToIncludeFileName)
        {
            options.WordsToIncludeFileName = wordsToIncludeFileName;
            var expected = Result.Fail<None>($"Файл \"{wordsToIncludeFileName}\" не существует");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithFileWithMoreThanOneWordInLine(
            string wordsToIncludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToIncludeFileName);
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

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithEmptyFile(string wordsToIncludeFileName)
        {
            var path = Path.Combine(directoryPath, wordsToIncludeFileName);
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
