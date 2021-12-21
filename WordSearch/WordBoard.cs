namespace WordSearch
{
    public class WordBoard
    {
        public const char DefaultCharacter = '.';
        private static Random Rand => new();

		private readonly IEnumerable<WordDirections> _availableDirections;
        private char[,] _board;

        public int Width {  get; set; }
        public int Height { get; set; }

        public WordBoard(int width, int height, IEnumerable<WordDirections> availableDirections)
        {
            Width = width;
            Height = height;
            _board = new char[Height, Width];
            _availableDirections = availableDirections;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    _board[row, column] = DefaultCharacter;
                }
            }
        }

        public double Fitness(GameWord word)
        {
            double fitness = 0.0;
            var position = word.Position.ShallowCopy();

            foreach (var ch in word.Text)
            {
                var isOutside = IsOutOfBounds(position);

                // bad: if the position is filled by another word
                // or the word doesn't fit on the board
                if (isOutside || HasPositionFilled(position, ch))
                {
                    word.ErrorCount++;
                    fitness--;
                }
                else
                {
                    fitness++;
                }

                // good: extra points if there is overlap with other words
                if (!isOutside && IsOverlapping(position, ch))
                {
                    fitness++;
                }

                position.MoveDirection(word.Direction);
            }

            return fitness / word.Text.Length;
        }

        public void PlaceWord(GameWord word)
        {
            var position = word.Position.ShallowCopy();

            foreach (char character in word.Text)
            {
                _board[position.Row, position.Column] = character;
                position.MoveDirection(word.Direction);
            }

            word.IsPlaced = true;
        }

        /// <summary>
        /// Move the word to another position and change direction
        /// </summary>
        public void RandomlyRepositionWord(GameWord word)
        {
            word.Position.Column = Rand.Next(0, Width);
            word.Position.Row = Rand.Next(0, Height);
            word.Direction = _availableDirections.ElementAt(Rand.Next(0, _availableDirections.Count()));
            word.ErrorCount = 0;
        }

        public char[,] GetWordBoard()
        {
            return _board;
        }

        public char[,] GetRandomlyFilledBoard()
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var board = new char[Height, Width];

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    var character = _board[row, column];

                    if (character == DefaultCharacter)
                    {
                        character = alphabet[Rand.Next(0, alphabet.Length - 1)];
                    }

                    board[row, column] = character;
                }
            }

            return board;
        }

        public int GetSpacesRemaining()
        {
            return (from char space in _board
                    where space == DefaultCharacter
                    select space).Count();
        }

        public int GetSpacesOccupied()
        {
            return (from char space in _board
                    where space != DefaultCharacter
                    select space).Count();
        }

        private bool IsOutOfBounds(WordPosition position)
        {
            return position.Column < 0
                || position.Column > Width - 1
                || position.Row < 0
                || position.Row > Height - 1;
        }

        private bool HasPositionFilled(WordPosition position, char checkCharacter)
        {
            char currentCharacter = _board[position.Row, position.Column];
            return !(currentCharacter == checkCharacter || currentCharacter == DefaultCharacter);
        }

        private bool IsOverlapping(WordPosition position, char checkCharacter)
        {
            char currentCharacter = _board[position.Row, position.Column];
            return currentCharacter == checkCharacter;
        }
    }
}
