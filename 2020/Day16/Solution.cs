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

        public object PartOne(string input) {
            var p = Parse(input);
            var res = 0;
            foreach (var ticket in p.tickets) {
                res += ticket.values.Where(v => p.fields.All(field => !field.isValid(v))).Sum();
            }
            return res;
        }

        public object PartTwo(string input) {
            var p = Parse(input);
            var tickets = new List<Ticket>();
            tickets.Add(p.ticket);
            tickets.AddRange(p.tickets.Where(ticket => ticket.values.All(v => p.fields.Any(field => field.isValid(v)))));

            List<Field> rec(Queue<Field> fields, int ifield) {
                var cfield = fields.Count;

                if (cfield == 0) {
                    return new List<Field>();
                }

                for (var i = 0; i < cfield; i++) {
                    var field = fields.Dequeue();

                    if (tickets.All(ticket => field.isValid(ticket.values[ifield]))) {

                        var orderedFields = rec(fields, ifield + 1);
                        if (orderedFields != null) {
                            orderedFields.Insert(0, field);
                            return orderedFields;
                        }

                    }

                    fields.Enqueue(field);
                }

                return null;

            }

            var orderedFields = rec(new Queue<Field>(p.fields), 0);
            var res = 1L;
            for (var i = 0; i < orderedFields.Count; i++) {
                var field = orderedFields[i];
                if (field.name.StartsWith("departure")) {
                    res *= p.ticket.values[i];
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