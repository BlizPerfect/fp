using FileSenderRailway;
using TagCloud.WordFilters;
using TagCloud.WordReaders;

namespace TagCloud.Factories
{
    internal interface IWordFilterFactory
    {
        public Result<IWordFilter> Create(
            string? wordsToIncludeFileName,
            string? wordsToExcludeFileName,
            IWordReader wordReader);
    }
}
