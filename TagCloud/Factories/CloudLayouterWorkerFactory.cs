using FileSenderRailway;
using TagCloud.CloudLayouterWorkers;
using TagCloud.Normalizers;
using TagCloud.WordCounters;
using TagCloud.WordFilters;
using TagCloud.WordReaders;

namespace TagCloud.Factories
{
    internal class CloudLayouterWorkerFactory(
        IWordReader wordReader,
        IWordCounter wordCounter,
        INormalizer normalizer,
        IWordFilterFactory wordFilterFactory) : ICloudLayouterWorkerFactory
    {
        public Result<ICloudLayouterWorker> Create(
            string dataFileName,
            string? wordsToIncludeFileName,
            string? wordsToExcludeFileName,
            int maxRectangleWidth,
            int maxRectangleHeight,
            bool isSorted)
            => ReadWords(dataFileName)
                .Then(initialWords => CreateFilter(
                    wordsToIncludeFileName,
                    wordsToExcludeFileName,
                    wordReader)
                    .Then(filter => AddWords(filter, initialWords)))
                .Then(_ => normalizer.Normalize(wordCounter.Values))
                .Then(normalizer
                    => Result.Ok<ICloudLayouterWorker>(
                        new NormalizedFrequencyBasedCloudLayouterWorker(
                            maxRectangleWidth,
                            maxRectangleHeight,
                            normalizer,
                            isSorted)))
            .OnFail(error => Result.Fail<ICloudLayouterWorker>(error));

        private Result<IEnumerable<string>> ReadWords(string dataFileName)
            => wordReader.ReadByLines(dataFileName);

        private Result<IWordFilter> CreateFilter(
            string? wordsToIncludeFileName,
            string? wordsToExcludeFileName,
            IWordReader wordReader)
            => wordFilterFactory.Create(wordsToIncludeFileName, wordsToExcludeFileName, wordReader);

        private Result<None> AddWords(IWordFilter wordFilter, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var wordInLowerCase = word.ToLower();
                if (!wordFilter.IsCorrectWord(wordInLowerCase))
                {
                    continue;
                }

                wordCounter.AddWord(wordInLowerCase);
            }
            return Result.Ok();
        }
    }
}