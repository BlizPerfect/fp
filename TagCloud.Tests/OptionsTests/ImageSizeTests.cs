using Autofac;
using FileSenderRailway;
using FluentAssertions;

namespace TagCloud.Tests.OptionsTests
{
    [TestFixture]
    internal class ImageSizeTests() : BaseOptionTest("ImageSize")
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("100:")]
        [TestCase(":100")]
        [TestCase("100:100:100")]
        [TestCase("abc")]
        public void Program_WorksCorrectly_WithIncorrectFormat(string size)
        {
            options.ImageSize = size;
            var expected = Result.Fail<None>(
                $"Некорректный формат размера изображения \"{size}\", используйте формат \"Ширина:Высота\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc:100")]
        [TestCase("100:abc")]
        [TestCase("abc:abc")]
        public void Program_WorksCorrectly_WithIncorrectInput(string size)
        {
            options.ImageSize = size;
            var expected = Result.Fail<None>($"Передано не число \"abc\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0:100")]
        [TestCase("-1:100")]
        [TestCase("100:0")]
        [TestCase("100:-1")]
        [TestCase("0:0")]
        [TestCase("-1:-1")]
        public void Program_WorksCorrectly_WithInputLessThanZero(string size)
        {
            options.ImageSize = size;
            var wrongValue = size.Contains("-1") ? "-1" : "0";
            var expected = Result.Fail<None>($"Переданное числовое значение должно быть больше 0: \"{wrongValue}\"");

            var container = DIContainer.ConfigureContainer(options);
            using var scope = container.BeginLifetimeScope();
            var executor = scope.Resolve<ProgramExecutor>();
            var actual = executor.Execute();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
