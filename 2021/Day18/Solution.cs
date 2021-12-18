using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day18;

[ProblemName("Snailfish")]
class Solution : Solver {

    // WARNING: What follows is obscure nonsense.
    //
    //     .-""-.
    //    /,..___\
    //   () {_____}
    //     (/-@-@-\)
    //     {`-=^=-'}
    //     {  `-'  } Max
    //      {     }
    //       `---'

    public object PartOne(string input) {
        // sum up all the 'numbers' in the input
        return input.Split("\n").Select(ParseNumber).Aggregate(
            new Number(), 
            (acc, number) => !acc.Any() ? number : Sum(acc, number), 
            Magnitude
        );
    }

    public object PartTwo(string input) {
        // get the highest magnitude resulted from adding any two 'numbers' in the input:
        var numbers = input.Split("\n").Select(ParseNumber).ToArray();
        return (
            from i in Enumerable.Range(0, numbers.Length)
            from j in Enumerable.Range(0, numbers.Length)
            where i != j
            select Magnitude(Sum(numbers[i], numbers[j]))
        ).Max();
    }

    long Magnitude(Number number) {
        var itoken = 0; // we will process the number tokenwise

        long computeRecursive() {
            var token = number[itoken++];
            if (token.kind == TokenKind.Digit) {
                // just the number
                return token.value;
            } else {
                // take left and right side of the pair
                var left = computeRecursive();
                var right = computeRecursive();
                itoken++; // don't forget to eat the closing parenthesis
                return  3 * left + 2 * right;
            }
        }

        return computeRecursive();
    }


    Number Sum(Number numberA, Number numberB) {
        // just wrap A and B in a new 'number' and reduce:

        var numbers = new Number();
        numbers.Add(new Token(TokenKind.LeftParenthesis));
        numbers.AddRange(numberA);
        numbers.AddRange(numberB);
        numbers.Add(new Token(TokenKind.RightParenthesis));

        return Reduce(numbers);
    }

    Number Reduce(Number number) {
        while (Explode(number) || Split(number)) {
            ; // repeat until we cannot explod or split anymore
        }
        return number;
    }

    bool Explode(Number number) {
        // exploding means we need to find the first pair in the number 
        // that is embedded in 4 other pairs and get rid of it:
        var depth = 0;
        for (var i = 0; i < number.Count; i++) {
            if (number[i].kind == TokenKind.LeftParenthesis) {
                depth++;
                if (depth == 5) {
                    // we are deep enough, let's to the reduce part

                    // find the digit to the left (if any) and increase:
                    for (var j = i - 1; j >= 0; j--) {
                        if (number[j].kind == TokenKind.Digit) {
                            number[j] = number[j] with { value = number[j].value + number[i + 1].value };
                            break;
                        }
                    }

                    // find the digit to the right (if any) and increase:
                    for (var j = i + 3; j < number.Count; j++) {
                        if (number[j].kind == TokenKind.Digit) {
                            number[j] = number[j] with { value = number[j].value + number[i + 2].value };
                            break;
                        }
                    }

                    // replace [a b] with 0:
                    number.RemoveRange(i, 4);
                    number.Insert(i, new Token(TokenKind.Digit, 0));

                    // successful reduce:
                    return true;
                }
            } else if (number[i].kind == TokenKind.RightParenthesis) {
                depth--;
            }
        }

        // couldn't reduce:
        return false;
    }

    bool Split(Number number) {

        // spliting means we neeed to find a token with a high value and make a pair out of it:
        for (var i = 0; i < number.Count; i++) {
            if (number[i].value >= 10) {

                var v = number[i].value;
                number.RemoveRange(i, 1);
                number.InsertRange(i, new[]{
                     new Token(TokenKind.LeftParenthesis),
                     new Token(TokenKind.Digit, v/2),
                     new Token(TokenKind.Digit, v-v/2),
                     new Token(TokenKind.RightParenthesis)
                });

                // successful split:
                return true;
            }
        }
         // couldn't split:
        return false;
    }

    // tokenize the input to a list of '[' ']' and digit tokens
    Number ParseNumber(string st) {
        var res = new Number();
        var n = "";
        foreach (var ch in st) {
            if (ch >= '0' && ch <= '9') {
                n += ch;
            } else {
                if (n != "") {
                    res.Add(new Token(TokenKind.Digit, int.Parse(n)));
                    n = "";
                }
                if (ch == '[') {
                    res.Add(new Token(TokenKind.LeftParenthesis));
                } else if (ch == ']') {
                    res.Add(new Token(TokenKind.RightParenthesis));
                }
            }
        }
        if (n != "") {
            res.Add(new Token(TokenKind.Digit, int.Parse(n)));
            n = "";
        }
        return res;
    }

}

// we will work with a list of tokens directly
enum TokenKind {
    LeftParenthesis,
    RightParenthesis,
    Digit
}
record Token(TokenKind kind, int value = 0);

class Number : List<Token>{};
