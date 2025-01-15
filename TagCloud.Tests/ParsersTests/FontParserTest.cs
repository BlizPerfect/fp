using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    internal class FontParserTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public void FontParser_ThrowsException_WithInvalidInput(string value)
        {
            var expected = Result.Fail<FontFamily>($"Некорректная строка {value}");
            var actual = FontParser.ParseFont(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc")]
        public void FontParser_ThrowsException_WithUnknownColor(string value)
        {
            var expected = Result.Fail<FontFamily>($"Неизвестный шрифт {value}");
            var actual = FontParser.ParseFont(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Arial")]
        [TestCase("Times New Roman")]
        [TestCase("Georgia")]
        [TestCase("Verdana")]
        public void FontParser_WorksCorrectly(string value)
        {
            var expected = new FontFamily(value);
            var actual = FontParser.ParseFont(value).GetValueOrThrow();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
