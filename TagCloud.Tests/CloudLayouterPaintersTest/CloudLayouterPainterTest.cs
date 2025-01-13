using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouterPainters;
using TagCloud.WordReaders;

namespace TagCloud.Tests.CloudLayouterPaintersTest
{
    internal class CloudLayouterPainterTest
    {
        private CloudLayouterPainter painter;

        [SetUp]
        public void SetUp()
        {
            painter = new CloudLayouterPainter(new Size(1, 1));
        }

        [Test]
        public void Draw_ThrowsArgumentException_WithEmptyTags()
        {
            var expected = Result.Fail<Bitmap>("Список тегов пуст");
            var actual = painter.Draw(new List<Tag>());
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Draw_ThrowsArgumentNullException_WithTagsAsNull()
        {

            var expected = Result.Fail<Bitmap>("Tags передан как null");
            var actual = painter.Draw(null!);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
