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
        [TestCase("abc")]
        public void FontParser_ThrowsException_WithInvalidFont(string font)
        {
            var expected = Result.Fail<FontFamily>($"Неизвестный шрифт \"{font}\"");
            var actual = FontParser.ParseFont(font);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Arial")]
        [TestCase("Times New Roman")]
        [TestCase("Georgia")]
        [TestCase("Verdana")]
        public void FontParser_WorksCorrectly(string font)
        {
            var expected = new FontFamily(font);
            var actual = FontParser.ParseFont(font).GetValueOrThrow();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
