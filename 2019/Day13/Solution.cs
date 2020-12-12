using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day13 {

    [ProblemName("Care Package")]
    class Solution : Solver {

        public object PartOne(string input) {
            var icm = new IntCodeMachine(input);
            var output = icm.Run();
            var chunks = Chunk(output, 3);
            return chunks.Count(x => x[2] == 2);
        }

        public object PartTwo(string input) {
            var icm = new IntCodeMachine(input);
            icm.memory[0] = 2;
            var score = 0;
            var icolBall = -1;
            var icolPaddle = -1;
            var dir = 0;
            while (true) {
                var output = icm.Run(dir);
                var chunks = Chunk(output, 3);
                foreach (var chunk in chunks) {
                    var (icol, irow, block) = (chunk[0], chunk[1], chunk[2]);
                    if ((icol, irow) == (-1, 0)) {
                        score = (int)block;
                    } if (block == 3) {
                        icolPaddle = (int)icol;
                    } else if (block == 4) {
                        icolBall = (int)icol;
                    }
                }

                if (icm.Halted()) {
                    break;
                }

                dir = 
                    icolBall < icolPaddle ? -1 :
                    icolBall > icolPaddle ? 1 :
                    0;
               
            }
            return score;
        }

        public T[][] Chunk<T>(IEnumerable<T> source, int chunksize) {
            var res = new List<T[]>();
            while (source.Any()) {
                res.Add(source.Take(chunksize).ToArray());
                source = source.Skip(chunksize);
            }
            return res.ToArray();
        }
    }

}