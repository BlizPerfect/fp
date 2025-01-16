using Autofac;
using FileSenderRailway;
using FluentAssertions;
using TagCloud.Tests.OptionsTests;
using TagCloud.Tests.Utilities;
namespace TagCloud.Tests
{
    [TestFixture]
    internal class MainTest() : BaseOptionTest("MainTest")
    {
        [Test]
        public void Program_ExecutesSuccessfully_WithValidArguments()
        {
            var expected = Result.Ok();

            var wordsToIncludePath = Path.Combine(directoryPath, "ToInclude.txt");
            FileUtilities.CreateDataFile(
                directoryPath,
                wordsToIncludePath,
                new string[]
                {
                    "snow",
                    "white"
                });

            var wordsToExcludePath = Path.Combine(directoryPath, "ToExclude.txt");
            FileUtilities.CreateDataFile(
                directoryPath,
                wordsToExcludePath,
                new string[]
                {
                    "the"
                });

            var options = new CommandLineOptions
            {
                BackgroundColor = "Black",
                TextColor = "Yellow",
                Font = "Calibri",
                IsSorted = Boolean.FalseString,
                ImageSize = "1000:1000",
                MaxRectangleHeight = "100",
                MaxRectangleWidth = "200",
                ImageFileName = imageFile,
                DataFileName = dataFile,
                ResultFormat = "bmp",
                WordsToIncludeFileName = wordsToIncludePath,
                WordsToExcludeFileName = wordsToExcludePath,
            };

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
            File.Exists($"{imageFile}.{options.ResultFormat}").Should().BeTrue();
        }
    }
}
