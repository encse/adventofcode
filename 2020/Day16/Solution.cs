using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day16;

record Field(string name, Func<int, bool> isValid);
record Problem(Field[] fields, int[][] tickets);

[ProblemName("Ticket Translation")]
class Solution : Solver {

    Field[] FieldCandidates(IEnumerable<Field> fields, params int[] values) =>
        fields.Where(field => values.All(field.isValid)).ToArray();

    public object PartOne(string input) {
        var problem = Parse(input);
        // add the values that cannot be associated with any of the fields
        return (
            from ticket in problem.tickets 
            from value in ticket 
            where !FieldCandidates(problem.fields, value).Any() 
            select value
        ).Sum();
    }

    public object PartTwo(string input) {

        var problem = Parse(input);
        // keep valid tickets only
        var tickets = (
            from ticket in problem.tickets 
            where ticket.All(value => FieldCandidates(problem.fields, value).Any()) 
            select ticket
        ).ToArray();

        // The problem is set up in a way that we can always find a column
        // that has just one field-candidate left.

        var fields = problem.fields.ToHashSet();
        var columns = Enumerable.Range(0, fields.Count).ToHashSet();

        var res = 1L;
        while (columns.Any()) {
            foreach (var column in columns) {
                var valuesInColumn = (from ticket in tickets select ticket[column]).ToArray();
                var candidates = FieldCandidates(fields, valuesInColumn);
                if (candidates.Length == 1) {
                    var field = candidates.Single();
                    fields.Remove(field);
                    columns.Remove(column);
                    if (field.name.StartsWith("departure")) {
                        res *= valuesInColumn.First();
                    }
                    break;
                }
            }
        }
        return res;
    }

    Problem Parse(string input) {
        int[] parseNumbers(string line) => (      
            from m in Regex.Matches(line, "\\d+") // take the consecutive ranges of digits
            select int.Parse(m.Value)             // convert them to numbers
        ).ToArray();

        var blocks = (
            from block in input.Split("\n\n")     // blocks are delimited by empty lines
            select block.Split("\n")              // convert them to lines
        ).ToArray();
        
        var fields = (                          
            from line in blocks.First()           // line <- ["departure location: 49-920 or 932-950", ...]
            let bounds = parseNumbers(line)       // bounds = [49, 920, 932, 950]
            select 
                new Field(
                    line.Split(":")[0],           // "departure location"
                    n => 
                        n >= bounds[0] && n <= bounds[1] || 
                        n >= bounds[2] && n <= bounds[3]
                )
        ).ToArray();

        var tickets = (                         
            from block in blocks.Skip(1)          // ticket information is in the second and third blocks 
            let numbers = block.Skip(1)           // skip "your ticket:" / "nearby tickets:"
            from line in numbers                  // line <- ["337,687,...", "223,323,...", ...]
            select parseNumbers(line)             // [337, 687, 607]
        ).ToArray();

        return new Problem(fields, tickets);
    }
}
