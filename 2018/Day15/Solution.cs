using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day15 {

    class Solution : Solver {

        public string GetName() => "Beverage Bandits";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            return Outcome(input, 3, 3).score;
        }

        int PartTwo(string input) {
            var elfAp = 3;
            while(true){
                var outcome = Outcome(input, 3, elfAp);
                if(outcome.noElfDied){
                    return outcome.score;
                }
                elfAp++;
            }
        }
        
        (bool noElfDied, int score) Outcome(string input, int goblinAp, int elfAp){
            var state = Parse(input, goblinAp, elfAp);
            var elfCount = state.players.Count(player => player.elf);
            
            var rounds = 0;
            while (Step(state)) {
                rounds++;
            }
            
            return (state.players.Count(p => p.elf) == elfCount, (rounds -1) * state.players.Select(player => player.hp).Sum());
        }

        bool Step(State state) {
            var moved = false;
            foreach (var player in state.players.OrderBy(a => a.pos)) {
                if (player.hp > 0) {
                    if(Attack(state, player)){
                        moved = true;
                    } else {
                        moved |= Move(state, player);
                        moved |= Attack(state, player);
                    }
                }
            }
            return moved;
        }

        bool Move(State state, Player player) {
            var opponents = ClosestOpponents(state, player);
            if (!opponents.Any()) {
                return false;
            }
            var opponent = opponents.OrderBy(a => a.player.pos).First();
            var nextPos = opponents.Where(a => a.player == opponent.player).Select(a => a.firstStep).OrderBy(_ => _).First();
            (state.mtx[nextPos.irow, nextPos.icol], state.mtx[player.pos.irow, player.pos.icol]) =
                (state.mtx[player.pos.irow, player.pos.icol], state.mtx[nextPos.irow, nextPos.icol]);
            player.pos = nextPos;
            return true;
        }


        IEnumerable<(Player player, (int irow, int icol) firstStep)> ClosestOpponents(State state, Player player) {
            var minDist = int.MaxValue;
            foreach (var (otherPlayer, firstStep, dist) in OpponentsByDistance(state, player)) {
                if (dist > minDist) {
                    break;
                } else {
                    minDist = dist;
                    yield return (otherPlayer, firstStep);
                }
            }
        }

        IEnumerable<(Player player, (int irow, int icol) firstStep, int dist)> OpponentsByDistance(State state, Player player) {
            var seen = new HashSet<(int irow, int icol)>();
            seen.Add(player.pos);
            var q = new Queue<((int irow, int icol) pos, (int drow, int dcol) origDir, int dist)>();

            foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
                var posT = (player.pos.irow + drow, player.pos.icol + dcol);
                q.Enqueue((posT, posT, 1));
            }

            while (q.Any()) {
                var (pos, firstStep, dist) = q.Dequeue();
                switch (GetBlock(state, pos)) {
                    case Player otherPlayer when player != otherPlayer && otherPlayer.elf != player.elf:
                        yield return (otherPlayer, firstStep, dist);
                        break;

                    case Wall _:
                        break;

                    case Empty _:
                        foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
                            var posT = (pos.irow + drow, pos.icol + dcol);
                            if (!seen.Contains(posT)){
                                seen.Add(posT);
                                q.Enqueue((posT, firstStep, dist + 1));
                            }
                        }
                        break;
                }
            }
        }

        bool Attack(State state, Player player) {
            var opponents = new List<Player>();

            foreach (var (drow, dcol) in new[] { (-1, 0), (0, -1), (0, 1), (1, 0) }) {
                var posT = (player.pos.irow + drow, player.pos.icol + dcol);
                var block = GetBlock(state, posT);
                switch (block) {
                    case Player otherPlayer when otherPlayer.elf != player.elf:
                        opponents.Add(otherPlayer);
                        break;
                }
            }

            if(!opponents.Any()){
                return false;
            }
            var minHp = opponents.Select(a => a.hp).Min();
            var opponent = opponents.First(a => a.hp == minHp);
            opponent.hp -= player.ap;
            if (opponent.hp <= 0) {
                state.players.Remove(opponent);
                state.mtx[opponent.pos.irow, opponent.pos.icol] = new Empty();
            }
            return true;
        }

     
        bool ValidPos(State state, (int irow, int icol) pos) {
            return !(pos.irow < 0 || pos.irow >= state.mtx.GetLength(0) || pos.icol < 0 || pos.icol >= state.mtx.GetLength(1));
        }
        Block GetBlock(State state, (int irow, int icol) pos) {
            return ValidPos(state, pos) ? state.mtx[pos.irow, pos.icol] : new Wall();
        }

        State Parse(string input, int goblinAp, int elfAp) {
            var players = new List<Player>();
            var lines = input.Split("\n");
            var mtx = new Block[lines.Length, lines[0].Length];
            for (var irow = 0; irow < lines.Length; irow++) {
                for (var icol = 0; icol < lines[0].Length; icol++) {
                    switch (lines[irow][icol]) {
                        case '#':
                            mtx[irow, icol] = new Wall();
                            break;
                        case '.':
                            mtx[irow, icol] = new Empty();
                            break;
                        case var ch when ch == 'G' || ch == 'E':
                            var player = new Player { 
                                elf = ch == 'E', 
                                ap = ch == 'E' ? elfAp : goblinAp,
                                pos = (irow, icol) };
                            players.Add(player);
                            mtx[irow, icol] = player;
                            break;
                    }
                }
            }
            return new State { mtx = mtx, players = players };
        }
    }
    class State {
        public Block[,] mtx;
        public List<Player> players;
    }
    abstract class Block { }
    class Empty : Block { }
    class Wall : Block { }
    class Player : Block {
        public (int irow, int icol) pos;
        public bool elf;
        public int ap = 3;
        public int hp = 200;
    }
}