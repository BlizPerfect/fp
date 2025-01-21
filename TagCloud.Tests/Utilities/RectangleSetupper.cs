using System.Drawing;
using TagCloud.CloudLayouters.CircularCloudLayouter;
using TagCloud.CloudLayouterWorkers;

namespace TagCloud.Tests.Utilities
{
    internal class RectangleSetupper
    {
        private readonly List<Tag> tags = new List<Tag>();
        public List<Tag> Tags => tags.ToList();
        public Rectangle[] Rectangles() => tags.Select(x => x.Rectangle).ToArray();

        public RectangleSetupper(
            int minRectangleWidth = 30,
            int maxRectangleWidth = 70,
            int minRectangleHeight = 20,
            int maxRectangleHeight = 50,
            int rectanglesCount = 1000)
        {
            var circularCloudLayouter = new CircularCloudLayouter();
            var randomWorker = new RandomCloudLayouterWorker(
                minRectangleWidth,
                maxRectangleWidth,
                minRectangleHeight,
                maxRectangleHeight);
            foreach (var rectangleProperty in randomWorker
                .GetNextRectangleProperties().GetValueOrThrow().Take(rectanglesCount))
            {
                tags.Add(
                    new Tag(
                        "Test",
                        circularCloudLayouter.PutNextRectangle(rectangleProperty.size)
                        .GetValueOrThrow()));
            }
        }
    }
}
