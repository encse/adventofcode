namespace AdventOfCode.Y2024.Day09;

using System.Linq;

using Fs = System.Collections.Generic.LinkedList<Block>;
using Node = System.Collections.Generic.LinkedListNode<Block>;
record struct Block(int fileId, int length) { }

[ProblemName("Disk Fragmenter")]
class Solution : Solver {

    public object PartOne(string input) => Checksum(CompactFs(Parse(input), fragmentsEnabled: true));

    public object PartTwo(string input) => Checksum(CompactFs(Parse(input), fragmentsEnabled: false));

    // moves used blocks of the filesystem towards the beginning of the disk using RelocateBlock
    Fs CompactFs(Fs fs, bool fragmentsEnabled) {
        var (i, j) = (fs.First, fs.Last);
        while (i != j) {
            if (i.Value.fileId != -1) {
                i = i.Next;
            } else if (j.Value.fileId == -1) {
                j = j.Previous;
            } else {
                RelocateBlock(fs, i, j, fragmentsEnabled);
                j = j.Previous;
            }
        }
        return fs;
    }

    // Relocates the contents of block `j` to a free space starting after the given node `start`. 
    // - Searches for the first suitable free block after `start`.
    // - If a block of equal size is found, `j` is moved entirely to that block.
    // - If a larger block is found, part of it is used for `j`, and the remainder is split into 
    //   a new free block.
    // - If a smaller block is found and fragmentation is enabled, a portion of `j` is moved to fit, 
    //   leaving the remainder in place.
    void RelocateBlock(Fs fs, Node start, Node j, bool fragmentsEnabled) {
        for (var i = start; i != j; i = i.Next) {
            if (i.Value.fileId != -1) {
                // noop
            } else if (i.Value.length == j.Value.length) {
                (i.Value, j.Value) = (j.Value, i.Value);
                return;
            } else if (i.Value.length > j.Value.length) {
                var d = i.Value.length - j.Value.length;
                i.Value = j.Value;
                j.Value = j.Value with { fileId = -1 };
                fs.AddAfter(i, new Block(-1, d));
                return;
            } else if (i.Value.length < j.Value.length && fragmentsEnabled) {
                var d = j.Value.length - i.Value.length;
                i.Value = i.Value with { fileId = j.Value.fileId };
                j.Value = j.Value with { length = d };
                fs.AddAfter(j, new Block(-1, i.Value.length));
            }
        }
    }

    long Checksum(Fs fs) {
        var res = 0L;
        var l = 0;
        for (var i = fs.First; i != null; i = i.Next) {
            for (var k = 0; k < i.Value.length; k++) {
                if (i.Value.fileId != -1) {
                    res += l * i.Value.fileId;
                }
                l++;
            }
        }
        return res;
    }

    Fs Parse(string input) {
        return new Fs(input.Select((ch, i) => new Block(i % 2 == 1 ? -1 : i / 2, ch - '0')));
    }
}
