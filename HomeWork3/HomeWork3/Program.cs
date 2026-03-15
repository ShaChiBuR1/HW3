using System;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Задание 1");
        string[] input1 = { "abc", "abcde", "a", "abcdefg", "abcdefghi" };
        string[] result1 = CenterAlignStrings(input1);
        foreach (var s in result1)
        {
            Console.WriteLine($"\"{s}\"");
        }

        Console.WriteLine("Задание 2");
        string[] forbidden = { "плохой", "злой" };
        string text2 = "Это плохой и злой человек.";
        string result2 = ReplaceForbiddenWords(text2, forbidden);
        Console.WriteLine(result2);

        Console.WriteLine("Задание 3");
        string[] input3 = { "Hello world test", "Short a longword", "One" };
        string[] result3 = FindLongestWords(input3);
        Console.WriteLine(string.Join(", ", result3));

        Console.WriteLine("Задание 5");
        string text5 = "Индекс 123456 и 654321, а также 12345 и 1234567";
        int count5 = CountPostalCodes(text5);
        Console.WriteLine($"Количество индексов: {count5}");

        Console.WriteLine("Задание 6"); 
        int count6 = CountPostalCodesRegex(text5);
        Console.WriteLine($"Количество индексов (regex): {count6}");

        Console.ReadKey();
    }

    static string[] CenterAlignStrings(string[] strings)
    {
        if (strings == null || strings.Length == 0 || strings.Length % 2 == 0)
        {
            throw new ArgumentException("Массив должен быть нечётной длины");
        }

        int maxLength = strings.Max(s => s.Length);
        string[] result = new string[strings.Length];

        for (int i = 0; i < strings.Length; i++)
        {
            int totalPadding = maxLength - strings[i].Length;
            int leftPadding = totalPadding / 2;
            int rightPadding = totalPadding - leftPadding;
            result[i] = new string(' ', leftPadding) + strings[i] + new string(' ', rightPadding);
        }

        return result;
    }

    static string ReplaceForbiddenWords(string text, string[] forbiddenWords)
    {
        string result = text;
        foreach (var word in forbiddenWords)
        {
            string pattern = $@"\b{Regex.Escape(word)}\b";
            string replacement = new string('*', word.Length);
            result = Regex.Replace(result, pattern, replacement, RegexOptions.IgnoreCase);
        }
        return result;
    }

    static string[] FindLongestWords(string[] strings)
    {
        char[] separators = { ' ', '.', ',', '!', '?' };
        string[] longestWords = new string[strings.Length];

        for (int i = 0; i < strings.Length; i++)
        {
            string[] words = strings[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0)
            {
                longestWords[i] = words.OrderByDescending(w => w.Length).First();
            }
            else
            {
                longestWords[i] = string.Empty;
            }
        }

        return longestWords.OrderBy(w => w.Length).ToArray();
    }

    static int CountPostalCodes(string text)
    {
        int count = 0;
        int i = 0;

        while (i <= text.Length - 6)
        {
            bool isAllDigits = true;
            for (int j = 0; j < 6; j++)
            {
                if (!char.IsDigit(text[i + j]))
                {
                    isAllDigits = false;
                    break;
                }
            }

            if (isAllDigits)
            {
                bool validStart = (i == 0 || !char.IsDigit(text[i - 1]));
                bool validEnd = (i + 6 >= text.Length || !char.IsDigit(text[i + 6]));

                if (validStart && validEnd)
                {
                    count++;
                    i += 6;
                    continue;
                }
            }

            i++;
        }

        return count;
    }

    static int CountPostalCodesRegex(string text)
    {
        string pattern = @"\b\d{6}\b";
        MatchCollection matches = Regex.Matches(text, pattern);
        return matches.Count;
    }
}