if (args.Length == 0)
{
    Console.WriteLine("Input file not specified.");
    return;
}

var wordsFilePath = args[0];

if (!File.Exists(wordsFilePath))
{
    Console.WriteLine($"Input file could not be found: {wordsFilePath}");
    return;
}

var basePath = Path.GetDirectoryName(wordsFilePath);
var outputBaseName = Path.GetFileNameWithoutExtension(wordsFilePath);

var outputFile = $@"{basePath}\{outputBaseName} - Solution - {DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";

var words = File.ReadAllLines(wordsFilePath).ToList().Where(w => !string.IsNullOrWhiteSpace(w));

var supportedWordDirections = new List<WordSearch.WordDirections>
{
    WordSearch.WordDirections.Right,
    WordSearch.WordDirections.Down,
    WordSearch.WordDirections.DownRight,
};

var wordSearchSolver = new WordSearch.WordSearchSolver(words, supportedWordDirections);
wordSearchSolver.Solve();

var loggingMessages = new List<string>();

loggingMessages.Add(wordSearchSolver.GetBestCompositionStatistics().ToString());
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetAllWordPositions());
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetBestCompositionSolutionBoard());
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetBestCompositionRandomlyFilledSpacesSolutionBoard());

loggingMessages.ToList().ForEach(s => Console.WriteLine(s));

Console.WriteLine($"Writing to file: \"{outputFile}\"");
File.WriteAllLines(outputFile, loggingMessages);
