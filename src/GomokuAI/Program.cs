using GomokuAI.Engine;

namespace GomokuAI;

class Program
{
    public static void Main(string[] args)
    {
        var configuration = new Configuration();
        var (player1Type, player2Type) = configuration.GetPlayerTypes();

        var gomoku = new Gomoku(player1Type, player2Type);
        gomoku.Play();
    }
}