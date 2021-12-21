namespace WordSearch
{
    public class WordBoardComposition
    {
        const int maxIterations = 1000;
        private static Random rand => new();

        private readonly IEnumerable<string> _words;

        public double Score { get; set; }
        public double PercentageSolved { get; set; }

        public WordBoard Board;
        public IEnumerable<GameWord> GameWords;

        public WordBoardComposition(
            IEnumerable<string> words,
            int width,
            int height,
            IEnumerable<WordDirections> availableDirections)
        {
            _words = words;

            Board = new WordBoard(width, height, availableDirections);

            var gameWords = new List<GameWord>();
            foreach (var word in _words)
            {
                var randomPosition = new WordPosition(rand.Next(0, width), rand.Next(0, height));
                var randomDirection = availableDirections.ElementAt(rand.Next(0, availableDirections.Count()));

                gameWords.Add(new GameWord(word, randomPosition, randomDirection));
            }
            GameWords = gameWords;
        }

        public void Solve()
        {
            foreach (var word in GameWords)
            {
                int iteration = 0;
                bool done = false;

                while (!done)
                {
                    double boardFitness = Board.Fitness(word);

                    if (word.ErrorCount == 0)
                    {
                        done = true;
                        Score += boardFitness;
                        PercentageSolved++;
                        Board.PlaceWord(word);
                    }
                    else
                    {
                        Board.RandomlyRepositionWord(word);
                    }

                    if (iteration > maxIterations)
                    {
                        Score = 0.0;
                        done = true;
                    }

                    iteration++;
                }
            }

            PercentageSolved /= GameWords.Count();
        }
    }
}
