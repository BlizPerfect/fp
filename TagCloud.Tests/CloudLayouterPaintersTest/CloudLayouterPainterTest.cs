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
        private static TestCaseData[] invalidTestCases = new TestCaseData[]
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
    }
}