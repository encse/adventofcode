using System;
using System.IO;

namespace AdventOfCode2017
{
    interface Solver
    {
        void Solve(string input);
    }

    class Runner
    {
        static void Main()
        {
            RunSolver(new Day06.Solution());
        }

        static void RunSolver(Solver solver)
        {
            var name = solver.GetType().FullName.Split('.')[1].ToLower();
            Console.WriteLine($"Solving {name}");
            foreach (var file in Directory.EnumerateFiles(name))
            {
                if (file.EndsWith(".in"))
                {
                    solver.Solve(File.ReadAllText(file));
                }
            }
        }
    }
}