using CommandLine;

namespace TagCloud
{
    public class CommandLineOptions
    {
        [Option(
            "backgroundColor",
            Required = false,
            HelpText = "Цвет заднего фона изображения, например \"White\".")]
        public string BackgroundColor { get; set; } = "White";

        [Option(
            "textColor",
            Required = false,
            HelpText = "Цвет текста на изображении, например \"Black\".")]
        public string TextColor { get; set; } = "Black";

        [Option(
            "font",
            Required = false,
            HelpText = "Шрифт текста на изображении, например \"Arial\".")]
        public string Font { get; set; } = "Arial";

        [Option(
            "nonSorted",
            Required = false,
            HelpText = "Отключение сортировки слов, например \"False\".")]
        public string IsSorted { get; set; } = Boolean.TrueString;

        [Option(
            "size",
            Required = false,
            HelpText = "Размер изображения в формате ШИРИНА:ВЫСОТА, например \"5000:5000\".")]
        public string ImageSize { get; set; } = "5000:5000";

        [Option(
            "maxRectangleWidth",
            Required = false,
            HelpText = "Максимальная ширина прямоугольника, например \"500\".")]
        public string MaxRectangleWidth { get; set; } = "500";

        [Option(
            "maxRectangleHeight",
            Required = false,
            HelpText = "Максимальная высота прямоугольника, например \"200\".")]
        public string MaxRectangleHeight { get; set; } = "200";

        [Option(
            "imageFile",
            Required = false,
            HelpText = "Имя выходного файла изображения, например \"Result\".")]
        public string ImageFileName { get; set; } = "Result";

        [Option(
            "dataFile",
            Required = true,
            HelpText = "Полный путь к файлу с исходными данными, например \"C:\\MyWorkSpace\\Coding\\MyCodes\\CSharp\\PostUniversityEra\\ShporaHomeworks\\Homework.6.TagCloudII\\SnowWhite.txt\".")]
        public required string DataFileName { get; set; }

        [Option(
            "resultFormat",
            Required = false,
            HelpText = "Формат создаваемого изображение, например \"png\".")]
        public string ResultFormat { get; set; } = "png";

        [Option(
            "wordsToIncludeFile",
            Required = false,
            HelpText = "Полный путь к файлу со словами для добавления в фильтр \"скучных слов\", например \"C:\\MyWorkSpace\\Coding\\MyCodes\\CSharp\\PostUniversityEra\\ShporaHomeworks\\Homework.6.TagCloudII\\WordsToInclude.txt\".")]
        public string? WordsToIncludeFileName { get; set; } = null; //+

        [Option(
            "wordsToExcludeFile",
            Required = false,
            HelpText = "Полный путь к файлу со словами для исключения из фильтра \"скучных слов\", например \"C:\\MyWorkSpace\\Coding\\MyCodes\\CSharp\\PostUniversityEra\\ShporaHomeworks\\Homework.6.TagCloudII\\WordsToExclude.txt\".")]
        public string? WordsToExcludeFileName { get; set; } = null; //+
    }
}