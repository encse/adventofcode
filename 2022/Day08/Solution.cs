using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day08;

[ProblemName("Treetop Tree House")]
class Solution : Solver {

    static Direction Left = new Direction(0, -1);
    static Direction Right = new Direction(0, 1);
    static Direction Up = new Direction(-1, 0);
    static Direction Down = new Direction(1, 0);

    public object PartOne(string input) {
        var forest = Parse(input);

        return forest.Trees().Count(tree =>
            forest.IsTallest(tree, Left) || forest.IsTallest(tree, Right) ||
            forest.IsTallest(tree, Up) || forest.IsTallest(tree, Down)
        );
    }

    public object PartTwo(string input) {
        var forest = Parse(input);

        return forest.Trees().Select(tree =>
            forest.ViewDistance(tree, Left) * forest.ViewDistance(tree, Right) *
            forest.ViewDistance(tree, Up) * forest.ViewDistance(tree, Down)
        ).Max();
    }

    Forest Parse(string input) {
        var items = input.Split("\n");
        var (ccol, crow) = (items[0].Length, items.Length);
        return new Forest(items, crow, ccol);
    }
}

record Direction(int drow, int dcol);
record Tree(int height, int irow, int icol);
record Forest(string[] items, int crow, int ccol) {

    public IEnumerable<Tree> Trees() =>
        from irow in Enumerable.Range(0, crow)
        from icol in Enumerable.Range(0, ccol)
        select new Tree(items[irow][icol], irow, icol);

    public bool IsTallest(Tree tree, Direction dir) =>
        SmallerTrees(tree, dir).Count() == TreesInDirection(tree, dir).Count();

    public int ViewDistance(Tree tree, Direction dir) =>
        IsTallest(tree, dir) ? TreesInDirection(tree, dir).Count() : SmallerTrees(tree, dir).Count() + 1;

    IEnumerable<Tree> SmallerTrees(Tree tree, Direction dir) =>
        TreesInDirection(tree, dir).TakeWhile(treeT => treeT.height < tree.height);

    IEnumerable<Tree> TreesInDirection(Tree tree, Direction dir) {
        var (first, irow, icol) = (true, tree.irow, tree.icol);
        while (irow >= 0 && irow < crow && icol >= 0 && icol < ccol) {
            if (!first) {
                yield return new Tree(height: items[irow][icol], irow: irow, icol: icol);
            }
            (first, irow, icol) = (false, irow + dir.drow, icol + dir.dcol);
        }
    }
}
