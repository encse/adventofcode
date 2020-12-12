using System.Linq;

namespace AdventOfCode.Y2017.Day19 {

    [ProblemName("A Series of Tubes")]
    class Solution : Solver {

        public object PartOne(string input) => FollowPath(input).msg;

        public object PartTwo(string input) => FollowPath(input).steps;

        (string msg, int steps) FollowPath(string input){
            var map = input.Split('\n');
            var (ccol, crow) = (map[0].Length, map.Length);
            var (icol, irow) = (map[0].IndexOf('|'), 0);
            var (dcol, drow) = (0, 1);

            var msg = "";
            var steps = 0;

            while (true) {
                irow += drow;
                icol += dcol;
                steps++;

                if (icol < 0 || icol >= ccol || irow < 0 || irow >= crow || map[irow][icol] == ' ') {
                    break;
                }

                switch (map[irow][icol]) {
                    case '+':
                        (dcol, drow) = (
                                from q in new[] { (drow: dcol, dcol: -drow), (drow: -dcol, dcol: drow)}
                                let icolT = icol + q.dcol
                                let irowT = irow + q.drow
                                where icolT >= 0 && icolT < ccol && irowT >= 0 && irowT < crow && map[irowT][icolT] != ' '
                                select (q.dcol, q.drow)
                            ).Single();
                        break;
                    case char ch when (ch >= 'A' && ch <= 'Z'):
                        msg += ch;
                        break;
                }
            }
            return (msg, steps);
        }
    }
}