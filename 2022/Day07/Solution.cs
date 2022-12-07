using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
class Solution : Solver {

    public object PartOne(string input) {
        return GetDirectorySizes(input).Where(size => size < 100000).Sum();
    }

    public object PartTwo(string input) {
        var directorySizes = GetDirectorySizes(input);
        var freeSpace = 70000000 - directorySizes.Max();
        return directorySizes.Where(size => size + freeSpace >= 30000000).Min();
    }

    record struct Dir(int totalSize, List<Dir> subdirs);

    private List<int> GetDirectorySizes(string input) {
        // parses the input starting with `$ cd foo` until `$ cd ..` or end of input is found
        Dir parseDir(IEnumerator<string> iterator) {
            iterator.MoveNext(); // $ cd foo

            var totalSize = 0;
            var subdirs = new List<Dir> { };
            while (iterator.MoveNext() && !iterator.Current.StartsWith("$ cd ..")) {
                if (iterator.Current.StartsWith("$ cd")) {
                    subdirs.Add(parseDir(iterator));
                    totalSize += subdirs.Last().totalSize;
                } else if (!iterator.Current.StartsWith("dir ")) {
                    totalSize += int.Parse(iterator.Current.Split(" ")[0]);
                }
            }
            return new Dir(totalSize, subdirs);
        }

        // post-order traversal of a directory
        IEnumerable<Dir> flatten(Dir dir) => dir.subdirs.SelectMany(flatten).Append(dir);

        var iterator = input.Split("\n").AsEnumerable<string>().GetEnumerator();
        iterator.MoveNext();
        return flatten(parseDir(iterator)).Select(dir => dir.totalSize).ToList();
    }
}
