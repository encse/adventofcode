original source: [https://adventofcode.com/2015/day/15](https://adventofcode.com/2015/day/15)
## --- Day 15: Science for Hungry People ---
Today, you set out on the task of perfecting your milk-dunking cookie recipe.  All you have to do is find the right balance of ingredients.

Your recipe leaves room for exactly <code>100</code> teaspoons of ingredients.  You make a list of the <em>remaining ingredients you could use to finish the recipe</em> (your puzzle input) and their <em>properties per teaspoon</em>:


 - <code>capacity</code> (how well it helps the cookie absorb milk)
 - <code>durability</code> (how well it keeps the cookie intact when full of milk)
 - <code>flavor</code> (how tasty it makes the cookie)
 - <code>texture</code> (how it improves the feel of the cookie)
 - <code>calories</code> (how many calories it adds to the cookie)

You can only measure ingredients in whole-teaspoon amounts accurately, and you have to be accurate so you can reproduce your results in the future.  The <em>total score</em> of a cookie can be found by adding up each of the properties (negative totals become <code>0</code>) and then multiplying together everything except calories.

For instance, suppose you have these two ingredients:

<pre>
<code>Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3
</code>
</pre>

Then, choosing to use <code>44</code> teaspoons of butterscotch and <code>56</code> teaspoons of cinnamon (because the amounts of each ingredient must add up to <code>100</code>) would result in a cookie with the following properties:


 - A <code>capacity</code> of <code>44*-1 + 56*2 = 68</code>
 - A <code>durability</code> of <code>44*-2 + 56*3 = 80</code>
 - A <code>flavor</code> of <code>44*6 + 56*-2 = 152</code>
 - A <code>texture</code> of <code>44*3 + 56*-1 = 76</code>

Multiplying these together (<code>68 * 80 * 152 * 76</code>, ignoring <code>calories</code> for now) results in a total score of  <code>62842880</code>, which happens to be the best score possible given these ingredients.  If any properties had produced a negative total, it would have instead become zero, causing the whole score to multiply to zero.

Given the ingredients in your kitchen and their properties, what is the <em>total score</em> of the highest-scoring cookie you can make?


## --- Part Two ---
Your cookie recipe becomes wildly popular!  Someone asks if you can make another recipe that has exactly <code>500</code> calories per cookie (so they can use it as a meal replacement).  Keep the rest of your award-winning process the same (100 teaspoons, same ingredients, same scoring system).

For example, given the ingredients above, if you had instead selected <code>40</code> teaspoons of butterscotch and <code>60</code> teaspoons of cinnamon (which still adds to <code>100</code>), the total calorie count would be <code>40*8 + 60*3 = 500</code>.  The total score would go down, though: only <code>57600000</code>, the best you can do in such trying circumstances.

Given the ingredients in your kitchen and their properties, what is the <em>total score</em> of the highest-scoring cookie you can make with a calorie total of <code>500</code>?


