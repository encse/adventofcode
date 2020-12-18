original source: [https://adventofcode.com/2020/day/18](https://adventofcode.com/2020/day/18)
## --- Day 18: Operation Order ---
As you look out the window and notice a heavily-forested continent slowly appear over the horizon, you are interrupted by the child sitting next to you. They're curious if you could help them with their math homework.

Unfortunately, it seems like this "math" [follows different rules](https://www.youtube.com/watch?v=3QtRK7Y2pPU&t=15) than you remember.

The homework (your puzzle input) consists of a series of expressions that consist of addition (<code>+</code>), multiplication (<code>*</code>), and parentheses (<code>(...)</code>). Just like normal math, parentheses indicate that the expression inside must be evaluated before it can be used by the surrounding expression. Addition still finds the sum of the numbers on both sides of the operator, and multiplication still finds the product.

However, the rules of <em>operator precedence</em> have changed. Rather than evaluating multiplication before addition, the operators have the <em>same precedence</em>, and are evaluated left-to-right regardless of the order in which they appear.

For example, the steps to evaluate the expression <code>1 + 2 * 3 + 4 * 5 + 6</code> are as follows:

<pre>
<code><em>1 + 2</em> * 3 + 4 * 5 + 6
  <em>3   * 3</em> + 4 * 5 + 6
      <em>9   + 4</em> * 5 + 6
         <em>13   * 5</em> + 6
             <em>65   + 6</em>
                 <em>71</em>
</code>
</pre>

Parentheses can override this order; for example, here is what happens if parentheses are added to form <code>1 + (2 * 3) + (4 * (5 + 6))</code>:

<pre>
<code>1 + <em>(2 * 3)</em> + (4 * (5 + 6))
<em>1 +    6</em>    + (4 * (5 + 6))
     7      + (4 * <em>(5 + 6)</em>)
     7      + <em>(4 *   11   )</em>
     <em>7      +     44</em>
            <em>51</em>
</code>
</pre>

Here are a few more examples:


 - <code>2 * 3 + (4 * 5)</code> becomes <em><code>26</code></em>.
 - <code>5 + (8 * 3 + 9 + 3 * 4 * 3)</code> becomes <em><code>437</code></em>.
 - <code>5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))</code> becomes <em><code>12240</code></em>.
 - <code>((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2</code> becomes <em><code>13632</code></em>.

Before you can help with the homework, you need to understand it yourself. <em>Evaluate the expression on each line of the homework; what is the sum of the resulting values?</em>


## --- Part Two ---
You manage to answer the child's questions and they finish part 1 of their homework, but get stuck when they reach the next section: <em>advanced</em> math.

Now, addition and multiplication have <em>different</em> precedence levels, but they're not the ones you're familiar with. Instead, addition is evaluated <em>before</em> multiplication.

For example, the steps to evaluate the expression <code>1 + 2 * 3 + 4 * 5 + 6</code> are now as follows:

<pre>
<code><em>1 + 2</em> * 3 + 4 * 5 + 6
  3   * <em>3 + 4</em> * 5 + 6
  3   *   7   * <em>5 + 6</em>
  <em>3   *   7</em>   *  11
     <em>21       *  11</em>
         <em>231</em>
</code>
</pre>

Here are the other examples from above:


 - <code>1 + (2 * 3) + (4 * (5 + 6))</code> still becomes <em><code>51</code></em>.
 - <code>2 * 3 + (4 * 5)</code> becomes <em><code>46</code></em>.
 - <code>5 + (8 * 3 + 9 + 3 * 4 * 3)</code> becomes <em><code>1445</code></em>.
 - <code>5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))</code> becomes <em><code>669060</code></em>.
 - <code>((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2</code> becomes <em><code>23340</code></em>.

<em>What do you get if you add up the results of evaluating the homework problems using these new rules?</em>


