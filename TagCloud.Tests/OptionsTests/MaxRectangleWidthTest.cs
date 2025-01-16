using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    internal class MaxRectangleWidthTest() : BaseOptionTest("MaxRectangleWidth")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithNotANumber(string maxRectangleWidth)
        {
            options.MaxRectangleWidth = maxRectangleWidth;
            var expected = Result.Fail<None>($"Передано не число \"{maxRectangleWidth}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void Program_WorksCorrectly_WithNumberZeroOrLess(string maxRectangleWidth)
        {
            options.MaxRectangleWidth = maxRectangleWidth;
            var expected = Result.Fail<None>(
                $"Переданное числовое значение должно быть больше 0: \"{maxRectangleWidth}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
