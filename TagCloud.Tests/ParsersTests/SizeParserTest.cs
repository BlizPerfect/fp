using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    internal class SizeParserTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public void ParseImageSize_ThrowsException_WithInvalidInput(string value)
        {
            var expected = Result.Fail<Size>($"Некорректная строка {value}");
            var actual = SizeParser.ParseImageSize(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("100:")]
        [TestCase(":100")]
        [TestCase("100:100:100")]
        [TestCase("abc")]
        public void ParseImageSize_ThrowsException_WithIncorrectFormat(string value)
        {
            var expected = Result.Fail<Size>($"Некорректный формат размера изображения: \"{value}\", используйте формат \"Ширина:Высота\", например 5000:5000");
            var actual = SizeParser.ParseImageSize(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc:100")]
        [TestCase("100:abc")]
        [TestCase("abc:abc")]
        public void ParseImageSize_ThrowsException_WithIncorrectInput(string value)
        {
            var expected = Result.Fail<Size>($"Передано не число: abc");
            var actual = SizeParser.ParseImageSize(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0:100")]
        [TestCase("100:0")]
        [TestCase("0:0")]
        public void ParseImageSize_ThrowsException_WithInputLessThanZero(string value)
        {
            var expected = Result.Fail<Size>($"Переданное числовое значение должно быть больше 0: 0");
            var actual = SizeParser.ParseImageSize(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public void ParseSizeDimension_ThrowsException_WithInvalidInputAsString(string value)
        {
            var expected = Result.Fail<int>($"Некорректная строка {value}");
            var actual = SizeParser.ParseSizeDimension(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("abc")]
        public void ParseSizeDimension_ThrowsException_WithIncorrectInputAsString(string value)
        {
            var expected = Result.Fail<int>($"Передано не число: {value}");
            var actual = SizeParser.ParseSizeDimension(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void ParseSizeDimension_ThrowsException_WithInputLessThanZeroAsString(string value)
        {
            var expected = Result.Fail<int>($"Переданное числовое значение должно быть больше 0: {value}");
            var actual = SizeParser.ParseSizeDimension(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("0")]
        [TestCase("-1")]
        public void ParseSizeDimension_ThrowsException_WithInputLessThanZeroAsInt(int value)
        {
            var expected = Result.Fail<int>($"Переданное числовое значение должно быть больше 0: {value}");
            var actual = SizeParser.ParseSizeDimension(value);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
