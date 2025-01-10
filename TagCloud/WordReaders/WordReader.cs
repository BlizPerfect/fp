namespace TagCloud.WordReaders
{
    internal class WordReader : IWordReader
    {
        public IEnumerable<string> ReadByLines(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Файл {path} не существует");
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (line.Contains(' '))
                {
                    throw new Exception($"Файл {path} содержит строку с двумя и более словами");
                }
                yield return line;
            }
        }
    }
}
