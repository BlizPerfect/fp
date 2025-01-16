using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    internal class DataFileNameTest() : BaseOptionTest("DataFileName")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("NonExistingFile.txt")]
        public void Program_WorksCorrectly_WithNonExistingFilename(string dataFileName)
        {
            options.DataFileName = dataFileName;
            var expected = Result.Fail<None>($"Файл \"{dataFileName}\" не существует");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithFileWithMoreThanOneWordInLine(string dataFileName)
        {
            var path = Path.Combine(directoryPath, dataFileName);
            var invalidContent = new string[]
            {
                "one",
                "two",
                "three three three",
                "four"
            };
            FileUtilities.CreateDataFile(directoryPath, path, invalidContent);

            options.DataFileName = path;
            var expected = Result.Fail<None>(
                $"Файл \"{path}\" содержит строку с двумя и более словами");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("InvalidFile_MoreThanOneWordInLine.txt")]
        public void Program_WorksCorrectly_WithEmptyFile(string dataFileName)
        {
            var path = Path.Combine(directoryPath, dataFileName);
            FileUtilities.CreateDataFile(directoryPath, path, Array.Empty<string>());

            options.DataFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" пустой");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("FileDoc.doc")]
        [TestCase("FileImg.png")]
        public void Program_WorksCorrectly_WithNonTxtFile(string dataFileName)
        {
            var path = Path.Combine(directoryPath, dataFileName);
            FileUtilities.CreateDataFile(directoryPath, path, Array.Empty<string>());

            options.DataFileName = path;
            var expected = Result.Fail<None>($"Файл \"{path}\" должен иметь расширение \"txt\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
