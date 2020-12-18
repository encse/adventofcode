using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day18 {

    record Node;
    record Add(Node left, Node right) : Node{
        public override string ToString(){
            return "(" + left + ") + (" + right + ")";
        }
    }
    record Mul(Node left, Node right) : Node{
        public override string ToString(){
            return "(" + left + ") * (" + right + ")";
        }
    }
    record Num(long value) : Node {
        public override string ToString(){
            return value.ToString();
        } 
    }

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) {
            var sum = 0L;
            foreach (var line in input.Split("\n")) {
                sum += Evaluate(line);
            }
            return sum;
        }

        public object PartTwo(string input) {
            var sum = 0L;
            foreach (var line in input.Split("\n")) {
                sum += Eval(Parse2(line));
            }
            return sum;

        }

        long Eval(Node node) {
            return node switch {
                Add add => Eval(add.left) + Eval(add.right),
                Mul mul => Eval(mul.left) * Eval(mul.right),
                Num num => num.value,
                _ => throw new Exception()
            };
        }

        long Evaluate(string input) {
            var stack = new Stack<object>();
            Evaluate1(input, stack);
            return (long)stack.Pop();
        }

        void Evaluate1(string input, Stack<object> stack) {
            if (input.Length == 0) {
                return;
            } else if (Regex.Match(input, "^[0-9]+").Success) {
                var m = Regex.Match(input, "^[0-9]+").Value;
                var v = long.Parse(m);
                if (stack.Any()) {
                    var op = stack.Pop() as string;
                    var arg = (long)stack.Pop();
                    if (op == "+")
                        v += arg;
                    else
                        v *= arg;
                }
                stack.Push(v);
                Evaluate1(input.Substring(m.Length), stack);
            } else if (input.StartsWith(" ")) {
                Evaluate1(input.Substring(1), stack);
            } else if (input[0] == '+') {
                stack.Push("+");
                Evaluate1(input.Substring(1), stack);
            } else if (input[0] == '*') {
                stack.Push("*");
                Evaluate1(input.Substring(1), stack);
            } else if (input[0] == '(') {
                stack.Push(0L);
                stack.Push("+");
                Evaluate1(input.Substring(1), stack);
            } else if (input[0] == ')') {
                var v = (long)stack.Pop();
                if (stack.Any()) {
                    var op = stack.Pop() as string;
                    var arg = (long)stack.Pop();
                    if (op == "+")
                        v += arg;
                    else
                        v *= arg;
                }
                stack.Push(v);
                Evaluate1(input.Substring(1), stack);
            } else {
                throw new Exception();
            }
        }

        Node Parse2(string input) {
            bool accept(string st) {
                if (input.StartsWith(st)) {
                    input = input.Substring(st.Length);
                    return true;
                }
                return false;
            }

            Node mul() {
                var res = add();
                while (accept("*")) {
                    res = new Mul(res, add());
                }
                return res;
            }
            Node add() {
                var res = primary();
                while (accept("+")) {
                    res = new Add(res, add());
                }
                return res;
            }
            Node primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(input, "^[0-9]+").Value;
                    input = input.Substring(m.Length);
                    return new Num(long.Parse(m));
                }
            }

            Node expr() {
                return mul();
            }

            input = input.Replace(" ", "");
            var res = expr();
            Console.WriteLine(res);
            return res;
        }
        // Node Parse(string input) {
        //     // input = input.Replace(" ", "");
        //     // var ich = 0;
        //     // bool plus(){

        //     //     if(input[ich] == '+'){
        //     //         ich++;
        //     //         return true;
        //     //     }
        //     //     return false;
        //     // }

        //     // bool mul(){
        //     //     if(input[ich] == '*'){
        //     //         ich++;
        //     //         return true;
        //     //     }
        //     //     return false;
        //     // }

        //     // bool num(){
        //     //     if(input[ich] == '*'){
        //     //         ich++;
        //     //         return true;
        //     //     }
        //     //     return false;
        //     // }

        //     // bool expr(){

        //     // }
        //     // bool addExpr(){

        //     // }

        //     // bool mulExpr(){

        //     // }

        //     // bool paren(){
        //     //     if(input[ich] == '('){
        //     //         ich++;
        //     //         return true;
        //     //     }
        //     // }


        //     Node rec(string input, Node acc) {
        //         if(input == "") {
        //             return acc;
        //         } else if (Regex.Match(input, "^[0-9]+").Success) {
        //             var m = Regex.Match(input, "^[0-9]+").Value;
        //             return rec(input.Substring(m.Length), new Num(long.Parse(m)));
        //         } else if (input.StartsWith(" ")) {
        //             return rec(input.Substring(1), acc);
        //         } else if (input[0] == '+') {
        //             return new Add(acc, rec(input.Substring(1), null));
        //         } else if (input[0] == '*') {
        //             return new Mul(acc, rec(input.Substring(1), null));
        //         } else if (input[0] == '(') {
        //             return rec(input.Substring(1), null);
        //         } else if (input[0] == ')') {
        //            return acc;
        //         } else {
        //             throw new Exception();
        //         }
        //     }
        //     return rec(input, null);
        // }

        void Evaluate2(string input, Stack<object> stack) {

            void Mul() {
                while (stack.Count > 1) {
                    var v = (long)stack.Pop();
                    if (stack.Peek() as string == "*") {
                        stack.Pop();
                        v *= (long)stack.Pop();
                        stack.Push(v);
                    } else {
                        stack.Push(v);
                        break;
                    }
                }

                while (stack.Count > 1) {
                    var v = (long)stack.Pop();
                    if (stack.Peek() as string == "+") {
                        stack.Pop();
                        v += (long)stack.Pop();
                        stack.Push(v);
                    } else {
                        stack.Push(v);
                        break;
                    }
                }
            }

            if (input.Length == 0) {
                Mul();
            } else if (Regex.Match(input, "^[0-9]+").Success) {
                var m = Regex.Match(input, "^[0-9]+").Value;
                var v = long.Parse(m);
                if (stack.Any()) {
                    if (stack.Peek() as string == "+") {
                        var op = stack.Pop() as string;
                        var arg = (long)stack.Pop();
                        v += arg;
                    }
                }
                stack.Push(v);
                Evaluate2(input.Substring(m.Length), stack);
            } else if (input.StartsWith(" ")) {
                Evaluate2(input.Substring(1), stack);
            } else if (input[0] == '+') {
                stack.Push("+");
                Evaluate2(input.Substring(1), stack);
            } else if (input[0] == '*') {
                stack.Push("*");
                Evaluate2(input.Substring(1), stack);
            } else if (input[0] == '(') {
                stack.Push(0L);
                stack.Push("+");
                Evaluate2(input.Substring(1), stack);
            } else if (input[0] == ')') {
                Mul();
                Evaluate2(input.Substring(1), stack);
            } else {
                throw new Exception();
            }
        }
    }
}