using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Y2016.Day05;

[ProblemName("How About a Nice Game of Chess?")]
class Solution : Solver
{

    public object PartOne(string input)
    {
       return string.Join("", Hashes(input).Select(hash => hash[5]).Take(8));
    }

    public object PartTwo(string input)
    {
        var res = new char[8];
        var found = 0;
        foreach (var hash in Hashes(input))
        {
            var idx = hash[5] - '0';
            if (0 <= idx && idx < 8 && res[idx] == 0)
            {
                res[idx] = hash[6];
                found++;
                if (found == 8) { 
                    break; 
                }
            }
           
        }
        return string.Join("", res);
    }

    public IEnumerable<string> Hashes(string input)
    {

        for (var i = 0; i < int.MaxValue; i++)
        {
            var q = new ConcurrentQueue<(int i, string hash)>();

            Parallel.ForEach(
                NumbersFrom(i),
                () => MD5.Create(),
                (i, state, md5) =>
                {
                    var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
                    var hashString = string.Join("", hash.Select(x => x.ToString("x2")));

                    if (hashString.StartsWith("00000"))
                    {
                        q.Enqueue((i, hashString));
                        state.Stop();
                    }
                    return md5;
                },
                (_) => { }
            );
            var item = q.OrderBy(x => x.i).First();
            i = item.i;
            yield return item.hash;
        }
    }

    IEnumerable<int> NumbersFrom(int i)
    {
        for (;;) yield return i++;
    }
}
