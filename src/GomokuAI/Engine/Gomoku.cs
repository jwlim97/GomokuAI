using GomokuAI.Factories;
using GomokuAI.Interfaces;
using GomokuAI.Players;

namespace GomokuAI.Engine;

public class Gomoku
{
    private readonly Board _board;
    private readonly IPlayer _player1;
    private readonly IPlayer _player2;
    private int _currentPlayer;
    
    public Gomoku(int player1Type, int player2Type)
    {
        _board = new Board();
        _player1 = PlayerFactory.CreatePlayer(player1Type, 1, _board);
        _player2 = PlayerFactory.CreatePlayer(player2Type, 2, _board);
        _currentPlayer = 1;
    }

    public void Play()
    {
        var isGameFinished = false;

        while (!isGameFinished)
        {
            PrintBoard();

            var currentPlayer = GetCurrentPlayer();

            int row, column;
            if (currentPlayer is BaseAIPlayer)
            {
                (row, column) = currentPlayer.GetMove();
                currentPlayer.PrintMove(row, column);
            }
            else
            {
                Console.Write($"Player {_currentPlayer}, where would you like to put your piece? (row, column): ");
                (row, column) = currentPlayer.GetMove();
            }
        
            SetMove(row, column);

            if (!IsGameOver(row, column)) continue;
            isGameFinished = true;
            PrintBoard();
            Console.WriteLine($"Player {_currentPlayer} wins!");
        }
    }



    private IPlayer GetCurrentPlayer()
    {
        return _currentPlayer == 1 ? _player1 : _player2;
    }
    
    private void SetMove(int row, int column)
    {
        _board.SetPosition(row, column, _currentPlayer);

        if (!IsGameOver(row, column))
        {
            SwapPlayer();
        }
    }

    private void SwapPlayer()
    {
        _currentPlayer = _currentPlayer == 1 ? 2 : 1;
    }

    private bool CheckDirection(int row, int column, (int rowDirection, int columnDirection) direction)
    {
        const int winCount = 5;
        var count = 0;

        for (var i = -winCount + 1; i < winCount; i++)
        {
            var potentialRow = row + i * direction.rowDirection;
            var potentialColumn = column + i * direction.columnDirection;

            if (potentialRow < 1 || potentialRow > Board.Size || potentialColumn < 1 || potentialColumn > Board.Size)
            {
                count = 0;
                continue;
            }

            if (_board.GetPosition(potentialRow, potentialColumn) == _currentPlayer)
            {
                count++;

                if (count == winCount)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }

        return false;
    }
    
    private bool IsGameOver(int row, int column)
    {
        var directions = new (int rowDirection, int columnDirection)[]
        {
            (1, 0), // Horizontal 
            (0, 1), // Vertical
            (1, 1), // Left to right
            (1, -1) // Right to left
        };

        return directions.Any(direction => CheckDirection(row, column, direction));
    }
    
    private void PrintBoard()
    {
        _board.PrintBoard();
    }
}