
# Advent of Code (2015-2019)
C# solutions to the advent of code problems.
Check out http://adventofcode.com.

![](demo.gif)

## Dependencies

- This project is based on `.NET Core 3.1`. It should work on Windows, Linux and OS X.
- `HtmlAgilityPack.NetCore` is used for problem download.

## Running

To run the project:

1. Install .NET Core
2. Clone the repo
3. Get help with `dotnet run`
```

Usage: dotnet run [arguments]
Supported arguments:

 [year]/[day|last|all] Solve the specified problems
 [year]                Solve the whole year
 last                  Solve the last problem
 all                   Solve everything

To start working on new problems:
login to https://adventofcode.com, then copy your session cookie, and export it in your console like this 

  export SESSION=73a37e9a72a87b550ef58c590ae48a752eab56946fb7328d35857279912acaa5b32be73bf1d92186e4b250a15d9120a0

then run the app with

 update [year]/[day]   Prepares a folder for the given day, updates the input, 
                       the readme and creates a solution template.
 update last           Same as above, but for the current day. Works in December only.  

```
