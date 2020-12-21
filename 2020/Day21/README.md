original source: [https://adventofcode.com/2020/day/21](https://adventofcode.com/2020/day/21)
## --- Day 21: Allergen Assessment ---
You reach the train's last stop and the closest you can get to your vacation island without getting wet. There aren't even any boats here, but nothing can stop you now: you build a raft. You just need a few days' worth of food for your journey.

You don't speak the local language, so you can't read any ingredients lists. However, sometimes, allergens are listed in a language you <em>do</em> understand. You should be able to use this information to determine which ingredient contains which allergen and work out which foods are safe to take with you on your trip.

You start by compiling a list of foods (your puzzle input), one food per line. Each line includes that food's <em>ingredients list</em> followed by some or all of the allergens the food contains.

Each allergen is found in exactly one ingredient. Each ingredient contains zero or one allergen. <em>Allergens aren't always marked</em>; when they're listed (as in <code>(contains nuts, shellfish)</code> after an ingredients list), the ingredient that contains each listed allergen will be <em>somewhere in the corresponding ingredients list</em>. However, even if an allergen isn't listed, the ingredient that contains that allergen could still be present: maybe they forgot to label it, or maybe it was labeled in a language you don't know.

For example, consider the following list of foods:

<pre>
<code>mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)
</code>
</pre>

The first food in the list has four ingredients (written in a language you don't understand): <code>mxmxvkd</code>, <code>kfcds</code>, <code>sqjhc</code>, and <code>nhms</code>. While the food might contain other allergens, a few allergens the food definitely contains are listed afterward: <code>dairy</code> and <code>fish</code>.

The first step is to determine which ingredients <em>can't possibly</em> contain any of the allergens in any food in your list. In the above example, none of the ingredients <code>kfcds</code>, <code>nhms</code>, <code>sbzzf</code>, or <code>trh</code> can contain an allergen. Counting the number of times any of these ingredients appear in any ingredients list produces <em><code>5</code></em>: they all appear once each except <code>sbzzf</code>, which appears twice.

Determine which ingredients cannot possibly contain any of the allergens in your list. <em>How many times do any of those ingredients appear?</em>


## --- Part Two ---
Now that you've isolated the inert ingredients, you should have enough information to figure out which ingredient contains which allergen.

In the above example:


 - <code>mxmxvkd</code> contains <code>dairy</code>.
 - <code>sqjhc</code> contains <code>fish</code>.
 - <code>fvjkl</code> contains <code>soy</code>.

Arrange the ingredients <em>alphabetically by their allergen</em> and separate them by commas to produce your <em>canonical dangerous ingredient list</em>. (There should <em>not be any spaces</em> in your canonical dangerous ingredient list.) In the above example, this would be <em><code>mxmxvkd,sqjhc,fvjkl</code></em>.

Time to stock your raft with supplies. <em>What is your canonical dangerous ingredient list?</em>


