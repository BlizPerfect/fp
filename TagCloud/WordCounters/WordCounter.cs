namespace TagCloud.WordCounters
{
    internal class WordCounter : IWordCounter
    {
        private readonly Dictionary<string, uint> counts = new Dictionary<string, uint>();
        public Dictionary<string, uint> Values => counts;

        public void AddWord(string word)
        {
            counts.TryGetValue(word, out uint value);
            counts[word] = value + 1;
        }
    }
}
