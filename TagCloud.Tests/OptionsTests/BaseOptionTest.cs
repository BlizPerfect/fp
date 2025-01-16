using TagCloud.Tests.Utilities;

namespace TagCloud.Tests.OptionsTests
{
    internal abstract class BaseOptionTest
    {
        protected readonly string directoryPath;
        protected readonly string dataFile;
        protected readonly string imageFile;
        protected readonly CommandLineOptions options;

        protected BaseOptionTest(string testName)
        {
            directoryPath = $"TempFilesFor{testName}Tests";
            dataFile = Path.Combine(directoryPath, "TestData.txt");
            imageFile = Path.Combine(directoryPath, "Test");

            options = new CommandLineOptions
            {
                DataFileName = dataFile,
                ImageFileName = imageFile,
            };
        }

        [OneTimeSetUp]
        protected void Init()
            => FileUtilities.CreateDataFile(
                directoryPath,
                dataFile,
                ValidValues.ValidDataFileContent);

        [OneTimeTearDown]
        protected void OneTimeCleanup()
            => FileUtilities.DeleteDirectory(directoryPath);
    }
}
