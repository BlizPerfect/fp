using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    internal class MaxRectangleHeightTest() : BaseOptionTest("MaxRectangleHeight")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithNotANumber(string maxRectangleHeight)
        {
            options.MaxRectangleWidth = maxRectangleHeight;
            var expected = Result.Fail<None>($"Передано не число \"{maxRectangleHeight}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void Program_WorksCorrectly_WithNumberZeroOrLess(string maxRectangleHeight)
        {
            options.MaxRectangleWidth = maxRectangleHeight;
            var expected = Result.Fail<None>(
                $"Переданное числовое значение должно быть больше 0: \"{maxRectangleHeight}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
