namespace WordSearch
{
    public class WordBoardCompositionStatistics
    {
        public bool IsProcessed { get; set; }
        public int WordCount { get; set; }
        public int PlacedWordCount {  get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public int BoardTotalSpaces { get; set; }
        public int BoardEmptySpaces { get; set; }
        public int LongestWordLength { get; set; }
        public int TotalPlacedWordLength { get; set; }
        public int NumberOfCompositions { get; set; }
        public double BestCompositionScore { get; set; }
        public double BestCompositionPercentageSolved { get; set; }
        public int NumberOfOverlappingCharacters { get; set; }
        public IEnumerable<WordDirections> AvailableWordDirections  { get; set; }
        public string? OtherInformation { get; set; }

        public override string ToString()
        {
            if (!IsProcessed) return "WordSearch has not been processed";

            var results = new List<string>
            {
                $"total word count: {WordCount}",
                $"longest word length: {LongestWordLength}",
                $"number of compositions tested: {NumberOfCompositions}",
                $"best composition score: {BestCompositionScore: ##.##}",
                $"best percentage solved: {BestCompositionPercentageSolved:P0}",
                $"board [height,width]: [{BoardHeight},{BoardWidth}]",
                $"board total spaces: {BoardTotalSpaces}",
                $"placed words count: {PlacedWordCount}",
                $"placed words character count: {TotalPlacedWordLength}",
                $"number of overlapping characters: {NumberOfOverlappingCharacters}",
                $"spaces remaining: {BoardEmptySpaces}",
                $"available word directions: {string.Join(", ", AvailableWordDirections.Select(d => d.ToString()))}"
            };

            if (!string.IsNullOrEmpty(OtherInformation))
            {
                results.Add($"other information: {OtherInformation}");
            }

            return string.Join(Environment.NewLine, results.ToArray());
        }
    }
}
