using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;


using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;


using System.Threading;
using System.Reflection.Emit;

namespace AdventOfCode.Y2018.Day21 {

    class Solution : Solver {

        public string GetName() => "Chronal Conversion";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Run("one", input).First();
        int PartTwo(string input) => Run("two", input).Last();

        IEnumerable<int> Run(string name, string input) {
            var run = Compile<int[], IEnumerable<int[]>>(name, input, new int[]{28});

            var seen = new List<int>();
            foreach(var r in run(new int[] { 0, 0, 0, 0, 0, 0 })){
                if (seen.Contains(r[3])) {
                    break;
                }
                seen.Add(r[3]);
                yield return r[3];
            }
        }

        public Func<A, B> Compile<A, B>(string name, string input, int[] breakpoints) {
            var code = CompileToCSharp(input, breakpoints);
            var tree = SyntaxFactory.ParseSyntaxTree(code);
            var systemRefLocation = typeof(object).GetTypeInfo().Assembly.Location;
            var systemReference = MetadataReference.CreateFromFile(systemRefLocation);
            var compilation = CSharpCompilation.Create(name)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference)
                .AddSyntaxTrees(tree);

            var ms = new MemoryStream();
            EmitResult compilationResult = compilation.Emit(ms);
            if (compilationResult.Success) {
                ms.Seek(0, SeekOrigin.Begin);
                var asm = AssemblyLoadContext.Default.LoadFromStream(ms);
                var m = asm.GetType("RoslynCore.Helper").GetMethod("Run");
                return (Func<A, B>)Delegate.CreateDelegate(typeof(Func<A, B>), null, m);
            } else {
                foreach (Diagnostic codeIssue in compilationResult.Diagnostics) {
                    string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: { codeIssue.Location.GetLineSpan()}, Severity: { codeIssue.Severity}";
                    Console.WriteLine(issue);
                }

                throw new Exception();
            }
        }

        string CompileToCSharp(string input, int[] breakpoints) {
            var ipReg = int.Parse(input.Split("\n").First().Substring("#ip ".Length));
            var srcLines = input.Split("\n").Skip(1).ToArray();

            var compiledStatements = new StringBuilder();

            for (var ip = 0; ip < srcLines.Length; ip++) {
                var line = srcLines[ip];
                var parts = line.Split(";")[0].Trim().Split(" ");
                var stm = parts.Skip(1).Select(int.Parse).ToArray();
                var compiledStm = "";
                switch (parts[0]) {
                    case "addr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] + r[{stm[1]}]"; break;
                    case "addi": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] + {stm[1]}"; break;
                    case "mulr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] * r[{stm[1]}]"; break;
                    case "muli": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] * {stm[1]}"; break;
                    case "banr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] & r[{stm[1]}]"; break;
                    case "bani": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] & {stm[1]}"; break;
                    case "borr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] | r[{stm[1]}]"; break;
                    case "bori": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] | {stm[1]}"; break;
                    case "setr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}]"; break;
                    case "seti": compiledStm = $"r[{stm[2]}] = {stm[0]}"; break;
                    case "gtir": compiledStm = $"r[{stm[2]}] = {stm[0]} > r[{stm[1]}] ? 1 : 0"; break;
                    case "gtri": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] > {stm[1]} ? 1 : 0"; break;
                    case "gtrr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] > r[{stm[1]}] ? 1 : 0"; break;
                    case "eqir": compiledStm = $"r[{stm[2]}] = {stm[0]} == r[{stm[1]}] ? 1 : 0"; break;
                    case "eqri": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] == {stm[1]} ? 1 : 0"; break;
                    case "eqrr": compiledStm = $"r[{stm[2]}] = r[{stm[0]}] == r[{stm[1]}] ? 1 : 0"; break;
                }
                var brk = breakpoints.Contains(ip) ? "yield return r;" : "";
                compiledStatements.AppendLine($"\t\tcase {ip}: {brk} {compiledStm}; break;");
            }

            return $@"
                using System;
                using System.Collections.Generic;
                namespace RoslynCore
                {{
                    public static class Helper
                    {{
                        public static IEnumerable<int[]> Run(int[] r) {{
                            while(true) {{
                                switch (r[{ipReg}]) {{
                                    {compiledStatements.ToString()}
                                }}
                                r[{ipReg}]++;
                            }}
                        }}

                    }}
                }}
            ";
        }

    }
}