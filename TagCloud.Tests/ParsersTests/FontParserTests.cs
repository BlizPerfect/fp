using FileSenderRailway;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
    internal class FontParserTests
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
