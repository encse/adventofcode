import scala.io.Source

case object Day01 extends App {


  def solve(input: String): Int = {
    val numbersToSum = for {
      i <- 0 until input.length
      if input.charAt(i) == input.charAt((i + 1) % input.length)
    } yield input.charAt(i).asDigit
    numbersToSum.sum
  }

  def solve2(input: String): Int = {
    val numbersToSum = for {
      i <- 0 until input.length
      if input.charAt(i) == input.charAt((i + input.length/2) % input.length)
    } yield input.charAt(i).asDigit
    numbersToSum.sum
  }

  var input = Source.fromFile(productPrefix.toLowerCase + "/input.in").mkString

  println(solve(input))
  println(solve2(input))
}