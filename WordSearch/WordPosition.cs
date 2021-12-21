namespace WordSearch
{
    public class WordPosition
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public WordPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public void MoveDirection(WordDirections direction)
        {
            switch (direction)
            {
                case WordDirections.Up:
                    Row--;
                    break;
                case WordDirections.UpRight:
                    Row--;
                    Column--;
                    break;
                case WordDirections.Right:
                    Column++;
                    break;
                case WordDirections.DownRight:
                    Row++;
                    Column++;
                    break;
                case WordDirections.Down:
                    Row++;
                    break;
                case WordDirections.DownLeft:
                    Row++;
                    Column--;
                    break;
                case WordDirections.Left:
                    Column--;
                    break;
                case WordDirections.UpLeft:
                    Row--;
                    Column--;
                    break;
            }
        }

        public WordPosition ShallowCopy()
        {
            return (WordPosition)MemberwiseClone();
        }

        public override string ToString()
        {
            return $"column {Column + 1}, row {Row + 1}"; // 0-based array
        }
    }
}