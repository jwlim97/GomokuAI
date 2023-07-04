using GomokuAI.Engine;

namespace GomokuAI.Interfaces;

public abstract class BaseAIPlayer : IPlayer
{
    private readonly int _playerNumber;
    protected readonly Board Board;

    protected BaseAIPlayer(int playerNumber, Board board)
    {
        _playerNumber = playerNumber;
        Board = board;
    }

    public abstract (int row, int column) GetMove(Gomoku gomoku);

    public void PrintMove(int row, int column)
    {
        Console.WriteLine($"AI player {_playerNumber} chooses: ({row}, {column})");
    }
}