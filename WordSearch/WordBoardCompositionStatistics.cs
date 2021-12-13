namespace WordSearch
{
    public class WordBoardCompositionStatistics
    {
        public bool IsProcessed { get; set; }
        public int WordCount { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BoardTotalSpaces { get; set; }
        public int BoardEmptySpaces { get; set; }
        public int LongestWordLength { get; set; }
        public int NumberOfCompositions { get; set; }
        public double BestCompositionScore { get; set; }
        public double BestCompositionPercentageSolved { get; set; }
        public int NumberOfOverlappingCharacters { get; set; }

        public override string ToString()
        {
            if (!IsProcessed) return "WordSearch has not been processed";

            var results = new List<string>
            {
                $"word count: {WordCount}",
                $"longest word length: {LongestWordLength}",
                $"board [width,height] (total spaces): [{BoardWidth},{BoardHeight}] ({BoardTotalSpaces})",
                $"number of compositions tested: {NumberOfCompositions}",
                $"best score: {BestCompositionScore: ##.##}",
                $"best percentage solved: {BestCompositionPercentageSolved:P0}",
                $"spaces left: {BoardEmptySpaces}",
                $"number of overlapping characters: {NumberOfOverlappingCharacters}"
            };

            return string.Join(Environment.NewLine, results.ToArray());
        }
    }
}
