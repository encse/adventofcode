using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2016.Day19 {

    class Solution : Solver {

        public string GetName() => "An Elephant Named Joseph";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var elves = Elves(int.Parse(input));
            return Solve(elves[0], elves[1], elves.Length, 
                (elfVictim, count) => elfVictim.next.next);
        }

        int PartTwo(string input) {
            var elves = Elves(int.Parse(input));
            return Solve(elves[0], elves[elves.Length / 2], elves.Length, 
                (elfVictim, count) => count % 2 == 1 ? elfVictim.next : elfVictim.next.next);
        }

        int Solve(Elf elf, Elf elfVictim, int elfCount, Func<Elf, int, Elf> nextVictim) {
            while (elfCount > 1) {
                elfVictim.prev.next = elfVictim.next;
                elfVictim.next.prev = elfVictim.prev;
                elf = elf.next;
                elfCount--;
                elfVictim = nextVictim(elfVictim, elfCount);
            }
            return elf.id;
        }

        Elf[] Elves(int count) {
            var elves = Enumerable.Range(0, count).Select(x => new Elf { id = x + 1 }).ToArray();
            for (var i = 0; i < count; i++) {
                elves[i].prev = elves[(i - 1 + count) % count];
                elves[i].next = elves[(i + 1) % count];
            }
            return elves;
        }

        class Elf {
            public int id;
            public Elf prev;
            public Elf next;
        }
    }
}