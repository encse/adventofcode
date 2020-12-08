using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day08 {

    [ProblemName("Handheld Halting")]      
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var stms = input.Split("\n");
            var ip = 0;
            var acc = 0;
            var seen = new HashSet<int>();
            while(true) {
                var stm = stms[ip].Split(" ");
                if(seen.Contains(ip)){
                    return acc;
                }
                seen.Add(ip);
                switch(stm[0]){
                    case "nop": ip++; break;
                    case "acc": acc+= int.Parse(stm[1]); ip++; break;
                    case "jmp": ip += int.Parse(stm[1]); break;
                }
            }
        }

        int PartTwo(string input) {
            var stms = input.Split("\n");

            for(var patch =0;patch<stms.Length;patch++){
                if(stms[patch].Split(" ")[0]=="acc") {
                    continue;
                }

                var ip = 0;
                var acc = 0;
                var timeout = 10000000/stms.Length;
                
                while(timeout > 0) {
                    timeout--;
                    if(ip>=stms.Length){
                        return acc;
                    }
                    var stm = stms[ip].Split(" ");
                    if(patch == ip){
                        stm[0] = stm[0] == "nop" ? "jmp" : "nop";
                    }
                    switch(stm[0]){
                        case "nop": ip++; break;
                        case "acc": acc+= int.Parse(stm[1]); ip++; break;
                        case "jmp": ip += int.Parse(stm[1]); break;
                    }
                }
            }
            throw new Exception();
        }
    }
}