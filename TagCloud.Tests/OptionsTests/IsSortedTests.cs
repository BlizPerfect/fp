using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    internal class IsSortedTests() : BaseOptionTest("IsSorted")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithInvalidIsSorted(string sorted)
        {
            options.IsSorted = sorted;
            var expected = Result.Fail<None>($"Неизвестный параметр сортировки \"{sorted}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
