using System.Text.RegularExpressions;

namespace WordSearch
{
    /// <summary>
    /// With much guidance following the example from repl.it/@blonkm/wordsearch
    /// </summary>
    public class WordSearchSolver
    {
        private const int _defaultNumberOfCompositions = 100;

        private readonly IEnumerable<string> _originalWordList;
        private readonly IEnumerable<string> _words;
        private IDictionary<string, string> _wordsConversions;
        private int _width;
        private int _height;
        private int _numberOfCompositions;
        private IEnumerable<WordBoardComposition> _compositions;
        private WordBoardComposition? _bestComposition;
        private IEnumerable<WordDirections> _availableDirections;

        public WordSearchSolver(
            IEnumerable<string> words,
            int width,
            int height,
            int numberOfCompositions,
            IEnumerable<WordDirections> availableDirections)
        {
            _originalWordList = words;

            _width = width;
            _height = height;
            _numberOfCompositions = numberOfCompositions;
            _availableDirections = availableDirections;

            _wordsConversions = new Dictionary<string, string>();
            foreach (var word in words)
            {
                var lettersOnly = Regex.Matches(word, "[A-Z]", RegexOptions.IgnoreCase).Cast<Match>().Select(m => m.Value);
                var convertedWord = string.Join("", lettersOnly).ToUpper();
                if (string.IsNullOrEmpty(convertedWord))
                {
                    continue;
                }
                _wordsConversions.Add(word, convertedWord);
            }

            _words = _wordsConversions.Values.Distinct();

            var compositions = new List<WordBoardComposition>();
            for (var compositionCount = 0; compositionCount < _numberOfCompositions; compositionCount++)
            {
                var composition = new WordBoardComposition(_words, _width, _height, _availableDirections);
                compositions.Add(composition);
            }
            _compositions = compositions;
        }

        public WordSearchSolver(IEnumerable<string> words, int squareSize, int numberOfCompositions, IEnumerable<WordDirections> availableDirections)
            : this(words, squareSize, squareSize, numberOfCompositions, availableDirections)
        { }

        public WordSearchSolver(IEnumerable<string> words, int squareSize, int numberOfCompositions)
            : this(words, squareSize, squareSize, numberOfCompositions, Enum.GetValues<WordDirections>().ToList())
        { }

        public WordSearchSolver(IEnumerable<string> words, int squareSize, IEnumerable<WordDirections> availableDirections)
            : this(words, squareSize, _defaultNumberOfCompositions, availableDirections)
        { }

        public WordSearchSolver(IEnumerable<string> words, int squareSize)
            : this(words, squareSize, _defaultNumberOfCompositions, Enum.GetValues<WordDirections>().ToList())
        { }

        public WordSearchSolver(IEnumerable<string> words, IEnumerable<WordDirections> availableDirections)
            : this(words, GetSquareSizeFromWords(words), availableDirections)
        { }

        public WordSearchSolver(IEnumerable<string> words)
            : this(words, GetSquareSizeFromWords(words), Enum.GetValues<WordDirections>().ToList())
        { }

        public WordSearchSolver(IEnumerable<string> words, GameDifficulty difficulty)
            : this(words, GetSquareSizeFromWordsAndDifficulty(words, difficulty), GetWordDirectionsFromDifficulty(difficulty))
        { }

        private static int GetSquareSizeFromWords(IEnumerable<string> words)
        {
            return Math.Max(words.Count(), words.Select(w => Regex.Replace(w, "\\s+", "")).Max(w => w.Length));
        }

        private static int GetSquareSizeFromWordsAndDifficulty(IEnumerable<string> words, GameDifficulty difficulty)
        {
            var squareSizeByWords = GetSquareSizeFromWords(words);
            return (difficulty == GameDifficulty.VeryHard)
                ? squareSizeByWords + Convert.ToInt32(Math.Round(squareSizeByWords * .25f))
                : squareSizeByWords;
        }

