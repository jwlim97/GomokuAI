namespace GomokuAI.Engine;

public class Board
{
    private const int BoardSize = 15;
    private readonly int[,] _positions;

    /// <summary>
    /// Creates the board based on the BoardSize
    /// </summary>
    public Board()
    {
        _positions = new int[BoardSize, BoardSize];
    }

    public static int Size => BoardSize;

    /// <summary>
    /// Gets the value at the specified position on board.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns>Position fixed to board with 0 index</returns>
    public int GetPosition(int row, int column)
    {
        return _positions[row - 1, column - 1];
    }

    /// <summary>
    /// Sets the value at the specified position on the board.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="value"></param>
    public void SetPosition(int row, int column, int value)
    {
        _positions[row - 1, column - 1] = value;
    }

    /// <summary>
    /// Prints the current state of the board.
    /// </summary>
    public void PrintBoard()
    {
        // This line ensures that the characters are properly printed. Without this it will print '?'
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Console.Write("    ");
        
        for (var i = 1; i <= BoardSize; i++)
        {
            if (i < 10)
            {
                Console.Write(i.ToString().PadLeft(2) + "  ");
            }
            else
            {
                Console.Write(i.ToString().PadLeft(3) + " ");
            }
        }
        
        Console.WriteLine();

        for (var i = 1; i <= BoardSize; i++)
        {
            Console.WriteLine("   " + "+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+");
            Console.Write(i.ToString().PadLeft(2));

            for (var j = 1; j <= BoardSize; j++)
            {
                var position = GetPosition(i, j);

                switch (position)
                {
                    case 1:
                        Console.Write(" | " + "●");
                        break;
                    case 2:
                        Console.Write(" | " + "○");
                        break;
                    default:
                        Console.Write(" |  ");
                        break;
                }
            }
            Console.WriteLine(" |");
        }
        Console.WriteLine("   " + "+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+");
    }
}