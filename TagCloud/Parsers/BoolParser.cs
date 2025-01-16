using FileSenderRailway;

namespace TagCloud.Parsers
{
    internal static class BoolParser
    {
        public static Result<bool> ParseIsSorted(string value)
        {
            if (value == bool.FalseString || value == bool.TrueString)
            {
                return Convert.ToBoolean(value).AsResult();
            }
            return Result.Fail<bool>($"Неизвестный параметр сортировки \"{value}\"");
        }
    }
}
