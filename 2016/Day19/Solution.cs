using System;
using System.Linq;

namespace AdventOfCode.Y2016.Day19;

[ProblemName("An Elephant Named Joseph")]
class Solution : Solver {

    public object PartOne(string input) {
        var elves = Elves(int.Parse(input));
        return Solve(elves[0], elves[1], elves.Length, 
            (elfVictim, count) => elfVictim.next.next);
    }

    public object PartTwo(string input) {
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
