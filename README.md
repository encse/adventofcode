# Advent of code 2017 (aoc 2017)
```
.-----------------------------------------------.       
|                                               |  25
|                                               |  24
|                                               |  23
|                                               |  22
|                                               |  21
|                                               |  20
|              *                                |  19
|              └─┐                      ┌─────* |  18 **
|                └──────────────┤[]├────┘*────┤ |  17 **
|                     *──────────────────┘o───┘ |  16 **
|                     └──────────────────oTo──* |  15 **
|       *─∧∧∧─────────oTo────────────────∧∧∧──┤ |  14 **
|       └────────────────────*o──────┐┌──┤|├──┘ |  13 **
|                            └────*┌─┘└┬───┐o─┐ |  12 **
|            ┌──────────┤[]├──oTo─┘└───┘┌*o┴──┘ |  11 **
|            └──∧∧∧──────────┐┌─┤[]├────┘└────* |  10 **
|                ┌───────────┘│*───────[─]────┤ |   9 **
|                └────[─]─────┘└──────*┌──────┘ |   8 **
| *──────────────────[─]──|(──────────┘│o─────┐ |   7 **
| ├────────────────┤[]├────────*o┬o┌───┘┌─────┤ |   6 **
| └────┐┌──┤|├───o*────────────┘V├─┘o───┴───┐┌┘ |   5 **
| ┌───o└┤*───[─]──┘o────────┬───┴┴──────────┘└┐ |   4 **
| └─────┘└─────────────────┐└o┌─────*o┬──o┌───┘ |   3 **
| *─────────────┤[]├──────┐└──┴────o└┐├───┘┌──┐ |   2 **
| └──────[─]────────────*o┴──────────┘└────┘o─┘ |   1 **
'-----------------------------------------------'       

```
My C# solutions to http://adventofcode.com/2017 using .NET Core 2.0.

## Dependencies

- This library is based on `.NET Core 2.0`. It should work on Windows, Linux and OS X.
- `HtmlAgilityPack.NetCore` is used for problem download.
- `Microsoft.AspNetCore.Razor.Language` and `Microsoft.CodeAnalysis.CSharp` for razor template generation.

## Running

To run the project:

1. Install .NET Core.
2. Download the code.
3. Run `dotnet run <day>`.

To prepare for the next day:

1. Run `dotnet run update <day>`.