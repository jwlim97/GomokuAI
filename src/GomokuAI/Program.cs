namespace GomokuAI;

public class Program
{
    public static void Main(string[] args)
    {
        var board = new Board(15);
        
        Console.Write("    ");
        for (var i = 1; i <= board.Size; i++)
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

        for (var i = 1; i <= board.Size; i++)
        {
            Console.WriteLine("   " + "+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+");
            
            Console.Write(i.ToString().PadLeft(2));

            for (var j = 1; j <= board.Size; j++)
            {
                Console.Write(" | " + board.GetPosition(i, j));
            }

            Console.WriteLine(" |");
        }

        Console.WriteLine("   " + "+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+");
    }
}