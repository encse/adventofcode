# Advent of code 2017 (aoc 2017)
```
.-----------------------------------------------.
|                                               |
|                                               |
|                                               |
|                                               |
|                                               |
|                                               |
|     *                                         |
| ┌───┘┌──*                                     |
| └────┴─o└┐               ┌──────────────────* |
| ┌────────┘         ┌─────┘o────┬─o*─────────┘ |
| └────┐             └─────┤|├──┐└─┐└───┤|├──*o |
| ┌────┘┌────────────┬─oTo────o┌┴──┴o*──────┐└┐ |
| ├──┬┴┴┴┴┴┬────o┌───┘┌────────┴o┌───┘┌*o───┴─┘ |
| └─┐┤   P ├─────┤┌───┘┌─o┌─────┐└────┘└──────* |
| ┌o└┤   R0├───┐o┘└──┐┌┴──┴───o┌┴───────o*──┐┌┘ |
| ├──┤   23├──┐└──┬┴┴┴┴┴┬──────┘*────────┘┌─┘└┐ |
| └──┤   AG├─┬┴───┤     ├*──────┘o────────┴───┘ |
| ┌──┤     ├o│┌───┤     ├┘┌───────────────|(──* |
| ├──┴┬┬┬┬┬┴─┘└───┤  ALU├─┘┌┐*────────────────┘ |
| └────┐o─────┬───┴┬┬┬┬┬┴──┘=└───┐┌─┤|├────*──┐ |
| ┌───o└─────┐└────┐┌────oTo─────┘└──┐*────┘o─┘ |
| └──────────┴─┤|├─┘└────┐┌───∧∧∧────┘└───────* |
| ┌────────────────[─]───┘│┌──────|(─────┐*───┘ |
| ├────────────────∧∧∧────┴┘*───∧∧∧─────┐=└───┐ |
| └────────┤|├─────────o*───┘o──────────┴─────┘ |
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