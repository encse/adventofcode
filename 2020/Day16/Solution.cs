using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day16 {

    record Ticket(int[] values);
    record Field(string name, Func<int, bool> isValid);
    record Problem(Field[] fields, Ticket ticket, Ticket[] tickets);

    [ProblemName("Ticket Translation")]
    class Solution : Solver {

        IEnumerable<Field> FieldsMatchingValue(int v, IEnumerable<Field> fields) => FieldsMatchingAllValues(new int[] { v }, fields);
        IEnumerable<Field> FieldsMatchingAllValues(IEnumerable<int> vs, IEnumerable<Field> fields) => fields.Where(field => vs.All(field.isValid));

        public object PartOne(string input) {
            var p = Parse(input);
            var res = 0;
            foreach (var ticket in p.tickets) {
                res += ticket.values.Where(value => !FieldsMatchingValue(value, p.fields).Any()).Sum();
            }
            return res;
        }

        public object PartTwo(string input) {
            var p = Parse(input);
            var tickets = new List<Ticket>();
            tickets.Add(p.ticket);
            tickets.AddRange(p.tickets.Where(ticket => ticket.values.All(v => FieldsMatchingValue(v, p.fields).Any())));

            // It turns out the problem is set up in a way, 
            // that we can always find a column that can be associated with a single field, 
            // we just need to assign them one by one...

            var fields = new HashSet<Field>(p.fields);
            var columns = Enumerable.Range(0, p.fields.Count()).ToHashSet();

            var res = 1L;
            while (fields.Any()) {
                foreach (var column in columns) {
                    var values = from ticket in tickets select ticket.values[column];
                    var fieldsForColumn = FieldsMatchingAllValues(values, fields).ToArray();
                    if (fieldsForColumn.Length == 1) {
                        var field = fieldsForColumn[0];
                        fields.Remove(field);
                        columns.Remove(column);
                        if (field.name.StartsWith("departure")) {
                            res *= p.ticket.values[column];
                        }
                        break;
                    }
                }
            }
            return res;
        }

        Problem Parse(string input) {
            var parts = input.Split("\n\n");
            var fields = parts[0].Split("\n").Select(line => {
                var name = line.Split(":")[0];
                var nums = Regex.Matches(line, "\\d+").Select(m => int.Parse(m.Value)).ToArray();
                Func<int, bool> check = (int n) => (n >= nums[0] && n <= nums[1]) || (n >= nums[2] && n <= nums[3]);
                return new Field(name, check);
            }).ToArray();

            Ticket parseTicket(string line) => new Ticket(line.Split(",").Select(int.Parse).ToArray());

            var ticket = parseTicket(parts[1].Split("\n")[1]);
            var tickets = parts[2].Split("\n").Skip(1).Select(parseTicket).ToArray();
            return new Problem(fields, ticket, tickets);
        }
    }
}