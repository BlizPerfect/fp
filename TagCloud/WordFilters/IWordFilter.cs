namespace TagCloud.WordFilters
{
    // Интерфейс фильтрации "скучных" слов
    internal interface IWordFilter
    {
        public bool Add(string word);
        public bool Remove(string word);
        public bool IsCorrectWord(string word);
    }
}
