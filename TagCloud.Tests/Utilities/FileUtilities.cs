namespace TagCloud.Tests.Utilities
{
    internal static class FileUtilities
    {
        public static void CreateDataFile(string directoryPath, string dataFile, string[] content)
        {
            Directory.CreateDirectory(directoryPath);
            File.WriteAllLines(dataFile, content);
        }

        public static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}
