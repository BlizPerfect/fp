using FileSenderRailway;
using System.Drawing;
using TagCloud.Parsers;

namespace TagCloud.CloudLayouterWorkers
{
    internal class NormalizedFrequencyBasedCloudLayouterWorker : ICloudLayouterWorker
    {
        public readonly int MaxRectangleWidth;
        public readonly int MaxRectangleHeight;
        private readonly Dictionary<string, double> values;
        private readonly string[] keysOrder;
        public string[] KeysOrder => keysOrder;

        public NormalizedFrequencyBasedCloudLayouterWorker(
            int maxRectangleWidth,
            int maxRectangleHeight,
            Dictionary<string, double> normalizedValues,
            bool isSorted = true)
        {
            MaxRectangleWidth = SizeParser.ParseSizeDimension(maxRectangleWidth).GetValueOrThrow();
            MaxRectangleHeight = SizeParser.ParseSizeDimension(maxRectangleHeight).GetValueOrThrow();
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

        public IEnumerable<(string word, Size size)> GetNextRectangleProperties()
        {
            foreach (var key in keysOrder)
            {
                var value = values[key];
                var width = (int)(MaxRectangleWidth * value);
                var height = (int)(MaxRectangleHeight * value);
                yield return (key, new Size(width, height));
            }
        }
    }
}
