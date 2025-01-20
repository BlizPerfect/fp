using FileSenderRailway;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.CloudLayouterPainters;

namespace TagCloud.Tests.CloudLayouterPaintersTest
{
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
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
        public void Draw_ThrowsException_WithTooSmallToFitImage()
        {
            var expected = Result
                .Fail<Bitmap>("Все прямоугольники не помещаются на изображение");
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
