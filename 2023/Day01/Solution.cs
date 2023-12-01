using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Text;
using System.Reflection;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input) {
        var regex = new Regex("([\\d])", RegexOptions.Compiled);
        var result = input.Split(System.Environment.NewLine)
             .Select(s => regex.Matches(s))
             .Select(r => r.First().Groups[1].Value + r.Last().Groups[1].Value)
             .Select(int.Parse)
             .Sum();
        return result;
    }

    public object PartTwo(string input)
    {
        var test = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";

        var numbers = new Dictionary<string, int>
            {
                { "one", 1},
                { "two", 2},
                { "three", 3},
                { "four", 4},
                { "five", 5},
                { "six", 6},
                { "seven", 7},
                { "eight", 8},
                { "nine", 9},
                { "zero", 0},
            };

        int getInt(string line, bool useWords = true)
        {
            int? first = null;
            for(var i = 0; i < line.Length; i++)
            {
                first = FindFirstNumberInSubstring(line, numbers, i, useWords);
                if (first.HasValue) break;
            }

            int? last = null;
            for(var i = line.Length -1; i >= 0; i--)
            {
                last = FindFirstNumberInSubstring(line, numbers, i, useWords);
                if (last.HasValue) break;

            }
            return first.Value * 10 + last.Value;
        }
        // Console.WriteLine($"eightwothree -> {getInt("eightwothree")}. should be 83");
        // Console.WriteLine($"abcone2threexyz -> {getInt("abcone2threexyz")}. should be 13");
        // Console.WriteLine($"xtwone3four -> {getInt("xtwone3four")}. should be 24");
        // Console.WriteLine($"4nineeightseven2 -> {getInt("4nineeightseven2")}. should be 42");
        // Console.WriteLine($"zoneight234 -> {getInt("zoneight234")}. should be 14");
        // Console.WriteLine($"7pqrstsixteen -> {getInt("7pqrstsixteen")}. should be 76");
        
        return input.Split(System.Environment.NewLine)
                          .Select(l => getInt(l, true))
                          .Sum();
    }

    private static int? FindFirstNumberInSubstring(string line, Dictionary<string, int> numbers, int i, bool useWords = true)
    {
        if (line[i].IsDigit())
            return line[i] - '0';

        if (!useWords) return null;

        foreach (var number in numbers)
        {
            if (line.Substring(i).StartsWith(number.Key))
                return number.Value;
        }

        return null;
    }
}
