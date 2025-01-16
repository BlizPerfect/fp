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
        public void BoolParser_ThrowsException_WithInvalidInput(string sorted)
        {
            var expected = Result.Fail<bool>($"Неизвестный параметр сортировки \"{sorted}\"");
            var actual = BoolParser.ParseIsSorted(sorted);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("True")]
        [TestCase("False")]
        public void BoolParser_WorksCorrectly(string sorted)
        {
            var expected = Convert.ToBoolean(sorted);
            var actual = BoolParser.ParseIsSorted(sorted).GetValueOrThrow();
            actual.Should().Be(expected);
        }
    }
}
