using FluentAssertions;
using TagCloud.WordFilters;

namespace TagCloud.Tests.WordFiltersTests
{
    [TestFixture]
    internal class WordFilterChangeBannedWordsTest
    {
        [Test]
        public void Clear_ShouldClearBannedWordList()
        {
            var wordFilter = new WordFilter();
            wordFilter.Clear();
            wordFilter.BannedWords.Should().BeEmpty();
        }

        [Test]
        public void Add_ShouldAddWord_ToBannedWords()
        {
            var wordFilter = new WordFilter();
            var wordToAdd = "WordToAdd";
            wordFilter.Clear();
            wordFilter.Add(wordToAdd);
            wordFilter.BannedWords.Should().Contain(wordToAdd).And.HaveCount(1);
        }

        [Test]
        public void Remove_ShouldRemoveWord_InBannedWords()
        {
            var wordFilter = new WordFilter();
            var wordToRemove = "WordToRemove";
            wordFilter.Clear();
            wordFilter.Add(wordToRemove);
            wordFilter.Remove(wordToRemove);
            wordFilter.BannedWords.Should().NotContain(wordToRemove);
        }
    }
}
