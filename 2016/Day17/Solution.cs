using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Y2016.Day17;

[ProblemName("Two Steps Forward")]
class Solution : Solver {

    public object PartOne(string input) => Routes(input).First();

    public object PartTwo(string input) => Routes(input).Last().Length;

    IEnumerable<string> Routes(string input) {

        var q = new Queue<(string path, int irow, int icol)>();
        q.Enqueue(("", 0, 0));

        while (q.Any()) {
            var s = q.Dequeue();

            if (s.icol == 3 && s.irow == 3) {
                yield return s.path;
            } else {
                var doors = DoorState(input + s.path);

                if (doors.down && s.irow < 3) {
                    q.Enqueue((s.path + "D", s.irow + 1, s.icol));
                }
                if (doors.up && s.irow > 0) {
                    q.Enqueue((s.path + "U", s.irow - 1, s.icol));
                }
                if (doors.left && s.icol > 0) {
                    q.Enqueue((s.path + "L", s.irow, s.icol - 1));
                }
                if (doors.right && s.icol < 3) {
                    q.Enqueue((s.path + "R", s.irow, s.icol + 1));
                }
            }
        }
    }

    (bool up, bool down, bool left, bool right) DoorState(string st) {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(st));
        var stHash = string.Join("", hash.Select(b => b.ToString("x2")));
        return (stHash[0] > 'a', stHash[1] > 'a', stHash[2] > 'a', stHash[3] > 'a');
    }
}
