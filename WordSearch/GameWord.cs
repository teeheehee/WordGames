namespace WordSearch
{
    public class GameWord
    {
        public string Text { get; set; }
        public WordPosition Position { get; set; }
        public WordDirections Direction { get; set; }
        public int ErrorCount { get; set; }
        public bool IsPlaced;

        public GameWord(
            string text,
            WordPosition? position = null,
            WordDirections direction = WordDirections.Right)
        {
            Text = text;
            Position = position ?? new WordPosition(0, 0);
            Direction = direction;
            ErrorCount = 0;
            IsPlaced = false;
        }

        public override string ToString()
        {
            if (!IsPlaced) return "not placed";
            return $"placed as {Text} @ {Position} going {Direction}";
        }
    }
}
