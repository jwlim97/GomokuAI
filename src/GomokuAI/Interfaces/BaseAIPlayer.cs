using GomokuAI.Engine;
using GomokuAI.Players;

namespace GomokuAI.Interfaces;

public abstract class BaseAIPlayer : IPlayer
{
    private readonly int _playerNumber;
    protected readonly Board _board;

    protected BaseAIPlayer(int playerNumber, Board board)
    {
        _playerNumber = playerNumber;
        _board = board;
    }

    public abstract (int row, int column) GetMove();

    public void PrintMove(int row, int column)
    {
        Console.WriteLine($"AI player {_playerNumber} chooses: ({row}, {column})");
    }
}