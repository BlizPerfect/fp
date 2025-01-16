using FileSenderRailway;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.CloudLayouterWorkers
{
    internal class NormalizedFrequencyBasedCloudLayouterWorker : ICloudLayouterWorker
    {
        private readonly int maxRectangleWidth;
        private readonly int maxRectangleHeight;
        private readonly Dictionary<string, double> values;
        private readonly string[] keysOrder;
        public string[] KeysOrder => keysOrder.ToArray();

        public NormalizedFrequencyBasedCloudLayouterWorker(
            int maxWidth,
            int maxHeight,
            Dictionary<string, double> normalizedValues,
            bool isSorted = true)
        {
            maxRectangleWidth = maxWidth;
            maxRectangleHeight = maxHeight;
            values = normalizedValues;
            if (isSorted)
            {
                keysOrder = values.OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            }
            else
            {
                keysOrder = values.Keys.ToArray();
            }
        }

        public Result<IEnumerable<(string word, Size size)>> GetNextRectangleProperties()
            => ValidateDimensions()
                .Then(_ => GenerateRectangles())
                .OnFail(error => Result.Fail<IEnumerable<(string word, Size size)>>(error));

        private Result<None> ValidateDimensions()
            => SizeParser.ParseSizeDimension(maxRectangleWidth)
                .Then(_ => SizeParser.ParseSizeDimension(maxRectangleHeight))
                .Then(_ => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));

        private IEnumerable<(string word, Size size)> GenerateRectangles()
        {
            foreach (var key in keysOrder)
            {
                var value = values[key];
                var width = (int)(maxRectangleWidth * value);
                var height = (int)(maxRectangleHeight * value);
                yield return (key, new Size(width, height));
            }
        }
    }
}