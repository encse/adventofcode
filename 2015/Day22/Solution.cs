using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day22 {

    class Solution : Solver {

        public string GetName() => "Wizard Simulator 20XX";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var state0 = Parse(input);
            return BinarySearch(mana => TrySolve(state0.WithManaLimit(mana), false));
        }

        int PartTwo(string input) {
            var state0 = Parse(input);
            return BinarySearch(mana => TrySolve(state0.WithManaLimit(mana), true));
        }

        int BinarySearch(Func<int, bool> f) {
            var hi = 1;
            while (!f(hi)) {
                hi *= 2;
            }
            var lo = hi / 2;
            var first = false;
            while (hi - lo > 1) {
                var m = (hi + lo) / 2;
                if (!first && f(m)) {
                    hi = m;
                } else {
                    lo = m;
                }
                first = false;
            }
            return hi;
        }

        bool TrySolve(State state, bool hard) {
            if (hard) {
                state = state.Damage(1);
            }
            state = state.ApplyEffects();
            foreach (var stateT in state.PlayerSteps()) {
                state = stateT.ApplyEffects();
                state = state.BossStep();
                if (state.bossHp <= 0 || state.playerHp > 0 && TrySolve(state, hard)) {
                    return true;
                }
            }
            return false;
        }

        State Parse(string input){
            var lines = input.Split("\n");
            return new State {
                playerHp = 50,
                playerMana = 500,
                bossHp = int.Parse(lines[0].Split(": ")[1]),
                bossDamage = int.Parse(lines[1].Split(": ")[1])
            };
        }
    }


    class State {
        const int missileMana = 53;
        const int drainMana = 73;
        const int shieldMana = 113;
        const int poisonMana = 173;
        const int rechargeMana = 229;

        public int shield;
        public int poison;
        public int recharge;
        public int playerHp;
        public int bossHp;
        public int playerMana;
        public int bossDamage;
        public int usedMana;
        public int playerArmor;
        public int manaLimit;

        public State Dup() {
            return this.MemberwiseClone() as State;
        }

        public State WithManaLimit(int manaLimit) {
            var newState = Dup();
            newState.manaLimit = manaLimit;
            return newState;
        }

        public State ApplyEffects() {
            if (playerHp <= 0 || bossHp <= 0) {
                return this;
            }

            var newState = Dup();
            if (newState.poison > 0) {
                newState.bossHp -= 3;
                newState.poison--;
            }

            if (newState.recharge > 0) {
                newState.playerMana += 101;
                newState.recharge--;
            }

            if (newState.shield > 0) {
                newState.shield--;
                newState.playerArmor = 7;
            } else {
                newState.playerArmor = 0;
            }
            return newState;
        }

        public State Damage(int damage) {
            if (playerHp <= 0 || bossHp <= 0) {
                return this;
            }

            var step = Dup();
            step.playerHp -= damage;
            return step;
        }

        public State BossStep(){
            if (playerHp <= 0 || bossHp <= 0) {
                return this;
            }

            var step = Dup();
            step.playerHp -= Math.Max(1, step.bossDamage - step.playerArmor);
            return step;
        }

        public IEnumerable<State> PlayerSteps() {

            if (playerHp <= 0 || bossHp <= 0) {
                yield return this;
                yield break;
            }
       
            if (playerMana >= missileMana && missileMana + usedMana <= manaLimit) {
                var c = Dup();
                c.playerMana -= missileMana;
                c.usedMana += missileMana;
                c.bossHp -= 4;
                yield return c;
            }

            if (playerMana >= drainMana && drainMana + usedMana <= manaLimit) {
                var c = Dup();
                c.playerMana -= drainMana;
                c.usedMana += drainMana;
                c.bossHp -= 2;
                c.playerHp += 2;
                yield return c;
            }

            if (playerMana >= shieldMana && shield == 0 && shieldMana + usedMana <= manaLimit) {
                var c = Dup();
                c.playerMana -= shieldMana;
                c.usedMana += shieldMana;
                c.shield = 6;
                yield return c;
            }

            if (playerMana >= poisonMana && poison == 0 && poisonMana + usedMana <= manaLimit) {
                var c = Dup();
                c.playerMana -= poisonMana;
                c.usedMana += poisonMana;
                c.poison = 6;
                yield return c;
            }

            if (playerMana >= rechargeMana && recharge == 0 && rechargeMana + usedMana <= manaLimit) {
                var c = Dup();
                c.playerMana -= rechargeMana;
                c.usedMana += rechargeMana;
                c.recharge = 5;
                yield return c;
            }
        }
    }
}