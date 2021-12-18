namespace WordSearch
{
    public enum WordDirections
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    /// <summary>
    /// Easy: Right and Down directions only
    /// Medium: Right, Down, Up, DownRight, UpRight directions
    /// Hard: all directions possible
    /// VeryHard: all directions and larger grid
    /// </summary>
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard,
        VeryHard
    }
}
