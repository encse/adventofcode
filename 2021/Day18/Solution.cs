using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day18;

[ProblemName("Snailfish")]
class Solution : Solver {

    public object PartOne(string input) {
        List<Token> tokens = null;
        foreach (var line in input.Split("\n")) {
            var tokensB = Tokenize(line);
            tokens = tokens == null ? tokensB : Sum(tokens, tokensB);
            // Console.WriteLine(ToString(tokens));
        }

        return Magnitude(tokens);
    }

    string ToString(List<Token> tokens) {
        var res = "";
        foreach (var token in tokens) {
            if (token.kind == Kind.L) {
                res += "[";
            } else if (token.kind == Kind.R) {
                res += "]";
            } else if (token.kind == Kind.Num) {
                res += token.value + " ";
            }
        }
        return res;
    }

    public object PartTwo(string input) {
        List<Token>[] tokens = input.Split("\n").Select(Tokenize).ToArray();
        
        return (
            from i in Enumerable.Range(0, tokens.Length)
            from j in Enumerable.Range(0, tokens.Length)
            where i != j
            select Magnitude(Sum(tokens[i], tokens[j]))
        ).Max();
    }


    long Magnitude(List<Token> tokens) {

        var i = 0;
        Token getToken() {
            return tokens[i++];
        }

        Node parseNode() {
            var token = getToken();
            if (token.kind == Kind.Num) {
                return new Node(token, null, null);
            } else {
                var left = parseNode();
                var right = parseNode();
                getToken();
                return new Node(token, left, right);
            }
        }
        var node = parseNode();

        long magni(Node node) {
            if (node.left == null) {
                return node.token.value;
            } else {
                return 3 * magni(node.left) + 2 * magni(node.right);
            }
        }
        return magni(node);
    }

    List<Token> Sum(List<Token> tokensA, List<Token> tokensB) {
        // Console.WriteLine("sum     " + ToString(tokensA) + " + " + ToString(tokensB));
        var tokens = new List<Token>();
        tokens.Add(new Token(Kind.L, 0));
        tokens.AddRange(tokensA);
        tokens.AddRange(tokensB);
        tokens.Add(new Token(Kind.R, 0));
        return Reduce(tokens);
    }

    List<Token> Reduce(List<Token> tokens) {
        // Console.WriteLine("reduce  "+ ToString(tokens));
        while (true) {
            bool cont;
            (tokens, cont) = Explode(tokens);
            if (cont) {
                // Console.WriteLine("explode " + ToString(tokens));
                continue;
            }
            (tokens, cont) = Split(tokens);
            if (cont) {
                // Console.WriteLine("split   " + ToString(tokens));
                continue;
            }
            break;
        }
        // Console.WriteLine("reduced " + ToString(tokens));
        return tokens;
    }

    void Assert(bool cond) {
        if (!cond) { throw new Exception(""); };
    }

    (List<Token>, bool) Explode(List<Token> tokens) {
        var depth = 0;
        for (var i = 0; i < tokens.Count; i++) {
            if (tokens[i].kind == Kind.L) {
                depth++;
                if (depth == 5) {
                    // akkor redukalni kell
                    Assert(tokens[i + 1].kind == Kind.Num);
                    Assert(tokens[i + 2].kind == Kind.Num);

                    for (var j = i - 1; j >= 0; j--) {
                        if (tokens[j].kind == Kind.Num) {
                            tokens[j] = tokens[j] with { value = tokens[j].value + tokens[i + 1].value };
                            break;
                        }
                    }
                    for (var j = i + 3; j < tokens.Count; j++) {
                        if (tokens[j].kind == Kind.Num) {
                            tokens[j] = tokens[j] with { value = tokens[j].value + tokens[i + 2].value };
                            break;
                        }
                    }

                    tokens.RemoveRange(i, 4);
                    tokens.Insert(i, new Token(Kind.Num, 0));
                    return (tokens, true);
                }
            } else if (tokens[i].kind == Kind.R) {
                depth--;
            }
        }
        return (tokens, false);
    }

    (List<Token>, bool) Split(List<Token> tokens) {
        for (var i = 0; i < tokens.Count; i++) {
            if (tokens[i].value >= 10) {
                var v = tokens[i].value;
                tokens.RemoveRange(i, 1);
                tokens.InsertRange(i, new[]{
                     new Token(Kind.L, 0),
                     new Token(Kind.Num, v/2),
                     new Token(Kind.Num, v-v/2),
                     new Token(Kind.R, 0)
                });
                return (tokens, true);
            }
        }
        return (tokens, false);
    }

    List<Token> Tokenize(string st) {
        var res = new List<Token>();
        var n = "";
        foreach (var ch in st) {
            if (ch >= '0' && ch <= '9') {
                n += ch;
            } else {
                if (n != "") {
                    res.Add(new Token(Kind.Num, int.Parse(n)));
                    n = "";
                }
                if (ch == '[') {
                    res.Add(new Token(Kind.L, 0));
                } else if (ch == ']') {
                    res.Add(new Token(Kind.R, 0));
                }
            }
        }
        if (n != "") {
            res.Add(new Token(Kind.Num, int.Parse(n)));
            n = "";
        }
        return res;
    }

}
enum Kind {
    L,
    R,
    Num
}
record Token(Kind kind, int value);
record Node(Token token, Node left, Node right);