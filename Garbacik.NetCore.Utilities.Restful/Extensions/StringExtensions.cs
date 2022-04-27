using System;
using System.Collections.Generic;
using System.Linq;

namespace Garbacik.NetCore.Utilities.Restful.Extensions;

public static class StringExtensions
{
    public static string PrepareString(this string source, Dictionary<string, string> replaceParts)
    {
        foreach (KeyValuePair<string, string> pair in replaceParts)
        {
            source = pair.Key.StartsWith("{") && pair.Key.EndsWith("}")
                ? source.Replace(pair.Key, pair.Value)
                : source.Replace($"{{{pair.Key}}}", pair.Value);
        }

        return source;
    }

    public static string ToPascalCase(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return source;

        var split = source
            .Split('_')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .SelectMany(x => x.SplitByUpperChar())
            .ToList();

        var pascalCaseWords = split
            .Select(word =>
            {
                if (word.Length == 1)
                    return word
                        .ToUpper();

                return word
                           .ElementAt(0)
                           .ToString()
                           .ToUpper() +
                       word[1..]
                           .ToLower();
            });

        return string.Join(string.Empty, pascalCaseWords);
    }

    public static string[] SplitByUpperChar(this string source)
    {
        var words = new List<string>();
        var word = string.Empty;
        
        for (var charIndex = 0; charIndex < source.Length; charIndex++)
        {
            var c = source[charIndex];
            if (charIndex == 0 || c.IsUpper())
            {
                if(!string.IsNullOrWhiteSpace(word))
                    words.Add(word);

                word = c.ToString();
            }
            else
            {
                word += c.ToString();
            }
        }
        
        if(!string.IsNullOrWhiteSpace(word))
            words.Add(word);

        return words.ToArray();
    }

    public static string ToCamelCase(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return source;

        var pascalCase = source
            .ToPascalCase();

        return pascalCase.Length == 1
            ? pascalCase.ToLower()
            : pascalCase
                  .ElementAt(0)
                  .ToString()
                  .ToLower() +
              pascalCase[1..];
    }

    public static bool IsUpper(this char c) =>
        c.ToString().ToUpper() == c.ToString();


    public static bool IsUpper(this string s) =>
        s.ToUpper() == s;

    public static bool IsLower(this char c) =>
        c.ToString().ToLower() == c.ToString();


    public static bool IsLower(this string s) =>
        s.ToLower() == s;
}