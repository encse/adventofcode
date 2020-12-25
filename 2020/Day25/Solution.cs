using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day25 {

    [ProblemName("Combo Breaker")]      
    class Solution : Solver {

        public object PartOne(string input) {
            var numbers = input.Split("\n").Select(int.Parse).ToArray();
            
            var loop = 0;
            var subj  = 7L;
            var card = subj;
            var door = subj;
            while(card != numbers[0] && door != numbers[1]){
                card = (card * subj) % 20201227;
                door = (door * subj) % 20201227;
                loop++;
            }
            subj = card == numbers[0] ? numbers[1] : numbers[0];
            var d = subj;
            while(loop > 0){
                d = (d * subj) % 20201227;
                loop --;
            }
            return d;
        }

    }
}