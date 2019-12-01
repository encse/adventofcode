using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day21 {

    class Solution : Solver {

        public string GetName() => "RPG Simulator 20XX";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var boss = Parse(input);
            var minGold = int.MaxValue;
            foreach (var c in Buy()) {
                if (DefeatsBoss((c.damage, c.armor, 100), boss)) {
                    minGold = Math.Min(c.gold, minGold);
                }
            }
            return minGold;
        }

        int PartTwo(string input) {
            var boss = Parse(input);
            var maxGold = 0;
            foreach (var c in Buy()) {
                if (!DefeatsBoss((c.damage, c.armor, 100), boss)) {
                    maxGold = Math.Max(c.gold, maxGold);
                }
            }
            return maxGold;
        }

        (int damage, int armor, int hp) Parse(string input) {
            var lines = input.Split("\n");
            var hp = int.Parse(lines[0].Split(": ")[1]);
            var damage = int.Parse(lines[1].Split(": ")[1]);
            var armor = int.Parse(lines[2].Split(": ")[1]);
            return (damage, armor, hp);
        }

        bool DefeatsBoss((int damage, int armor, int hp) player, (int damage, int armor, int hp) boss) {
            while (true) {
                boss.hp -= Math.Max(player.damage - boss.armor, 1);
                if (boss.hp <= 0) {
                    return true;
                }

                player.hp -= Math.Max(boss.damage - player.armor, 1);
                if (player.hp <= 0) {
                    return false;
                }
            }
        }

        IEnumerable<(int gold, int damage, int armor)> Buy() {
            return
                from weapon in Buy(1, 1, new[] { (8, 4, 0), (10, 5, 0), (25, 6, 0), (40, 7, 0), (74, 8, 0) })
                from armor in Buy(0, 1, new[] { (13, 0, 1), (31, 0, 2), (53, 0, 3), (75, 0, 4), (102, 0, 5) })
                from ring in Buy(1, 2, new[] { (25, 1, 0), (50, 2, 0), (100, 3, 0), (20, 0, 1), (40, 0, 2), (80, 0, 3) })
                select Sum(weapon, armor, ring);
        }

        IEnumerable<(int gold, int damage, int armor)> Buy(int min, int max, (int gold, int damage, int armor)[] items) {
            if (min == 0) {
                yield return (0, 0, 0);
            }

            foreach (var item in items) {
                yield return item;
            }

            if (max == 2) {
                for (int i = 0; i < items.Length; i++) {
                    for (int j = i + 1; j < items.Length; j++) {
                        yield return Sum(items[i], items[j]);
                    }
                }
            }
        }

        (int gold, int damage, int armor) Sum(params (int gold, int damage, int armor)[] items) {
            return (items.Select(item => item.gold).Sum(), items.Select(item => item.damage).Sum(), items.Select(item => item.armor).Sum());
        }

    }
}