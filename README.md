
# Advent of Code 2017
```
.-----------------------------------------------.       
|                                               |  25
|                                               |  24
|                                               |  23
|                                               |  22
|    *                                          |  21
|    └──────────────────oTo─────────*           |  20 **
|            *────∧∧∧────|(────────┐└─┐         |  19 **
| ┌────┬─────┘┌*o────┬───────∧∧∧──┐└┬┴┴┴┴┬─┐    |  18 **
| ├───┐=┌─────┘└────*├─o┌─────────┤┌┤  MC├─┘    |  17 **
| └──o└─┴o*─────┤|├─┘├┴┴┴┴┬──┐o───┘└┤  AS├────┐ |  16 **
| V┌*────┐└────┐o──┬─┤   5├─┐└──────┤  GM├────┘ |  15 **
| └┘└───*└────┐└──┐└─┤   A├─┴──o┌───┤  IK├────┐ |  14 **
| *─┤[]├┘o────┴──┐└──┤   2├─────┴───┤    ├────┤ |  13 **
| └────────────*o┴───┤   2├─────────┼┬┬┬┬┴────┘ |  12 **
| *───────────┐└───┐o┴┬┬┬┬┴─┬───────┘└──oTo───┐ |  11 **
| ├───────*o──┴───┐└───────┐│┌──────────oTo───┘ |  10 **
| └────┐┌o└─────*o┴────────┘│└────[─]─────────┐ |   9 **
| ┌────┘├─┬┴┴┴┴┴┼───────*o──┴──────[─]────────┤ |   8 **
| │o────┴─┤     ├────┬─o└──┐┌──────────*┌─────┘ |   7 **
| └──────┐┤  ROT├────┤┌────┘│*────oTo──┘│o────┐ |   6 **
| o─┬──┐V│┤   13├──┐o┘└─────┘└┐┌──────*o┴─────┤ |   5 **
| ┌─┘o─┴┘└┴┬┬┬┬┬┼──┤┌─────────┤└┬────o└─────*V│ |   4 **
| ├───|(───────┴┘o─┘└┐o───────┴o│*──────────┘└┘ |   3 **
| │o────┤|├─┐┌───────┘┌─────────┘└──────┤[]├──* |   2 **
| └─────────┘└───┤[]├─┴o*─────────────────────┘ |   1 **
'-----------------------------------------------'       

```
C# solutions to http://adventofcode.com/2017 using .NET Core 2.0.

## Dependencies

- This library is based on `.NET Core 2.0`. It should work on Windows, Linux and OS X.
- `HtmlAgilityPack.NetCore` is used for problem download.

## Running

To run the project:

1. Install .NET Core.
2. Download the code.
3. Run `dotnet run <day>`.

To prepare for the next day:

1. Run `dotnet run update <day>`.