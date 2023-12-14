# Advent of Code (2015-2023)
C# solutions to the [Advent of Code](https://adventofcode.com) problems.

<a href="https://adventofcode.com"><img src="2023/calendar.svg" width="80%" /></a>

This project is best used as a template for your own AoC repository and a guide in solving
the puzzles. I put a lot of effort into my solutions. I aim for clarity which means that 
they are neither super effective or super short, but hopefully more readable.

Due to copyright requirements I'm not allowed to include my input files within this repository
so you cannot just clone it and run. However I wanted to have a self contained documentary
for myself that I can later refactor, so I decided to commit the encrypted version of the
input files. It doesn't violate the copyright since it's just random garbage for everyone else
but when I check it out, a plugin called `git-crypt` decrypts all my inputs transparently,
so I can work with them freely. On commit the whole process is reversed and the files get
encrypted again.

## Dependencies

- This project is based on `.NET 8`  and `C# 12`. 
- `AngleSharp` is used for problem download.

## Use it as a solution template:

1. Install .NET Core
2. Clone the repo
3. Remove all solution folders

```
> cd repo-dir
> rm -fr 20*
```

4. Install and initialize git-crypt:

```
> brew install git-crypt
> cd repo-dir
> git-crypt init
> git-crypt export-key ~/aoc-crypt.key
```

5. Don't commit `aoc-crypt.key` into a public repo, back it up in some protected place. 
If you need to clone your repo later you will need to unlock it using this key such as:

```
> git clone ...
> cd repo-dir
> git-crypt unlock ~/aoc-crypt.key
```

6. Get help with `dotnet run` and start coding.
```
Usage: dotnet run [arguments]
1) To run the solutions and admire your advent calendar:

 [year]/[day|all]      Solve the specified problems
 today                 Shortcut to the above
 [year]                Solve the whole year
 all                   Solve everything

 calendars             Show the calendars

2) To start working on new problems:
login to https://adventofcode.com, then copy your session cookie, and export 
it in your console like this

 export SESSION=73a37e9a72a...

then run the app with

 update [year]/[day]   Prepares a folder for the given day, updates the input,
                       the readme and creates a solution template.
 update today          Shortcut to the above.

3) To upload your answer:
set up your SESSION variable as above.

 upload [year]/[day]   Upload the answer for the selected year and day.
 upload today          Shortcut to the above.

```

## Working in Visual Studio Code
If you prefer, you can work directly in VSCode as well. 

 Open the command Palette (⇧ ⌘ P), select `Tasks: Run Task` then e.g. `update today`.

 Work on part 1. Check the solution with the `upload today` task. Continue with part 2.

 **Note:** this feature relies on the "Memento Inputs" extension to store your session cookie, you need 
 to set it up in advance from the Command Palette with `Install Extensions`.