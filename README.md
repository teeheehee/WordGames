# WordGames

This repository is an exploration of simple word games and puzzles using the most recent .NET stack.

## WordSearch

A C# library for generating a Word Search puzzle. Example usage can be found in the paired command line application project `WordSearchCmd`.

The `IEnumerable` of `string` values used to instantiate the Solver gets normalized as UPPERCASED and with spaces removed. Generating the word search puzzle is done by constructing multiple compositions, each with randomized settings to try and place the words on the grid.

By default a `WordBoard` is constructed matching the maximum string size of the normalized strings. Board size and other behavior can be controlled by using an alternate `WordSearchSolver` constructor.

## WordSearchCmd

A C# command line application that generates a Word Search puzzle from a simple text file of words terms.

# Acknowledgements

Inspiration for the `WordSearch` algorithm and structure was heavily pulled from [this solution](https://repl.it/@blonkm/wordsearch).

The `ToJaggedArray` extension method for converting `T[,]` to `T[][]` came from this [Stack Overflow response](https://stackoverflow.com/a/25995025).
