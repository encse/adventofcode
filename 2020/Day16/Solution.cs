using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day16 {

    record Field(string name, Func<int, bool> isValid);
    record Problem(Field[] fields, int[][] tickets);

    [ProblemName("Ticket Translation")]
    class Solution : Solver {

        Field[] FieldCandidates(IEnumerable<Field> fields, params int[] values) =>
            fields.Where(field => values.All(field.isValid)).ToArray();

        public object PartOne(string input) {
            var problem = Parse(input);
            return (
                from ticket in problem.tickets 
                from value in ticket 
                where !FieldCandidates(problem.fields, value).Any() 
                select value
            ).Sum();
        }

        public object PartTwo(string input) {
            var problem = Parse(input);
            var tickets = (
                from ticket in problem.tickets 
                where ticket.All(value => FieldCandidates(problem.fields, value).Any()) 
                select ticket
            ).ToArray();

            // The problem is set up in a way that we can always find a column of values
            // that must belong to single field. 

            var fields = problem.fields.ToHashSet();
            var columns = Enumerable.Range(0, fields.Count).ToHashSet();

            var res = 1L;
            while (columns.Any()) {
                foreach (var column in columns) {
                    var values = (from ticket in tickets select ticket[column]).ToArray();
                    var candidates = FieldCandidates(fields, values);
                    if (candidates.Length == 1) {
                        fields.Remove(candidates[0]);
                        columns.Remove(column);
                        if (candidates[0].name.StartsWith("departure")) {
                            res *= problem.tickets.First()[column];
                        }
                        break;
                    }
                }
            }
            return res;
        }

        Problem Parse(string input) {
            int[] parseNumbers(string line) => (      // to ignore separator:
                from m in Regex.Matches(line, "\\d+") // take the consecutive range of digits
                select int.Parse(m.Value)             // convert them to numbers
            ).ToArray();

            var blocks = (
                from block in input.Split("\n\n")   // blocks are delimited by empty lines
                select block.Split("\n")            // convert blocks to lines
            ).ToArray();
            
            var fields = (
                from line in blocks.First()         // line <- "departure location: 49-920 or 932-950"
                let bounds = parseNumbers(line)     // bounds <- [49, 920, 932, 950]
                select 
                    new Field(
                        line.Split(":")[0],         // "departure location"
                        n => n >= bounds[0] && n <= bounds[1] || n >= bounds[2] && n <= bounds[3]
                    )
            ).ToArray();

            var tickets = (                         
                from block in blocks.Skip(1)        // Combine the second and third groups containing the ticket infos
                let numbers = block.Skip(1)         // skip "your ticket:" and "nearby tickets:"
                from line in numbers                // line <- "337,687,607,98,229,737,512,521,..."
                select parseNumbers(line)           // [337, 687, 607, 98, 229, 737, 512, 521...]
            ).ToArray();

            return new Problem(fields, tickets);
        }
    }
}