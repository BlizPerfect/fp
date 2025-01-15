using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    internal class ColorParserTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public void ColorParser_ThrowsException_WithInvalidInput(string value)
        {
            var expected = Result.Fail<Color>($"Некорректная строка {value}");
            var actual = ColorParser.ParseColor(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc")]
        public void ColorParser_ThrowsException_WithUnknownColor(string value)
        {
            var expected = Result.Fail<Color>($"Неизвестный цвет {value}");
            var actual = ColorParser.ParseColor(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("red")]
        [TestCase("blue")]
        [TestCase("white")]
        [TestCase("black")]
        [TestCase("green")]
        [TestCase("yellow")]
        public void ColorParser_WorksCorrectly(string value)
        {
            var expected = Color.FromName(value);
            var actual = ColorParser.ParseColor(value).GetValueOrThrow();
            actual.Should().Be(expected);
        }
    }
}
