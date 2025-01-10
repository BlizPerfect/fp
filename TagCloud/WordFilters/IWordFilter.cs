namespace TagCloud.WordFilters
{
    // Интерфейс фильтрации "скучных" слов
    internal interface IWordFilter
    {
        public bool IsCorrectWord(string word);
    }
}
