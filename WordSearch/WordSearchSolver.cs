using System.Text.RegularExpressions;

namespace WordSearch
{
    /// <summary>
    /// With much guidance following the example from repl.it/@blonkm/wordsearch
    /// </summary>
    public class WordSearchSolver
    {
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
                var convertedWord = Regex.Replace(word, "\\s+", "").ToUpper();
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
            : this(words, squareSize, 100, availableDirections)
        { }

        public WordSearchSolver(IEnumerable<string> words, int squareSize)
            : this(words, squareSize, 100, Enum.GetValues<WordDirections>().ToList())
        { }

        public WordSearchSolver(IEnumerable<string> words, IEnumerable<WordDirections> availableDirections)
            : this(words, words.Select(w => Regex.Replace(w, "\\s+", "")).Max(w => w.Length), availableDirections)
        { }

        public WordSearchSolver(IEnumerable<string> words)
            : this(words, words.Select(w => Regex.Replace(w, "\\s+", "")).Max(w => w.Length), Enum.GetValues<WordDirections>().ToList())
        { }

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
            var spacesRemaining = _bestComposition.Board.SpacesRemaining();

            var overlappingCharacterCount = spacesRemaining
                + _bestComposition.GameWords.Where(w => w.IsPlaced).Sum(pw => pw.Text.Length)
                - (totalSpaces);

            return new WordBoardCompositionStatistics
            {
                IsProcessed = true,
                WordCount = _words.Count(),
                BoardWidth = _width,
                BoardHeight = _height,
                LongestWordLength = _words.Max(w => w.Length),
                BoardTotalSpaces = totalSpaces,
                BoardEmptySpaces = spacesRemaining,
                NumberOfCompositions = _numberOfCompositions,
                BestCompositionScore = _bestComposition.Score,
                BestCompositionPercentageSolved = _bestComposition.PercentageSolved,
                NumberOfOverlappingCharacters = overlappingCharacterCount
            };
        }

        public IEnumerable<string> GetAllWordPositions()
        {
            if (_bestComposition == null) return Enumerable.Empty<string>();

            var results = new List<string>();
            var orderedWords = _originalWordList.ToList();
            orderedWords.Sort((a, b) => a.CompareTo(b));

            foreach (var word in orderedWords)
            {
                results.Add($"\"{word}\" {_bestComposition.GameWords.First(gw => gw.Text == _wordsConversions[word])}");
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
