using GomokuAI.Factories;
using GomokuAI.Interfaces;

namespace GomokuAI.Engine;

public class Gomoku
{
    private readonly Board _board;
    private readonly IPlayer _player1;
    private readonly IPlayer _player2;
    private int _currentPlayer;
    
    /// <summary>
    /// Creates the Gomoku game with the playerTypes passed in from Configuration
    /// </summary>
    /// <param name="player1Type"></param>
    /// <param name="player2Type"></param>
    public Gomoku(int player1Type, int player2Type)
    {
        _board = new Board();
        _player1 = PlayerFactory.CreatePlayer(player1Type, 1, _board);
        _player2 = PlayerFactory.CreatePlayer(player2Type, 2, _board);
        _currentPlayer = 1;
    }

    /// <summary>
    /// The core loop of the game that determines whether or not game is finished
    /// Gets player move if player is a human
    /// Otherwise gets the AI's move
    /// Prints current move
    /// </summary>
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
                (row, column) = currentPlayer.GetMove(this);
                currentPlayer.PrintMove(row, column);
            }
            else
            {
                Console.Write($"Player {_currentPlayer}, where would you like to put your piece? (row, column): ");
                (row, column) = currentPlayer.GetMove(this);
            }
        
            SetMove(row, column);

            if (!IsGameOver(row, column)) continue;
            isGameFinished = true;
            PrintBoard();
            Console.WriteLine($"Player {_currentPlayer} wins!");
        }
    }

    /// <summary>
    /// Gets currentPlayer to make sure who the current player is
    /// </summary>
    /// <returns>The current player, be it player1 or player2 (AI or not)</returns>
    private IPlayer GetCurrentPlayer()
    {
        return _currentPlayer == 1 ? _player1 : _player2;
    }
    
    /// <summary>
    /// Sets the move of based on input
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    private void SetMove(int row, int column)
    {
        _board.SetPosition(row, column, _currentPlayer);

        if (!IsGameOver(row, column))
        {
            SwapPlayer();
        }
    }

    /// <summary>
    /// Swaps currentPlayer to swap between player1 or player 2
    /// </summary>
    private void SwapPlayer()
    {
        _currentPlayer = _currentPlayer == 1 ? 2 : 1;
    }

    /// <summary>
    /// Checks the direction of the board to check for a win 9x9 space
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="direction"></param>
    /// <returns>A boolean if the game is over or not</returns>
    private bool CheckDirection(int row, int column, (int rowDirection, int columnDirection) direction)
    {
        const int winCount = 5;
        var count = 0;

        for (var i = -4; i <= 4; i++) 
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

    /// <summary>
    /// Checks if game is over
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns>The game over check calling directions</returns>
    public bool IsGameOver(int row, int column)
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