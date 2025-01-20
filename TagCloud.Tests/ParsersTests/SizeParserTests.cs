using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    internal class SizeParserTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("100:")]
        [TestCase(":100")]
        [TestCase("100:100:100")]
        [TestCase("abc")]
        public void ParseImageSize_ThrowsException_WithIncorrectFormat(string size)
        {
            var expected = Result.Fail<Size>(
                $"Некорректный формат размера изображения \"{size}\", используйте формат \"Ширина:Высота\"");
            var actual = SizeParser.ParseImageSize(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc:100")]
        [TestCase("100:abc")]
        [TestCase("abc:abc")]
        public void ParseImageSize_ThrowsException_WithIncorrectInput(string size)
        {
            var expected = Result.Fail<Size>($"Передано не число \"abc\"");
            var actual = SizeParser.ParseImageSize(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0:100")]
        [TestCase("-1:100")]
        [TestCase("100:0")]
        [TestCase("100:-1")]
        [TestCase("0:0")]
        [TestCase("-1:-1")]
        public void ParseImageSize_ThrowsException_WithInputLessThanZero(string size)
        {
            var wrongValue = size.Contains("-1") ? "-1" : "0";
            var expected = Result.Fail<Size>($"Переданное числовое значение должно быть больше 0: \"{wrongValue}\"");
            var actual = SizeParser.ParseImageSize(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void ParseSizeDimension_ThrowsException_WithIncorrectInputAsString(string size)
        {
            var expected = Result.Fail<int>($"Передано не число \"{size}\"");
            var actual = SizeParser.ParseSizeDimension(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void ParseSizeDimension_ThrowsException_WithInputLessThanZeroAsString(string size)
        {
            var expected = Result.Fail<int>($"Переданное числовое значение должно быть больше 0: \"{size}\"");
            var actual = SizeParser.ParseSizeDimension(size);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void ParseSizeDimension_ThrowsException_WithInputLessThanZeroAsInt(int size)
        {
            var expected = Result.Fail<int>($"Переданное числовое значение должно быть больше 0: \"{size}\"");
            var actual = SizeParser.ParseSizeDimension(size);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
