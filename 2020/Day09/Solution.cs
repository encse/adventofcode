using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day09 {

    [ProblemName("Encoding Error")]      
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            var lines = input.Split("\n").Select(long.Parse).ToList();
            var numbers = new Queue<long>(lines.Take(25));
            for(var i=25;i<lines.Count;i++){
                var num = lines[i];
                var found = false;
                for(var j =0;j<numbers.Count;j++){
                    for(var k =j+1;k<numbers.Count;k++){
                        if(numbers.ElementAt(j) + numbers.ElementAt(k) == num){
                            found = true;
                        }
                    }
                }
                if(!found){
                    return num;
                }
                numbers.Dequeue();
                numbers.Enqueue(num);
            }
            throw new Exception();
        }

        long PartTwo(string input) {
            var d = PartOne(input);

            var lines = input.Split("\n").Select(long.Parse).ToList();

            for(var j =0;j<lines.Count;j++){
                var s = lines[j];
                for(var k =j+1;k<lines.Count;k++){
                    s+= lines[k];

                    if(s == d){
                        return lines.Skip(j-1).Take(k-j+1).Min() + lines.Skip(j-1).Take(k-j+1).Max();
                    } if(s>d) {
                        break;
                    }
                }
            }
            throw new Exception();
        }
    }
}