using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerVeryEasy : BaseAIPlayer
{
    public AIPlayerVeryEasy(int playerNumber, Board board) : base(playerNumber, board)
    {
    }

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