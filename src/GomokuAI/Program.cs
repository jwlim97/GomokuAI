using GomokuAI.Engine;

namespace GomokuAI;

class Program
{
    /// <summary>
    /// Creates a configuration to obtain playerTypes based on factory
    /// Creates a gomoku game based on the playerTypes
    /// And runs the game
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var configuration = new Configuration();
        var (player1Type, player2Type) = configuration.GetPlayerTypes();

        var gomoku = new Gomoku(player1Type, player2Type);
        gomoku.Play();
    }
}