using System;
using System.Linq;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input) => 
        input.Split("\n").Select(GetNumber).Sum();

    public object PartTwo(string input)=>
        input.Split("\n").Select(PreProcess).Select(GetNumber).Sum();

    int GetNumber(string line) {
        var digits = line.Where(char.IsDigit).Select(ch => ch - '0').ToArray();
        var (first, last) = (digits.First(), digits.Last());
        return 10 * first + last;
    } 

    string PreProcess(string line) {
        var st = "";
        for (var i =0;st == "" && i < line.Length;i++) {
            if (line[i..].StartsWith("0")) st += "0";
            if (line[i..].StartsWith("1")) st += "1";
            if (line[i..].StartsWith("2")) st += "2";
            if (line[i..].StartsWith("3")) st += "3";
            if (line[i..].StartsWith("4")) st += "4";
            if (line[i..].StartsWith("5")) st += "5";
            if (line[i..].StartsWith("6")) st += "6";
            if (line[i..].StartsWith("7")) st += "7";
            if (line[i..].StartsWith("8")) st += "8";
            if (line[i..].StartsWith("9")) st += "9";
            if (line[i..].StartsWith("one")) st += "1";
            if (line[i..].StartsWith("two")) st += "2";
            if (line[i..].StartsWith("three")) st += "3";
            if (line[i..].StartsWith("four")) st += "4";
            if (line[i..].StartsWith("five")) st += "5";
            if (line[i..].StartsWith("six")) st += "6";
            if (line[i..].StartsWith("seven")) st += "7";
            if (line[i..].StartsWith("eight")) st += "8";
            if (line[i..].StartsWith("nine")) st += "9";
        }

        for (var i =line.Length-1;st.Length == 1;i--) {
            if (line[i..].StartsWith("0")) st += "0";
            if (line[i..].StartsWith("1")) st += "1";
            if (line[i..].StartsWith("2")) st += "2";
            if (line[i..].StartsWith("3")) st += "3";
            if (line[i..].StartsWith("4")) st += "4";
            if (line[i..].StartsWith("5")) st += "5";
            if (line[i..].StartsWith("6")) st += "6";
            if (line[i..].StartsWith("7")) st += "7";
            if (line[i..].StartsWith("8")) st += "8";
            if (line[i..].StartsWith("9")) st += "9";
            if (line[i..].StartsWith("one")) st += "1";
            if (line[i..].StartsWith("two")) st += "2";
            if (line[i..].StartsWith("three")) st += "3";
            if (line[i..].StartsWith("four")) st += "4";
            if (line[i..].StartsWith("five")) st += "5";
            if (line[i..].StartsWith("six")) st += "6";
            if (line[i..].StartsWith("seven")) st += "7";
            if (line[i..].StartsWith("eight")) st += "8";
            if (line[i..].StartsWith("nine")) st += "9";
        }

        return st;
    }
}
