using FileSenderRailway;
using FluentAssertions;
using System.Drawing;
using TagCloud.CloudLayouterPainters;

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
        public void Draw_ThrowsException_WithEmptyTags()
        {
            var expected = Result.Fail<Bitmap>("Список тегов пуст");
            var actual = painter.Draw(new List<Tag>());
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Draw_ThrowsException_WithTagsAsNull()
        {

            var expected = Result.Fail<Bitmap>("Tags передан как null");
            var actual = painter.Draw(null!);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Draw_ThrowsException_WithTooSmallTiFitImage()
        {
            var expected = Result
                .Fail<Bitmap>("Все прямоугольники не помещаются на изображение. Измените его размеры");
            var actual = painter
                .Draw(
                    new Tag[]
                    {
                        new Tag(
                            "Test",
                            new Rectangle(new Point(0, 0), new Size(100, 100)))
                    });
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
