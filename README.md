
# Advent of Code (2015-2020)
C# solutions to the Advent of Code problems.
Check out http://adventofcode.com.
![](demo.gif)
## Dependencies

- This project is based on `.NET 5`. It should work on Windows, Linux and OS X.
- `AngleSharp` is used for problem download.

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

  export SESSION=73a37e9a72a...

then run the app with

 update [year]/[day]   Prepares a folder for the given day, updates the input, 
                       the readme and creates a solution template.
 update last           Same as above, but for the current day. Works in December only.  

```
