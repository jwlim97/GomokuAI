namespace GomokuAI.Engine;

public class Board
{
    private const int BoardSize = 15;
    private readonly int[,] _positions;

    public Board()
    {
        _positions = new int[BoardSize, BoardSize];
    }

    public static int Size => BoardSize;

    public int GetPosition(int row, int column)
    {
        return _positions[row - 1, column - 1];
    }

    public void SetPosition(int row, int column, int value)
    {
        _positions[row - 1, column - 1] = value;
    }

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