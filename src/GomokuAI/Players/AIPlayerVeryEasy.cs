using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerVeryEasy : BaseAIPlayer
{
    /// <summary>
    /// Constructor for Very Easy Player
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="board"></param>
    public AIPlayerVeryEasy(int playerNumber, Board board) : base(playerNumber, board)
    {
    }

    /// <summary>
    /// GetMove Implementation of Easy AI completely at random
    /// </summary>
    /// <param name="gomoku"></param>
    /// <returns>AI move</returns>
    public override (int row, int column) GetMove(Gomoku gomoku)
    {
        var random = new Random();
        int row;
        int column;

        do
        {
            row = random.Next(1, Board.Size + 1);
            column = random.Next(1, Board.Size + 1);
        } while (Board.GetPosition(row, column) != 0);

        return (row, column);
    }
}