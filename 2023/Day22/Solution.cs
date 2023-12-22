namespace AdventOfCode.Y2023.Day22;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record Range(int begin, int end);

record Block(Range x, Range y, Range z) {
    public int Bottom => z.begin;
    public int Top => z.end;
}

record Supports(
    Dictionary<Block,HashSet<Block>> blocksAbove, 
    Dictionary<Block, HashSet<Block>> blocksBelow
);

[ProblemName("Sand Slabs")]
class Solution : Solver {

    public object PartOne(string input) => Kaboom(input).Count(x => x == 0);
    public object PartTwo(string input) => Kaboom(input).Sum();

    // goes over the blocks in the input and desintegrates them one by one.
    // returns the number of blocks that this would case falling.
    IEnumerable<int> Kaboom(string input) {
        var blocks = Fall(ParseBlocks(input));
        var supports = GetSupports(blocks);

        foreach (var desintegratedBlock in blocks) {
            var q = new Queue<Block>();
            q.Enqueue(desintegratedBlock);

            var falling = new HashSet<Block>();
            while (q.TryDequeue(out var block)) {
                falling.Add(block);
                var blocksStartFalling =
                    from blockT in supports.blocksAbove[block]
                    where supports.blocksBelow[blockT].IsSubsetOf(falling)
                    select blockT;

                foreach (var blockT in blocksStartFalling) {
                    q.Enqueue(blockT);
                }
            }
            yield return falling.Count - 1;
        }
    }

    // applies 'gravity' to the blocks
    Block[] Fall(Block[] blocks) {
        blocks = blocks.OrderBy(block => block.Bottom).ToArray();
        for (var i = 0; i < blocks.Length; i++) {
            var bottom = 1;
            for (var j = 0; j < i; j++) {
                if (IntersectsXY(blocks[i], blocks[j])) {
                    bottom = Math.Max(bottom, blocks[j].Top + 1);
                }
            }

            var fall = blocks[i].Bottom - bottom;
            blocks[i] = blocks[i] with {
                z = new Range(blocks[i].Bottom - fall, blocks[i].Top - fall)
            };
        }
        return blocks;
    }

    // returns a pair of dictionaries that tells the upper and lower neighbours for each block
    Supports GetSupports(Block[] blocks) {
        var blocksAbove = blocks.ToDictionary(b => b, _ => new HashSet<Block>());
        var blocksBelow = blocks.ToDictionary(b => b, _ => new HashSet<Block>());
        for (var i = 0; i < blocks.Length; i++) {
            var blockLo = blocks[i];
            for (var j = i + 1; j < blocks.Length; j++) {
                var blockHi = blocks[j];
                if (blockLo.Top == blockHi.Bottom - 1 && IntersectsXY(blockHi, blockLo)) {
                    blocksBelow[blockHi].Add(blockLo);
                    blocksAbove[blockLo].Add(blockHi);
                }
            }
        }
        return new Supports(blocksAbove, blocksBelow);
    }

    bool IntersectsXY(Block blockA, Block blockB) =>
        Intersects(blockA.x, blockB.x) && Intersects(blockA.y, blockB.y);

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) => r1.begin <= r2.end && r2.begin <= r1.end;

    Block[] ParseBlocks(string input) => (
        from p in Enumerable.Zip(input.Split('\n'), Enumerable.Range(1, 100000))
        let line = p.First
        let id = p.Second
        let v = (from m in Regex.Matches(line, @"\d+") select int.Parse(m.Value)).ToArray()
        select new Block(new Range(v[0], v[3]), new Range(v[1], v[4]), new Range(v[2], v[5]))
    ).ToArray();
}