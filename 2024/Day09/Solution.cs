namespace AdventOfCode.Y2024.Day09;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.AccessControl;

[ProblemName("Disk Fragmenter")]
class Solution : Solver {

    // WIP
    
    public object PartOne(string input) {
        return Defragment(Flatten(Parse(input)).ToList());
    }
    // public object PartOne(string input) {
    //     var extracted = Extract(input);
    //     var i = 0;
    //     var j = extracted.Length - 1;
    //     while (i < j) {
    //         if (extracted[j] == -1) {
    //             j--;
    //         } else if (extracted[i] != -1) {
    //             i++;
    //         } else {
    //             (extracted[i], extracted[j]) = (extracted[j], extracted[i]);
    //             j--;
    //             i++;
    //         }
    //     }
    //     var res = 0L;
    //     for (i = 0; i < extracted.Length; i++) {
    //         if (extracted[i] != -1) {
    //             res += i * extracted[i];
    //         }
    //     }
    //     return res;
    // }

    public object PartTwo(string input) {
        return Defragment(Parse(input));
    }

    public long Defragment(List<Part> fs) {
        var j = fs.Count - 1;
        while (j > 0) {
            if (fs[j].inode == -1) {
                j--;
            } else {
                for (var i = 0; i <= j; i++) {
                    if (i == j){
                        j--;
                        break;
                    } else if (fs[i].inode == -1 && fs[i].length == fs[j].length) {
                        (fs[i], fs[j]) = (fs[j], fs[i]);
                        j--;
                        break;
                    } else if (fs[i].inode == -1 && fs[i].length > fs[j].length) {
                        fs.Insert(i+1, new Part(-1, fs[i].length - fs[j].length));
                        j++;
                        (fs[i], fs[j]) = (fs[j], new Part(-1, fs[j].length));
                        j--;
                        break;
                    }
                }
            }
        }

        var q = Flatten(fs).ToArray();
        var res = 0L;
        for (var i = 0; i < q.Length; i++) {
            if (q[i].inode != -1) {
                res += i * q[i].inode;
            }
        }
        return res;
    }

    IEnumerable<Part> Flatten(List<Part> parts) {
        for(var i = 0;i<parts.Count;i++){
            for(var j=0;j<parts[i].length;j++){
                yield return new Part(parts[i].inode, 1);
            }
        }
    }

    IEnumerable<int> GetDescriptors() {
        for (var i = 0; ; i++) {
            yield return i;
            yield return -1;
        }
    }

    int[] Extract(string input) =>
        Enumerable.Zip(GetDescriptors(), input)
            .SelectMany(p => Enumerable.Repeat(p.First, p.Second - '0'))
            .ToArray();

    List<Part> Parse(string input) {
        return input.Select((ch, i) => new Part(i % 2 == 1 ? -1 : i / 2, ch - '0')).ToList();
    }
}

record struct Part(int inode, int length) {

}