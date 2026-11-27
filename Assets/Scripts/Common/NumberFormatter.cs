using System;

public static class NumberFormatter
{
    private static readonly string[] _suffixes =
    {
        "", "K", "M", "B", "T",
        "Qa", "Qi", "Sx", "Sp", "Oc", "No"
    };

    /// <summary>
    /// 숫자 크기에 맞는 단위를 붙여 표시 문자열을 만든다.
    /// </summary>
    public static string Format(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return value.ToString();
        }

        double absValue = Math.Abs(value);

        if (absValue < 1000)
        {
            return Math.Floor(value).ToString("N0");
        }

        int index = 0;

        while (absValue >= 1000 && index < _suffixes.Length - 1)
        {
            absValue /= 1000;
            index++;
        }

        double shortened = Math.Floor(absValue * 10) / 10;

        return $"{(value < 0 ? "-" : "")}{shortened:0.#}{_suffixes[index]}";
    }

    public static string FormatPercent(double value)
    {
        return $"{value:0.#}%";
    }
}