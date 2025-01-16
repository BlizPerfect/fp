using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class BackgroundColorTests() : BaseOptionTest("BackgroundColor")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithInvalidBackgroundColor(string backgroundColor)
        {
            options.BackgroundColor = backgroundColor;
            var expected = Result.Fail<None>($"Неизвестный цвет \"{backgroundColor}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
