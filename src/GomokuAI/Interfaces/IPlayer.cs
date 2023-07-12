using GomokuAI.Engine;

namespace GomokuAI.Interfaces;

public interface IPlayer
{
    /// <summary>
    /// Required implementation of GetMove
    /// </summary>
    /// <param name="gomoku"></param>
    /// <returns>The move user or AI inputted</returns>
    public (int row, int column) GetMove(Gomoku gomoku);
    
    /// <summary>
    /// Required implementation of PrintMove
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void PrintMove(int row, int column);
}