namespace GomokuAI;

public class Program
{
    public static void Main(string[] args)
    {
        var board = new Board(15);

        Console.WriteLine(new string('_', board.Size * 3 - 1 + 2));
        
        for (var i = 0; i < board.Size; i++)
        {
            
            Console.Write("|");
            
            for (var j = 0; j < board.Size; j++)
            {
                Console.Write(board.GetPosition(i, j) + " ");

                if (j < board.Size - 1)
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine("|");
        }
        
        Console.WriteLine(new string('-', board.Size * 3 - 1 + 2));
        
    }
    
}