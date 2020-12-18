original source: [https://adventofcode.com/2019/day/1](https://adventofcode.com/2019/day/1)
## --- Day 1: The Tyranny of the Rocket Equation ---
Santa has become stranded at the edge of the Solar System while delivering presents to other planets! To accurately calculate his position in space, safely align his warp drive, and return to Earth in time to save Christmas, he needs you to bring him measurements from <em>fifty stars</em>.

Collect stars by solving puzzles.  Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.  Each puzzle grants <em>one star</em>. Good luck!

The Elves quickly load you into a spacecraft and prepare to launch.

At the first Go / No Go poll, every Elf is Go until the Fuel Counter-Upper.  They haven't determined the amount of fuel required yet.

Fuel required to launch a given <em>module</em> is based on its <em>mass</em>.  Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

For example:


 - For a mass of <code>12</code>, divide by 3 and round down to get <code>4</code>, then subtract 2 to get <code>2</code>.
 - For a mass of <code>14</code>, dividing by 3 and rounding down still yields <code>4</code>, so the fuel required is also <code>2</code>.
 - For a mass of <code>1969</code>, the fuel required is <code>654</code>.
 - For a mass of <code>100756</code>, the fuel required is <code>33583</code>.

The Fuel Counter-Upper needs to know the total fuel requirement.  To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

<em>What is the sum of the fuel requirements</em> for all of the modules on your spacecraft?


## --- Part Two ---
During the second Go / No Go poll, the Elf in charge of the Rocket Equation Double-Checker stops the launch sequence.  Apparently, you forgot to include additional fuel for the fuel you just added.

Fuel itself requires fuel just like a module - take its mass, divide by three, round down, and subtract 2.  However, that fuel <em>also</em> requires fuel, and <em>that</em> fuel requires fuel, and so on.  Any mass that would require <em>negative fuel</em> should instead be treated as if it requires <em>zero fuel</em>; the remaining mass, if any, is instead handled by <em>wishing really hard</em>, which has no mass and is outside the scope of this calculation.

So, for each module mass, calculate its fuel and add it to the total.  Then, treat the fuel amount you just calculated as the input mass and repeat the process, continuing until a fuel requirement is zero or negative. For example:


 - A module of mass <code>14</code> requires <code>2</code> fuel.  This fuel requires no further fuel (2 divided by 3 and rounded down is <code>0</code>, which would call for a negative fuel), so the total fuel required is still just <code>2</code>.
 - At first, a module of mass <code>1969</code> requires <code>654</code> fuel.  Then, this fuel requires <code>216</code> more fuel (<code>654 / 3 - 2</code>).  <code>216</code> then requires <code>70</code> more fuel, which requires <code>21</code> fuel, which requires <code>5</code> fuel, which requires no further fuel.  So, the total fuel required for a module of mass <code>1969</code> is <code>654 + 216 + 70 + 21 + 5 = 966</code>.
 - The fuel required by a module of mass <code>100756</code> and its fuel is: <code>33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2 = 50346</code>.

<em>What is the sum of the fuel requirements</em> for all of the modules on your spacecraft when also taking into account the mass of the added fuel? (Calculate the fuel requirements for each module separately, then add them all up at the end.)


