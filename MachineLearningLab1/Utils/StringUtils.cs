namespace MachineLearningLab1.Utils
{
    internal static class StringUtils
    {
        public static string GetRidOfRedundantSymbols(this string str) =>
            // TODO: add removal of one-quotes in such cases like 'fuck', 'camel' etc
            new(str
                .Where(c => c == '\'' || char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                .Select(char.ToUpper)
                .ToArray()
            );
    }
}