        private static IEnumerable<WordDirections> GetWordDirectionsFromDifficulty(GameDifficulty difficulty)
        {
            var availableDirections = new List<WordDirections>();
            switch (difficulty)
            {
                case GameDifficulty.Hard:
                case GameDifficulty.VeryHard:
                    availableDirections.AddRange(new[] { WordDirections.Left, WordDirections.UpLeft, WordDirections.DownLeft });
                    goto case GameDifficulty.Medium;
                case GameDifficulty.Medium:
                    availableDirections.AddRange(new[] { WordDirections.Up, WordDirections.DownRight, WordDirections.UpRight });
                    goto case GameDifficulty.Easy;
                case GameDifficulty.Easy:
                    availableDirections.AddRange(new[] { WordDirections.Right, WordDirections.Down });
                    break;
            }
            return availableDirections;
        }

        public void Solve()
        {
            foreach (var composition in _compositions)
            {
                // Each composition has a way to randomly attempt to solve the board
                composition.Solve();
            }

            _bestComposition = _compositions.OrderByDescending(c => c.Score).First();
        }

        public WordBoardCompositionStatistics GetBestCompositionStatistics()
        {
            if (_bestComposition == null) return new WordBoardCompositionStatistics();

            var totalSpaces = _width * _height;
            var spacesRemaining = _bestComposition.Board.GetSpacesRemaining();
            var totalPlacedWordLength = _bestComposition.GameWords.Where(w => w.IsPlaced).Sum(pw => pw.Text.Length);
            var overlappingCharacterCount = totalPlacedWordLength - _bestComposition.Board.GetSpacesOccupied();

            return new WordBoardCompositionStatistics
            {
                IsProcessed = true,
                WordCount = _words.Count(),
                PlacedWordCount = _bestComposition.GameWords.Where(w => w.IsPlaced).Count(),
                BoardWidth = _width,
                BoardHeight = _height,
                LongestWordLength = _words.Max(w => w.Length),
                TotalPlacedWordLength = totalPlacedWordLength,
                BoardTotalSpaces = totalSpaces,
                BoardEmptySpaces = spacesRemaining,
                NumberOfCompositions = _numberOfCompositions,
                BestCompositionScore = _bestComposition.Score,
                BestCompositionPercentageSolved = _bestComposition.PercentageSolved,
                NumberOfOverlappingCharacters = overlappingCharacterCount,
                AvailableWordDirections = _availableDirections
            };
        }

        public IEnumerable<string> GetWordListInOrder()
        {
            var words = _originalWordList.ToList();
            words.Sort((a, b) => a.CompareTo(b));
            return words;
        }

        public IEnumerable<string> GetPlacedWordListInOrder()
        {
            if (_bestComposition == null) return Enumerable.Empty<string>();

            var placedWords = _bestComposition.GameWords.Where(w => w.IsPlaced).Select(gw => gw.Text);

            var originalWords = _wordsConversions
                .Where(wc => placedWords.Contains(wc.Value))
                .ToList()
                .Select(kv => kv.Key)
                .ToList();

            originalWords.Sort((a, b) => a.CompareTo(b));

            return originalWords;
        }

        public IEnumerable<string> GetAllWordPositions()
        {
            if (_bestComposition == null) return Enumerable.Empty<string>();

            var results = new List<string>();
            var orderedWords = GetPlacedWordListInOrder();

            foreach (var word in orderedWords)
            {
                results.Add($"\"{word}\" {_bestComposition.GameWords.Single(gw => gw.Text == _wordsConversions[word])}");
            }

            return results;
        }

        public IEnumerable<string> GetBestCompositionSolutionBoard()
        {
            if (_bestComposition == null) return Enumerable.Empty<string>();

            var results = new List<string>();

            var board = _bestComposition.Board.GetWordBoard();
            var jaggedBoard = board.ToJaggedArray();

            for (var row = 0; row < _height; row++)
            {
                results.Add(string.Join("", jaggedBoard[row]).Replace(WordBoard.DefaultCharacter, ' '));
            }

            return results;
        }

        public IEnumerable<string> GetBestCompositionRandomlyFilledSpacesSolutionBoard()
        {
            if (_bestComposition == null) return Enumerable.Empty<string>();

            var results = new List<string>();

            var board = _bestComposition.Board.GetRandomlyFilledBoard();
            var jaggedBoard = board.ToJaggedArray();

            for (var row = 0; row < _height; row++)
            {
                results.Add(string.Join("", jaggedBoard[row]));
            }

            return results;
        }
    }
}
