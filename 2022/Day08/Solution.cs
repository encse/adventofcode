using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day08;

[ProblemName("Treetop Tree House")]
class Solution : Solver {

    public object PartOne(string input) {
        var grid = Parse(input);
        return grid.Positions().Count(grid.IsVisible);
    }

    public object PartTwo(string input) {
        var grid = Parse(input);
        return grid.Positions().Select(grid.GetScenicScore).Max();
    }

    Grid Parse(string input) {
        var items = input.Split("\n");
        var (ccol, crow) = (items[0].Length, items.Length);
        return new Grid(items, crow, ccol);
    }
}

record Postion(int irow, int icol);
record Direction(int drow, int dcol);
record Grid(string[] items, int crow, int ccol) {

    static Direction Left = new Direction(0, -1);
    static Direction Right = new Direction(0, 1);
    static Direction Up = new Direction(-1, 0);
    static Direction Down = new Direction(1, 0);

    public bool IsVisible(Postion pos) =>
        IsVisible(pos, Left) || IsVisible(pos, Right) || IsVisible(pos, Up) || IsVisible(pos, Down);

    public int GetScenicScore(Postion pos) =>
        ViewDistance(pos, Left) * ViewDistance(pos, Right) * ViewDistance(pos, Up) * ViewDistance(pos, Down);

    public IEnumerable<Postion> Positions() =>
        from irow in Enumerable.Range(0, crow)
        from icol in Enumerable.Range(0, ccol)
        select new Postion(irow, icol);

    bool IsVisible(Postion pos, Direction dir) => 
        SmallerTreeCount(pos, dir) == TreesInDirection(pos, dir).Count();
        
    int ViewDistance(Postion pos, Direction dir) => 
        SmallerTreeCount(pos, dir) + (IsVisible(pos, dir) ? 0 : 1);

    int SmallerTreeCount(Postion pos, Direction dir) =>
        TreesInDirection(pos, dir).TakeWhile(tree => tree < items[pos.irow][pos.icol]).Count();

    IEnumerable<int> TreesInDirection(Postion pos, Direction dir) {
        for (var first = true; 
            pos.irow >= 0 && pos.irow < crow && pos.icol >= 0 && pos.icol < ccol; 
            pos = new Postion(pos.irow + dir.drow, pos.icol + dir.dcol)
        ) {
            if (first) {
                first = false;
            } else {
                yield return items[pos.irow][pos.icol];
            }
        }
    }
}
