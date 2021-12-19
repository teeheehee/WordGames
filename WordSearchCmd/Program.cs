var programName = $"{AppDomain.CurrentDomain.FriendlyName}.exe";
var difficultyLevels = string.Join(", ", Enum.GetNames(typeof(WordSearch.GameDifficulty)));
Console.WriteLine($"\n{programName} [filePath: list of words] [difficultyLevel: {difficultyLevels}]");

if (args.Length == 0)
{
    Console.WriteLine("\nExample:");
    Console.WriteLine($"\t{programName} \"C:\\temp\\words.txt\" VeryHard\n");
    return;
}

var wordsFilePath = args[0];

if (!File.Exists(wordsFilePath))
{
    Console.WriteLine($"Word list file could not be found: {wordsFilePath}");
    return;
}

var difficulty = WordSearch.GameDifficulty.Medium;

if (args.Length == 2)
{
    WordSearch.GameDifficulty requestedDifficulty;
    if (Enum.TryParse(args[1], out requestedDifficulty))
    {
        difficulty = requestedDifficulty;
    }
    else
    {
        Console.WriteLine($"Could not set difficulty with \"{args[1]}\", sticking with default difficulty setting");
    }
}

Console.WriteLine("");

var loggingMessages = new List<string>();

loggingMessages.Add("Word search being created with the following settings:");
loggingMessages.Add("");
loggingMessages.Add($"words list file: \"{wordsFilePath}\"");
loggingMessages.Add($"difficulty level: {difficulty}");
loggingMessages.Add("");

var basePath = Path.GetDirectoryName(wordsFilePath);
var outputBaseName = Path.GetFileNameWithoutExtension(wordsFilePath);

var outputFile = $@"{basePath}\{outputBaseName} - WordSearch - {DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";

var words = File.ReadAllLines(wordsFilePath).ToList().Where(w => !string.IsNullOrWhiteSpace(w));

var wordSearchSolver = new WordSearch.WordSearchSolver(words, WordSearch.GameDifficulty.VeryHard);
wordSearchSolver.Solve();

loggingMessages.Add("Board statistics:");
loggingMessages.Add("");
loggingMessages.Add(wordSearchSolver.GetBestCompositionStatistics().ToString());
loggingMessages.Add("");
loggingMessages.Add("Solutions:");
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetAllWordPositions());
loggingMessages.Add("");
loggingMessages.Add("Board with just the words:");
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetBestCompositionSolutionBoard());
loggingMessages.Add("");
loggingMessages.Add("Board with randomized filler content:");
loggingMessages.Add("");
loggingMessages.AddRange(wordSearchSolver.GetBestCompositionRandomlyFilledSpacesSolutionBoard());

loggingMessages.ToList().ForEach(s => Console.WriteLine(s));

Console.WriteLine($"Writing to file: \"{outputFile}\"");
File.WriteAllLines(outputFile, loggingMessages);
