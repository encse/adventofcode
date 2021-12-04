using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day04;

[ProblemName("Giant Squid")]
class Solution : Solver {

    public object PartOne(string input) => BoardsInWinningOrder(input).First().point;
    public object PartTwo(string input) => BoardsInWinningOrder(input).Last().point;

    IEnumerable<Board> BoardsInWinningOrder(string input) {

        var lines = input.Split("\n");

        // first line contains the numbers
        var numbers = lines[0].Split(",").Select(int.Parse);

        // followed by an empty line and blocks of 6 lines, each describing a board:
        var boards = new List<Board>();
        for (var i = 2; i < lines.Length; i += 6) {
            boards.Add(new Board(lines[i..(i + 5)]));
        }

        // let's play the game
        foreach (var num in numbers) {
            foreach (var board in boards.ToArray()) {
                board.AddNumber(num);
                if (board.point > 0) {
                    yield return board;
                    boards.Remove(board);
                }
            }
        }
    }
}

record Cell(int number, bool marked = false);

// Let's be ho-ho-ooop this time.
class Board {

    public int point { get; private set;}
    private List<Cell> cells;

    IEnumerable<Cell> CellsInRow(int irow) =>
        from icol in Enumerable.Range(0, 5) select cells[irow * 5 + icol];

    IEnumerable<Cell> CellsInCol(int icol) =>
        from irow in Enumerable.Range(0, 5) select cells[irow * 5 + icol];

    public Board(string[] lines) {

        // split the input into words & read the numbers into cells
        cells = (
            from word in string.Join(" ", lines).Split(" ")
            where word != ""
            select new Cell(int.Parse(word))
        ).ToList();
    }

    public void AddNumber(int number) {

        var icell = cells.FindIndex(cell => cell.number == number);

        if (icell >= 0) {

            // mark the cell
            cells[icell] = cells[icell] with { marked = true };

            // if the board is completed, compute point
            for (var i = 0; i < 5; i++) {
                if (
                    CellsInRow(i).All(cell => cell.marked) ||
                    CellsInCol(i).All(cell => cell.marked)
                ) {

                    var unmarkedNumbers =
                        from cell in cells where !cell.marked select cell.number;

                    point = number * unmarkedNumbers.Sum();
                }
            }
        }
    }
}
