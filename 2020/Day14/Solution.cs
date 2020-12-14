using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day14 {

    [ProblemName("Docking Data")]      
    class Solution : Solver {

        public object PartOne(string input) {
            var mem = new Dictionary<long, long>();
            var orMask = 0L;
            var andMask = 0xffffffffffffffL;
            foreach(var line in input.Split("\n")){
                if (line.StartsWith("mask")){
                    var mask = line.Split(" = ")[1];
                    andMask = Convert.ToInt64(mask.Replace("X", "1"),2);
                    orMask = Convert.ToInt64(mask.Replace("X", "0"),2);
                } else {
                    var num = Regex.Matches(line, "\\d+").Select(match => long.Parse(match.Value)).ToArray();
                    mem[num[0]] = (num[1] & andMask) | orMask;
                }
            }
            return mem.Values.Sum();
        }

        public object PartTwo(string input) {
            var mem = new Dictionary<long, long>();
            var mask ="".PadLeft(36, '0');
            foreach(var line in input.Split("\n")){
                if (line.StartsWith("mask")){
                    mask = line.Split(" = ")[1].PadLeft(36, '0');
                    
                } else {
                    var num = Regex.Matches(line, "\\d+").Select(match => long.Parse(match.Value)).ToArray();
                    var baseAddr = num[0];
                    var value = num[1];
                    foreach(var addr in Addresses(Convert.ToString(baseAddr, 2).PadLeft(36, '0'), mask)){
                        mem[Convert.ToInt64(addr, 2)] = value;
                    }
                  
                }
            }
            return mem.Values.Sum();
        }

        IEnumerable<string> Addresses(string baseAddr, string mask){
            if(mask == ""){
                yield return "";
            } else {
                foreach(var suffix in Addresses(baseAddr.Substring(1), mask.Substring(1))){
                    if (mask[0] == '0') {
                        yield return baseAddr[0] + suffix;
                    } else if (mask[0] == '1'){
                        yield return "1" + suffix;
                    } else {
                        yield return "0" + suffix;
                        yield return "1" + suffix;
                    }
                }
            }
        }
    }
}