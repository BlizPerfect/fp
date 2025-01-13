using FileSenderRailway;

namespace TagCloud.WordReaders
{
    // Интерфейс для построчного чтения содержимого файла
    internal interface IWordReader
    {
        public IEnumerable<Result<string>> ReadByLines(string path);
    }
}
