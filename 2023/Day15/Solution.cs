using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day15;
using Boxes = List<Lens>[];

[ProblemName("Lens Library")]
class Solution : Solver {

    public object PartOne(string input) => input.Split(',').Select(Hash).Sum();

    // "funcionally imperative of imperatively functional", only for 🎄
    public object PartTwo(string input) =>
        ParseSteps(input).Aggregate(MakeBoxes(256), UpdateBoxes, GetFocusingPower);

    Boxes UpdateBoxes(Boxes boxes, Step step) {
        var box = boxes[Hash(step.label)];
        var ilens = box.FindIndex(lens => lens.label == step.label);

        if (!step.focalLength.HasValue && ilens >= 0) {
            box.RemoveAt(ilens);
        } else if (step.focalLength.HasValue && ilens >= 0) {
            box[ilens] = new Lens(step.label, step.focalLength.Value);
        } else if (step.focalLength.HasValue && ilens < 0) {
            box.Add(new Lens(step.label, step.focalLength.Value));
        }
        return boxes;
    }

    IEnumerable<Step> ParseSteps(string input) =>
        from item in input.Split(',')
        let parts = item.Split('-', '=')
        select new Step(parts[0], parts[1] == "" ? null : int.Parse(parts[1]));

    Boxes MakeBoxes(int count) =>
        Enumerable.Range(0, count).Select(_ => new List<Lens>()).ToArray();

    int GetFocusingPower(Boxes boxes) => (
        from ibox in Enumerable.Range(0, boxes.Length)
        from ilens in Enumerable.Range(0, boxes[ibox].Count)
        select (ibox + 1) * (ilens + 1) * boxes[ibox][ilens].focalLength
    ).Sum();

    int Hash(string st) => st.Aggregate(0, (ch, a) => (ch + a) * 17 % 256);
}

record Lens(string label, int focalLength);
record Step(string label, int? focalLength);
