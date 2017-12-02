package day01

import scala.io.Source

case object Solution extends App {

  def inverseCaptcha(input: String, skip: Int): Int = {
    val numbersToSum = for {
      i <- 0 until input.length
      if input.charAt(i) == input.charAt((i + skip) % input.length)
    } yield input.charAt(i).asDigit
    numbersToSum.sum
  }

  def partOne(input: String): Int = inverseCaptcha(input, 1)
  def partTwo(input: String): Int = inverseCaptcha(input, input.length /2)

  val input = Source.fromFile(getClass.getPackage.getName + "/input.in").mkString

  println(partOne(input))
  println(partTwo(input))

}
