using FileSenderRailway;

namespace TagCloud.Parsers
{
    internal static class BoolParser
    {
        public static Result<bool> ParseIsSorted(string value)
        {
            if (value == false.ToString() || value == true.ToString())
            {
                return (value == true.ToString()).AsResult();
            }
            return Result.Fail<bool>($"Неизвестный параметр сортировки {value}");
        }
    }
}
