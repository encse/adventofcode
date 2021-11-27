using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day15;

[ProblemName("Beverage Bandits")]
class Solution : Solver {

    public object PartOne(string input) {
        return Outcome(input, 3, 3, false).score;
    }

    public object PartTwo(string input) {
        var elfAp = 4;
        while (true) {
            var outcome = Outcome(input, 3, elfAp, false);
            if (outcome.noElfDied) {
                return outcome.score;
            }
            elfAp++;
        }
    }

    (bool noElfDied, int score) Outcome(string input, int goblinAp, int elfAp, bool tsto) {
        var game = Parse(input, goblinAp, elfAp);
        var elfCount = game.players.Count(player => player.elf);

        if (tsto) {
            Console.WriteLine(game.Tsto());
        }

        while (!game.Finished()) {
            game.Step();
            if (tsto) {
                Console.WriteLine(game.Tsto());
            }
        }

        return (game.players.Count(p => p.elf) == elfCount, game.rounds * game.players.Select(player => player.hp).Sum());
    }


    Game Parse(string input, int goblinAp, int elfAp) {
        var players = new List<Player>();
        var lines = input.Split("\n");
        var mtx = new Block[lines.Length, lines[0].Length];

        var game = new Game { mtx = mtx, players = players };

        for (var irow = 0; irow < lines.Length; irow++) {
            for (var icol = 0; icol < lines[0].Length; icol++) {
                switch (lines[irow][icol]) {
                    case '#':
                        mtx[irow, icol] = Wall.Block;
                        break;
                    case '.':
                        mtx[irow, icol] = Empty.Block;
                        break;
                    case var ch when ch == 'G' || ch == 'E':
                        var player = new Player {
                            elf = ch == 'E',
                            ap = ch == 'E' ? elfAp : goblinAp,
                            pos = (irow, icol),
                            game = game
                        };
                        players.Add(player);
                        mtx[irow, icol] = player;
                        break;
                }
            }
        }
        return game;
    }


}

class Game {
    public Block[,] mtx;
    public List<Player> players;
    public int rounds;

    private bool ValidPos((int irow, int icol) pos) =>
        pos.irow >= 0 && pos.irow < this.mtx.GetLength(0) && pos.icol >= 0 && pos.icol < this.mtx.GetLength(1);

    public Block GetBlock((int irow, int icol) pos) =>
        ValidPos(pos) ? mtx[pos.irow, pos.icol] : Wall.Block;

    public void Step() {
        var finishedBeforeEndOfRound = false;
        foreach (var player in players.OrderBy(player => player.pos).ToArray()) {
            if (player.hp > 0) {
                finishedBeforeEndOfRound |= Finished();
                player.Step();
            }
        }

        if (!finishedBeforeEndOfRound) {
            rounds++;
        }
    }

    public bool Finished() =>
        players.Where(p => p.elf).All(p => p.hp == 0) ||
        players.Where(p => !p.elf).All(p => p.hp == 0);

    public string Tsto() {

        var res = "";
        res += rounds == 0 ? "Initial:\n" : $"After round {rounds}:\n";
        for (var irow = 0; irow < mtx.GetLength(0); irow++) {
            for (var icol = 0; icol < mtx.GetLength(1); icol++) {
                res += GetBlock((irow, icol)) switch {
                    Player p when p.elf => "E",
                    Player p when !p.elf => "G",
                    Empty _ => ".",
                    Wall _ => "#",
                    _ => throw new ArgumentException()
                };
            }

            foreach (var player in players.Where(player => player.pos.irow == irow).OrderBy(player => player.pos)) {
                var ch = player.elf ? 'E' : 'G';
                res += $" {ch}{{{player.pos.irow}, {player.pos.icol}}}({player.hp})";
            }
            res += "\n";
        }
        res += "\n";
        return res;
    }
}

abstract class Block { }

class Empty : Block {
    public static readonly Empty Block = new Empty();
    private Empty() { }
}

class Wall : Block {
    public static readonly Wall Block = new Wall();
    private Wall() { }
}

class Player : Block {
    public (int irow, int icol) pos;
    public bool elf;
    public int ap = 3;
    public int hp = 200;
    public Game game;

    public bool Step() {
        if (hp <= 0) {
            return false;
        } else if (Attack()) {
            return true;
        } else if (Move()) {
            Attack();
            return true;
        } else {
            return false;
        }
    }

    private bool Move() {
        var targets = FindTargets();
        if (!targets.Any()) {
            return false;
        }
        var opponent = targets.OrderBy(a => a.target).First();
        var nextPos = targets.Where(a => a.player == opponent.player).Select(a => a.firstStep).OrderBy(_ => _).First();
        (game.mtx[nextPos.irow, nextPos.icol], game.mtx[pos.irow, pos.icol]) =
            (game.mtx[pos.irow, pos.icol], game.mtx[nextPos.irow, nextPos.icol]);
        pos = nextPos;
        return true;
    }

    private IEnumerable<(Player player, (int irow, int icol) firstStep, (int irow, int icol) target)> FindTargets() {

        var minDist = int.MaxValue;
        foreach (var (otherPlayer, firstStep, target, dist) in BlocksNextToOpponentsByDistance()) {
            if (dist > minDist) {
                break;
            } else {
                minDist = dist;
                yield return (otherPlayer, firstStep, target);
            }
        }
    }

    private IEnumerable<(Player player, (int irow, int icol) firstStep, (int irow, int icol) target, int dist)> BlocksNextToOpponentsByDistance() {
        var seen = new HashSet<(int irow, int icol)>();
        seen.Add(pos);
        var q = new Queue<((int irow, int icol) pos, (int drow, int dcol) origDir, int dist)>();

        foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
            var posT = (pos.irow + drow, pos.icol + dcol);
            q.Enqueue((posT, posT, 1));
        }

        while (q.Any()) {
            var (pos, firstStep, dist) = q.Dequeue();

            if (game.GetBlock(pos) is Empty) {
                foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
                    var posT = (pos.irow + drow, pos.icol + dcol);
                    if (!seen.Contains(posT)) {
                        seen.Add(posT);
                        q.Enqueue((posT, firstStep, dist + 1));

                        var nextBlock = game.GetBlock(posT);
                        if (nextBlock is Player) {
                            var player = nextBlock as Player;
                            if (player.elf != this.elf) {
                                yield return (player, firstStep, pos, dist);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool Attack() {
        var opponents = new List<Player>();

        foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
            var posT = (this.pos.irow + drow, this.pos.icol + dcol);
            var block = game.GetBlock(posT);
            switch (block) {
                case Player otherPlayer when otherPlayer.elf != this.elf:
                    opponents.Add(otherPlayer);
                    break;
            }
        }

        if (!opponents.Any()) {
            return false;
        }
        var minHp = opponents.Select(a => a.hp).Min();
        var opponent = opponents.First(a => a.hp == minHp);
        opponent.hp -= this.ap;
        if (opponent.hp <= 0) {
            game.players.Remove(opponent);
            game.mtx[opponent.pos.irow, opponent.pos.icol] = Empty.Block;
        }
        return true;
    }

}
