package day02

import scala.io.Source

case object Solution extends App {

  def partOne(input: String): Int = {
    val diffs = for {
      line <- input.split("\n")
      numbers = line.split("\t").map(_.toInt)
    } yield numbers.max - numbers.min

    diffs.sum
  }

  def partTwo(input: String): Int = {
    val divisions = for {
      line <- input.split("\n")
      numbers = line.split("\t").map(_.toInt)
      a <- numbers
      b <- numbers
      if a > b && a % b == 0
    } yield a / b

    divisions.sum
  }

  val input = Source.fromFile(getClass.getPackage.getName + "/input.in").mkString

  println(partOne(input))
  println(partTwo(input))

}
