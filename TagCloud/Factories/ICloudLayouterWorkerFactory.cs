using FileSenderRailway;
using TagCloud.CloudLayouterWorkers;

namespace TagCloud.Factories
{
    internal interface ICloudLayouterWorkerFactory
    {
        public Result<ICloudLayouterWorker> Create(
            string dataFileName,
            string? wordsToIncludeFileName,
            string? wordsToExcludeFileName,
            int maxRectangleWidth,
            int maxRectangleHeight,
            bool isSorted);
    }
}
