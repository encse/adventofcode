using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;


using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace AdventOfCode.Y2018.Day21 {

    [ProblemName("Chronal Conversion")]
    class Solution : Solver {

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
                var compiledStm = parts[0] switch {
                    "addr" => $"r[{stm[2]}] = r[{stm[0]}] + r[{stm[1]}]",
                    "addi" => $"r[{stm[2]}] = r[{stm[0]}] + {stm[1]}",
                    "mulr" => $"r[{stm[2]}] = r[{stm[0]}] * r[{stm[1]}]",
                    "muli" => $"r[{stm[2]}] = r[{stm[0]}] * {stm[1]}",
                    "banr" => $"r[{stm[2]}] = r[{stm[0]}] & r[{stm[1]}]",
                    "bani" => $"r[{stm[2]}] = r[{stm[0]}] & {stm[1]}",
                    "borr" => $"r[{stm[2]}] = r[{stm[0]}] | r[{stm[1]}]",
                    "bori" => $"r[{stm[2]}] = r[{stm[0]}] | {stm[1]}",
                    "setr" => $"r[{stm[2]}] = r[{stm[0]}]",
                    "seti" => $"r[{stm[2]}] = {stm[0]}",
                    "gtir" => $"r[{stm[2]}] = {stm[0]} > r[{stm[1]}] ? 1 : 0",
                    "gtri" => $"r[{stm[2]}] = r[{stm[0]}] > {stm[1]} ? 1 : 0",
                    "gtrr" => $"r[{stm[2]}] = r[{stm[0]}] > r[{stm[1]}] ? 1 : 0",
                    "eqir" => $"r[{stm[2]}] = {stm[0]} == r[{stm[1]}] ? 1 : 0",
                    "eqri" => $"r[{stm[2]}] = r[{stm[0]}] == {stm[1]} ? 1 : 0",
                    "eqrr" => $"r[{stm[2]}] = r[{stm[0]}] == r[{stm[1]}] ? 1 : 0",
                    _ => throw new ArgumentException()
                };
                var brk = breakpoints.Contains(ip) ? "yield return r;" : "";
                compiledStatements.AppendLine($"\t\tcase {ip}: {brk} {compiledStm}; r[{ipReg}]++; break;");
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
                            }}
                        }}
                    }}
                }}
            ";
        }

    }
}