using FileSenderRailway;

namespace TagCloud.WordReaders
{
    // Интерфейс для построчного чтения содержимого файла
    internal interface IWordReader
    {
        public Result<IEnumerable<string>> ReadByLines(string path);
    }
}
