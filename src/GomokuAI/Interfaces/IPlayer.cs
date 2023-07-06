using GomokuAI.Engine;

namespace GomokuAI.Interfaces;

public interface IPlayer
{
    public (int row, int column) GetMove(Gomoku gomoku);
    public void PrintMove(int row, int column);
}