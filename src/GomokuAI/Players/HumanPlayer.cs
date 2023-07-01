using GomokuAI.Engine;
using System;

namespace GomokuAI.Players
{
    public class HumanPlayer : IPlayer
    {
        private readonly int _playerNumber;
        private readonly Board _board;

        public HumanPlayer(int playerNumber, Board board)
        {
            _playerNumber = playerNumber;
            _board = board;
        }

        public (int row, int column) GetMove()
        {
            return GetValidUserInput();
        }
        
        public void PrintMove(int row, int column)
        {
            Console.WriteLine($"Player {_playerNumber} chooses: ({row}, {column})");
        }

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

        private static bool IsInputFormatValid(IReadOnlyList<string> coordinates, out int row, out int column)
        {
            row = -1;
            column = -1;

            return coordinates.Count == 2 
                   && int.TryParse(coordinates[0], out row) 
                   && int.TryParse(coordinates[1], out column);
        }

        private static bool IsRowValid(int row)
        {
            return row >= 1 && row <= Board.Size;
        }

        private static bool IsColumnValid(int column)
        {
            return column >= 1 && column <= Board.Size;
        }

        private bool IsPositionAvailable(int row, int column)
        {
            return _board.GetPosition(row, column) == 0;
        }
    }
}
