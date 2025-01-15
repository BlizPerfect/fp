using FileSenderRailway;
using FluentAssertions;
using TagCloud.Parsers;

namespace TagCloud.Tests.ParsersTests
{
    [TestFixture]
    internal class BoolParserTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        [TestCase("abc")]
        public void BoolParser_ThrowsException_WithInvalidInput(string value)
        {
            var expected = Result.Fail<bool>($"Неизвестный параметр сортировки {value}");
            var actual = BoolParser.ParseIsSorted(value);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("True")]
        [TestCase("False")]
        public void BoolParser_WorksCorrectly(string value)
        {
            var expected = Convert.ToBoolean(value);
            var actual = BoolParser.ParseIsSorted(value).GetValueOrThrow();
            actual.Should().Be(expected);
        }
    }
}
