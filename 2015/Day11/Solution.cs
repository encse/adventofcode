using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2015.Day11;

[ProblemName("Corporate Policy")]
class Solution : Solver {

    public object PartOne(string input) => Passwords(input).First();
    public object PartTwo(string input) => Passwords(input).Skip(1).First();

    IEnumerable<string> Passwords(string pwd) =>
        from word in Words(pwd) 
        let straigth = Enumerable.Range(0, word.Length - 2).Any(i => word[i] == word[i + 1] - 1 && word[i] == word[i + 2] - 2)
        let reserved = "iol".Any(ch => word.Contains(ch))
        let pairs = Enumerable.Range(0, word.Length - 1).Select(i => word.Substring(i, 2)).Where(sword => sword[0] == sword[1]).Distinct()
        where straigth && !reserved && pairs.Count() > 1
        select word;
    
    IEnumerable<string> Words(string word) {
        while (true) {
            var sb = new StringBuilder();
            for (var i = word.Length - 1; i >= 0; i--) {
                var ch = word[i] + 1;
                if (ch > 'z') {
                    ch = 'a';
                    sb.Insert(0, (char)ch);
                } else {
                    sb.Insert(0, (char)ch);
                    sb.Insert(0, word.Substring(0, i));
                    i = 0;
                }
            }
            word = sb.ToString();
            yield return word;
        }
    }
}
