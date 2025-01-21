using FileSenderRailway;
using TagCloud.CloudLayouterWorkers;
using TagCloud.WordFilters;
using TagCloud.WordReaders;

namespace TagCloud.Factories
{
    internal class WordFilterFactory : IWordFilterFactory
    {
        public Result<IWordFilter> Create(
            string? wordsToIncludeFileName,
            string? wordsToExcludeFileName,
            IWordReader wordReader)
            => Result.Ok<IWordFilter>(new WordFilter())
                .Then(wordFilter => AddWords(wordReader, wordsToIncludeFileName!, wordFilter))
                .Then(wordFilter => RemoveWords(wordReader, wordsToExcludeFileName!, wordFilter))
                .OnFail(error => Result.Fail<ICloudLayouterWorker>(error));

        private static bool IsFileNameSet(string? fileName)
            => fileName is not null;

        private static Result<IWordFilter> AddWords(
            IWordReader wordReader,
            string wordsToIncludeFileName,
            IWordFilter wordFilter)
        {
            if (IsFileNameSet(wordsToIncludeFileName))
            {
                var wordsResult = wordReader.ReadByLines(wordsToIncludeFileName);
                if (!wordsResult.IsSuccess)
                {
                    return Result.Fail<IWordFilter>(wordsResult.Error);
                }

                foreach (var word in wordsResult.GetValueOrThrow())
                {
                    wordFilter.Add(word.ToLower());
                }
            }

            return Result.Ok(wordFilter);
        }

        private static Result<IWordFilter> RemoveWords(
            IWordReader wordReader,
            string wordsToExcludeFileName,
            IWordFilter wordFilter)
        {
            if (IsFileNameSet(wordsToExcludeFileName))
            {
                var wordsResult = wordReader.ReadByLines(wordsToExcludeFileName);
                if (!wordsResult.IsSuccess)
                {
                    return Result.Fail<IWordFilter>(wordsResult.Error);
                }

                foreach (var word in wordsResult.GetValueOrThrow())
                {
                    wordFilter.Remove(word.ToLower());
                }
            }

            return Result.Ok(wordFilter);
        }
    }
}