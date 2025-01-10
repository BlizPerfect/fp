namespace TagCloud.WordReaders
{
    // Интерфейс для построчного чтения содержимого файла
    internal interface IWordReader
    {
        public IEnumerable<string> ReadByLines(string path);
    }
}
