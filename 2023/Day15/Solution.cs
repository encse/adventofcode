using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Y2023.Day15;

[ProblemName("Lens Library")]
class Solution : Solver {

    public object PartOne(string input) {
        return input.Split(',').Select(Hash).Sum();
    }
    int Hash(string st) => st.Aggregate(0, (ch, a) => (ch + a) * 17 % 256);

    T Tsto<T>(T t) {
        Console.WriteLine(t);
        return t;
    }
    public object PartTwo(string input) {
        var boxes = new List<Lens>[256];
        for(var i = 0;i<256;i++) {
            boxes[i] = new List<Lens>();
        }

        foreach(var cmd in input.Split(',')) {
            var parts = cmd.Split('-', '=');
            var label = parts[0];
            var box = Hash(label);
            
            if (parts[1] == "") {
                boxes[box] = boxes[box].Where(x => x.label != label).ToList();
            } else  {
                var focalLength = int.Parse(parts[1]);
                var newLens = new Lens(label, focalLength);
                var idx = boxes[box].FindIndex(lens => lens.label == newLens.label);
                if (idx < 0) {
                    boxes[box].Add(newLens);
                } else {
                    boxes[box][idx] = newLens;
                }
            }
        }

        var m = 0L;
        for(var i = 0;i<256;i++) {
            for(var idx =0; idx <  boxes[i].Count;idx++) {
                var power = (i + 1) * boxes[i][idx].focalLength * (idx + 1);
                m += power;
            }
        }
        return m;
    }
}
record Lens(string label, int focalLength);