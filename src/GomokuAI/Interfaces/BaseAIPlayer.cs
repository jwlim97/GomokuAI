using GomokuAI.Engine;

namespace GomokuAI.Interfaces;

public abstract class BaseAIPlayer : IPlayer
{
    private readonly int _playerNumber;
    protected readonly Board Board;

    /// <summary>
    /// BaseAIPlayer constructor
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="board"></param>
    protected BaseAIPlayer(int playerNumber, Board board)
    {
        _playerNumber = playerNumber;
        Board = board;
    }

    /// <summary>
    /// Required implementation of IPlayer
    /// </summary>
    /// <param name="gomoku"></param>
    /// <returns></returns>
    public abstract (int row, int column) GetMove(Gomoku gomoku);

    /// <summary>
    /// Prints the move of the AI
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void PrintMove(int row, int column)
    {
        Console.WriteLine($"AI player {_playerNumber} chooses: ({row}, {column})");
    }
}