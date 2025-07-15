public static class ReadingTimeEstimator
{
    private const float WordsPerSecond = 3.5f;

    public static float EstimateReadingTime(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0f;

        int wordCount = text.Split(' ', '\n', '\t').Length;
        return wordCount / WordsPerSecond;
    }
}
