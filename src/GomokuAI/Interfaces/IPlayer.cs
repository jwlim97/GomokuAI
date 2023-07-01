using GomokuAI.Engine;

namespace GomokuAI.Players;

public interface IPlayer
{
    public (int row, int column) GetMove();
    public void PrintMove(int row, int column);
}