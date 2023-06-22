namespace GomokuAI;

public class Board
{
    private readonly int[,] _board;
    public int Size { get; }

    public Board(int size)
    {
        this.Size = size;
        _board = new int[size, size];
    }

    public char GetPosition(int x, int y)
    {
        var position = _board[x, y];

        return position switch
        {
            0 when x is 3 or 7 or 11 && y is 3 or 7 or 11 => '+',
            0 => '.',
            1 => 'X',
            _ => '0'
        };
    }
}