using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class ResultFormatTests() : BaseOptionTest("ResultFormat")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithInvalidResultFormat(string resultFormat)
        {
            options.ResultFormat = resultFormat;
            var expected = Result.Fail<None>($"Формат \"{resultFormat}\" не поддерживается");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
