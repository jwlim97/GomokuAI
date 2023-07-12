using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;
public class HumanPlayer : IPlayer
{
    private readonly int _playerNumber;
    private readonly Board _board;

    /// <summary>
    /// Human constructor
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="board"></param>
    public HumanPlayer(int playerNumber, Board board)
    {
        _playerNumber = playerNumber;
        _board = board;
    }

    /// <summary>
    /// Human implementation of GetMove
    /// </summary>
    /// <param name="gomoku"></param>
    /// <returns>Valid human move</returns>
    public (int row, int column) GetMove(Gomoku gomoku)
    {
        return GetValidUserInput();
    }
    
    /// <summary>
    /// Human implementation of PrintMove
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void PrintMove(int row, int column)
    {
        Console.WriteLine($"Player {_playerNumber} chooses: ({row}, {column})");
    }

    /// <summary>
    /// Ensures human makes a valid move
    /// </summary>
    /// <returns>An error if error, otherwise returns the move</returns>
    private (int row, int column) GetValidUserInput()
    {
        while (true)
        {
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.Write("Invalid input. Please try again (row, column): ");
                continue;
            }

            var coordinates = input.Split(',');

            if (!IsInputFormatValid(coordinates, out var row, out var column))
            {
                Console.Write("Invalid input. Please try again (row, column): ");
                continue;
            }

            if (!IsRowValid(row))
            {
                Console.Write("Invalid input. Please try again (row, column): ");
                continue;
            }

            if (!IsColumnValid(column))
            {
                Console.Write("Invalid input. Please try again (row, column): ");
                continue;
            }

            if (!IsPositionAvailable(row, column))
            {
                Console.Write("Invalid input. Please try again (row, column): ");
                continue;
            }

            return (row, column);
        }
    }

    /// <summary>
    /// Checks format of input to ensure the correct input
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns>Returns the 2 coordinates of row and column</returns>
    private static bool IsInputFormatValid(IReadOnlyList<string> coordinates, out int row, out int column)
    {
        row = -1;
        column = -1;

        return coordinates.Count == 2 
               && int.TryParse(coordinates[0], out row) 
               && int.TryParse(coordinates[1], out column);
    }

    /// <summary>
    /// Checks for valid row
    /// </summary>
    /// <param name="row"></param>
    /// <returns>True or false</returns>
    private static bool IsRowValid(int row)
    {
        return row >= 1 && row <= Board.Size;
    }

    /// <summary>
    /// Checks for valid column
    /// </summary>
    /// <param name="column"></param>
    /// <returns>True or false</returns>
    private static bool IsColumnValid(int column)
    {
        return column >= 1 && column <= Board.Size;
    }

    /// <summary>
    /// Checks for valid position
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns>True or false based on board status</returns>
    private bool IsPositionAvailable(int row, int column)
    {
        return _board.GetPosition(row, column) == 0;
    }
}