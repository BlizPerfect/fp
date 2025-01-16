using FileSenderRailway;

namespace TagCloud.WordReaders
{
    internal class WordReader : IWordReader
    {
        public Result<IEnumerable<string>> ReadByLines(string path)
        {
            if (!File.Exists(path))
            {
                return Result.Fail<IEnumerable<string>>($"Файл \"{path}\" не существует");
            }

            if (path.Split('.')[^1] != "txt")
            {
                return Result.Fail<IEnumerable<string>>($"Файл \"{path}\" должен иметь расширение \"txt\"");
            }

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                return Result.Fail<IEnumerable<string>>($"Файл \"{path}\" пустой");
            }
            if (lines.Any(line => line.Contains(' ')))
            {
                return Result.Fail<IEnumerable<string>>($"Файл \"{path}\" содержит строку с двумя и более словами");
            }

            return lines.AsEnumerable().AsResult();
        }
    }
}