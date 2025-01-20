using FluentAssertions;
using TagCloud.Tests.OptionsTests;
using TagCloud.WordCounters;

namespace TagCloud.Tests.WordCountersTests
{
    [TestFixture]
    internal class WordCounterTest
    {
        [Test]
        public void WordCounter_CountsCorrect()
        {
            var wordCounter = new WordCounter();
            var expected = new Dictionary<string, uint>();
            foreach (var word in ValidValues.ValidDataFileContent)
            {
                expected.TryGetValue(word, out var count);
                expected[word] = count + 1;
            }

            foreach (var value in ValidValues.ValidDataFileContent)
            {
                wordCounter.AddWord(value);
            }
            wordCounter.Values.Should().BeEquivalentTo(expected);
        }
    }
}
