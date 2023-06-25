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
        var position = _board[x-1, y-1];

        return position switch
        {
            0 => '.',
            1 => 'X',
            _ => '0'
        };
    }
}