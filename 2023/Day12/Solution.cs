using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.VisualBasic;

namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
class Solution : Solver {

    public object PartOne(string input) {
        var s = 0L;
        foreach(var line in input.Split("\n")) {
            var parts = line.Split(" ");
            var pattern = parts[0];
            
            var nums = parts[1].Split(",").Select(int.Parse);
            var d = SolveLine(pattern, ImmutableStack.CreateRange(nums.Reverse()), new Dictionary<string, long>());
            s += d;
        }
        return s;
    }

    public object PartTwo(string input) {
        var s = 0L;
        foreach(var line in input.Split("\n")) {
            var parts = line.Split(" ");
            var pattern = Unfold(parts[0], '?');
            var nums =  Unfold(parts[1],',').Split(",").Select(int.Parse);
            var d = SolveLine(pattern, ImmutableStack.CreateRange(nums.Reverse()), new Dictionary<string, long>());
            Console.WriteLine(d);
            s += d;
        }
        return s;
    }

    string Unfold(string st, char join) {
        return string.Join(join, [st, st, st, st, st]);
    }

    long SolveLine(string pattern, ImmutableStack<int> nums, Dictionary<string, long> cache) {
        var key = pattern +"," + string.Join(",", nums.Select(n => n.ToString()));

        if (!cache.ContainsKey(key)) {
            long foo() {
                    if (pattern == "") {
                    var ok = nums.All(x=>x==0);
                    return ok ? 1 : 0;
                } else if (!nums.Any(x=>x!=0)) {
                    var ok = pattern.All(ch => ch != '#');
                    return ok ? 1 : 0;
                } else if (pattern[0] == '.') {
                    return SolveLine(pattern[1..], nums, cache);
                } else if (pattern[0] == '#') {
                    var n = nums.Peek();
                    nums = nums.Pop();
                    if (pattern.Length < n) {
                        return 0;
                    } else {
                        if (pattern.Take(n).Any(ch => ch == '.')){
                            return 0;
                        }
                        pattern = pattern[n..];
                        if (pattern.Length > 0) {
                            if (pattern[0] == '#') {
                                return 0;
                            } else {
                                pattern = pattern[1..];
                            }
                        }
                    }
                    return SolveLine(pattern, nums, cache);
                    
                } else {
                    var l = SolveLine("." + pattern[1..], nums, cache);
                    var r = SolveLine("#" + pattern[1..], nums, cache);
                    return l + r; 
                }
            }
            cache[key] = foo();
        }
        return cache[key];
    }
}


// int Solve(string pattern, ImmutableStack<int> nums, string solution) {
//         if (pattern == "") {
//             var ok = nums.All(x=>x==0);
//             if (ok) {
//                 Console.WriteLine(solution);
//             }
//             return ok ? 1 : 0;
//         } else if (!nums.Any(x=>x!=0)) {
//             var ok = pattern.All(ch => ch != '#');
//              if (ok) {
//                 Console.WriteLine(solution+pattern.Replace('?', '.'));
//             }
//             return ok ? 1 : 0;
//         } else if (pattern[0] == '.') {
//             var n = nums.Peek();
//             if (n == 0) {
//                 nums = nums.Pop();
//             }
//             return Solve(pattern[1..], nums, solution+".");
//         } else if (pattern[0] == '#') {
//             var n = nums.Peek();
//             if (n == 0) {
//                 return 0;
//             } else {
//                 nums = nums.Pop();
//                 nums = nums.Push(n-1);
//             }
//             return Solve(pattern[1..], nums, solution+"#");
            
//         } else {
//             var l = Solve("." + pattern[1..], nums, solution);
//             var r = Solve("#" + pattern[1..], nums, solution);
//             return l + r; 
//         }
//     }