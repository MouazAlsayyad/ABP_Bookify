namespace ITE.Bookify;
public static class SeederStringExtensions
{
    public static string ClampLength(this string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Length <= maxLength ? str : str.Substring(0, maxLength);
    }
}