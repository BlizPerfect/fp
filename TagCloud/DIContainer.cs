using Autofac;
using TagCloud.CloudLayouters.CircularCloudLayouter;
using TagCloud.CloudLayouters;
using TagCloud.ImageSavers;
using TagCloud.Normalizers;
using TagCloud.WordCounters;
using TagCloud.WordReaders;
using TagCloud.Factories;

namespace TagCloud
{
    public static class DIContainer
    {
        public static IContainer ConfigureContainer(CommandLineOptions options)
        {
            var builder = new ContainerBuilder();

            RegisterSimpleSevice<WordReader, IWordReader>(builder);
            RegisterSimpleSevice<WordFilterFactory, IWordFilterFactory>(builder);
            RegisterSimpleSevice<WordCounter, IWordCounter>(builder);
            RegisterSimpleSevice<Normalizer, INormalizer>(builder);
            RegisterSimpleSevice<CircularCloudLayouter, ICloudLayouter>(builder);
            RegisterSimpleSevice<ImageSaver, IImageSaver>(builder);
            RegisterSimpleSevice<CloudLayouterWorkerFactory, ICloudLayouterWorkerFactory>(builder);
            RegisterSimpleSevice<CloudLayouterPainterFactory, ICloudLayouterPainterFactory>(builder);

            RegisterProgramExecutorService(builder, options);

            return builder.Build();
        }

        private static void RegisterSimpleSevice<TImplementation, TService>(
            ContainerBuilder builder)
            where TImplementation : TService
            where TService : notnull
            => builder
                .RegisterType<TImplementation>()
                .As<TService>()
                .SingleInstance();

        private static void RegisterProgramExecutorService(
            ContainerBuilder builder,
            CommandLineOptions options)
            => builder.RegisterType<ProgramExecutor>()
                .WithParameter("backgroundColor", options.BackgroundColor)
                .WithParameter("textColor", options.TextColor)
                .WithParameter("font", options.Font)
                .WithParameter("isSorted", options.IsSorted)
                .WithParameter("imageSize", options.ImageSize)
                .WithParameter("maxRectangleWidth", options.MaxRectangleWidth)
                .WithParameter("maxRectangleHeight", options.MaxRectangleHeight)
                .WithParameter("imageFileName", options.ImageFileName)
                .WithParameter("dataFileName", options.DataFileName)
                .WithParameter("resultFormat", options.ResultFormat)
                .WithParameter("wordsToIncludeFileName", options.WordsToIncludeFileName!)
                .WithParameter("wordsToExcludeFileName", options.WordsToExcludeFileName!)
                .SingleInstance();
    }
}
