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
        [TestCase("abc")]
        public void ColorParser_ThrowsException_WithInvalidColor(string color)
        {
            var expected = Result.Fail<Color>($"Неизвестный цвет \"{color}\"");
            var actual = ColorParser.ParseColor(color);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("red")]
        [TestCase("blue")]
        [TestCase("white")]
        [TestCase("black")]
        [TestCase("green")]
        [TestCase("yellow")]
        public void ColorParser_WorksCorrectly(string color)
        {
            var expected = Color.FromName(color);
            var actual = ColorParser.ParseColor(color).GetValueOrThrow();
            actual.Should().Be(expected);
        }
    }
}
