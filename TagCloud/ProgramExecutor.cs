using FileSenderRailway;
using TagCloud.CloudLayouters;
using TagCloud.CloudLayouterWorkers;
using TagCloud.ImageSavers;
using TagCloud.Factories;
using TagCloud.Parsers;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;

namespace TagCloud
{
    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
    internal class ProgramExecutor(
        string backgroundColor,
        string textColor,
        string font,
        string isSorted,
        string imageSize,
        string maxRectangleWidth,
        string maxRectangleHeight,
        string imageFileName,
        string dataFileName,
        string resultFormat,
        string? wordsToIncludeFileName,
        string? wordsToExcludeFileName,
        ICloudLayouterWorkerFactory cloudLayouterWorkerFactory,
        ICloudLayouterPainterFactory cloudLayouterPainterFactory,
        ICloudLayouter layouter,
        IImageSaver imageSaver)
    {
        public Result<None> Execute()
            => ProcessTags()
                .Then(tags => Draw(tags))
                .Then(image => Save(image))
                .OnFail(error => Result.Fail<None>(error));

        private Result<ICloudLayouterWorker> CreateWorker()
            => SizeParser.ParseSizeDimension(maxRectangleWidth)
                .Then(width => SizeParser.ParseSizeDimension(maxRectangleHeight)
                    .Then(height => BoolParser.ParseIsSorted(isSorted)
                        .Then(isSorted => cloudLayouterWorkerFactory.Create(
                            dataFileName,
                            wordsToIncludeFileName,
                            wordsToExcludeFileName,
                            width,
                            height,
                            isSorted))
                    )
                )
                .OnFail(error => Result.Fail<ICloudLayouterWorker>(error));

        private Result<List<Tag>> ProcessTagsWithWorker(ICloudLayouterWorker worker)
            => worker
                .GetNextRectangleProperties()
                .Then(rectangleProperties =>
                {
                    var tags = new List<Tag>();
                    foreach (var rectangleProperty in rectangleProperties)
                    {
                        var tagResult = layouter
                            .PutNextRectangle(rectangleProperty.size)
                            .Then(tagSize => new Tag(rectangleProperty.word, tagSize));

                        if (!tagResult.IsSuccess)
                        {
                            return Result.Fail<List<Tag>>(tagResult.Error);
                        }

                        tags.Add(tagResult.GetValueOrThrow());
                    }
                    return Result.Ok(tags);
                })
                .OnFail(error => Result.Fail<List<Tag>>(error));

        private Result<List<Tag>> ProcessTags()
            => CreateWorker()
                .Then(worker => ProcessTagsWithWorker(worker))
                .OnFail(error => Result.Fail<List<Tag>>(error));

        private Result<Bitmap> Draw(List<Tag> tags)
        => SizeParser.ParseImageSize(imageSize)
            .Then(size => ColorParser.ParseColor(backgroundColor)
                .Then(bgColor => ColorParser.ParseColor(textColor)
                    .Then(textColor => FontParser.ParseFont(font)
                        .Then(font => cloudLayouterPainterFactory.Create(size, bgColor, textColor, font))
                        .Then(painter => painter.Draw(tags))
                    )
                )
            )
            .OnFail(error => Result.Fail<Bitmap>(error));

        private Result<None> Save(Bitmap image)
            => imageSaver.SaveFile(image, imageFileName, resultFormat)
                .Then(_ => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));

        private Result<None> DrawAndSaveImage(List<Tag> tags)
            => Draw(tags)
                .Then(image => Save(image))
                .OnFail(error => Result.Fail<None>(error));
    }
}