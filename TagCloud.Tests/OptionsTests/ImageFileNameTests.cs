using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class ImageFileNameTests() : BaseOptionTest("ImageFileName")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public void Program_WorksCorrectly_WithInvalidImageFileName(string imageFileName)
        {
            options.ImageFileName = imageFileName;
            var expected = Result.Fail<None>(
                $"Некорректное имя файла для создания \"{imageFileName}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
