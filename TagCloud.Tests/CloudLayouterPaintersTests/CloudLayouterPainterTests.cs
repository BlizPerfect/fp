using FileSenderRailway;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagCloud.CloudLayouterPainters;

namespace TagCloud.Tests.CloudLayouterPaintersTests
{
    [TestFixture]
    [SuppressMessage(
        "Interoperability",
        "CA1416:Проверка совместимости платформы",
        Justification = "Код предназначен для выполнения только на Windows 6.1 и новее")]
    internal class CloudLayouterPainterTests
    {
        private static readonly TestCaseData[] invalidTestCases = new TestCaseData[]
        {
            new TestCaseData(
                "Список тегов пуст",
                new  List<Tag>())
            .SetArgDisplayNames("EmptyTags"),
            new TestCaseData(
                "Tags передан как null",
                null)
            .SetArgDisplayNames("TagsAsNull"),
            new TestCaseData(
                "Все прямоугольники не помещаются на изображение",
                new Tag[]
                {
                    new Tag(
                        "Test",
                        new Rectangle(new Point(0, 0), new Size(100, 100)))
                })
            .SetArgDisplayNames("TooSmallToFitImage"),
        };

        [TestCaseSource(nameof(invalidTestCases))]
        public void Draw_ThrowsException_WithInvalidCases(string errorMessage, IList<Tag> tags)
        {
            var painter = new CloudLayouterPainter(new Size(1, 1));
            var expected = Result.Fail<Bitmap>(errorMessage);
            var actual = painter.Draw(tags);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Draw_ReturnsBitmapWithCorrectSize()
        {
            var size = new Size(1000, 1000);
            var tags = new List<Tag>
            {
                new Tag(
                    "Test",
                    new Rectangle(
                        new Point(size.Width / 2, size.Height / 2),
                        new Size(size.Width / 10, size.Height / 10)))
            };

            var painter = new CloudLayouterPainter(size);
            var result = painter.Draw(tags);

            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Size.Should().Be(size);
        }
    }
}