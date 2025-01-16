using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class TextColorTests() : BaseOptionTest("TextColor")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithInvalidTextColor(string textColor)
        {
            options.TextColor = textColor;
            var expected = Result.Fail<None>($"Неизвестный цвет \"{textColor}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);

            var result = executor.Execute();
        }
    }
}
