using System;

public static class NumberFormatter
{
    private static readonly string[] suffixes =
    {
        "", "K", "M", "B", "T",
        "Qa", "Qi", "Sx", "Sp", "Oc", "No"
    };

    public static string Format(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return value.ToString();

        double absValue = Math.Abs(value);

        if (absValue < 1000)
            return Math.Floor(value).ToString("N0");

        int index = 0;

        while (absValue >= 1000 && index < suffixes.Length - 1)
        {
            absValue /= 1000;
            index++;
        }

        double shortened = Math.Floor(absValue * 10) / 10;

        return $"{(value < 0 ? "-" : "")}{shortened:0.#}{suffixes[index]}";
    }
}