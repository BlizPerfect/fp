using FileSenderRailway;

namespace TagCloud.WordReaders
{
    internal class WordReader : IWordReader
    {
        public IEnumerable<Result<string>> ReadByLines(string path)
        {
            if (!File.Exists(path))
            {
                yield return Result.Fail<string>($"Файл {path} не существует");
                yield break;
            }

            foreach (var line in File.ReadAllLines(path))
            {
                if (line.Contains(' '))
                {
                    yield return Result.Fail<string>($"Файл {path} содержит строку с двумя и более словами");
                    yield break;
                }
                yield return line.AsResult();
            }
        }
    }
}
