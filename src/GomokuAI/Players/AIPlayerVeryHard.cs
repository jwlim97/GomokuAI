//Programmer Name: Jong Won Lim
//Date: 07/03/2023
//Description: Artificial Intelligence using Montecarlo Tree Search, and known Gomoku strategies.

using System;
using System.Collections.Generic;
using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players
{
    public class AIPlayerVeryHard : BaseAIPlayer
    {
        private const int NumberOfSimulations = 100000;
        private const double ExplorationParameter = 1.4;
        private const int BlockingPriorityScore = 100; // The higher, the more priority on defense.

        private int _playerNumber;
        private Board _board;
        private Gomoku _gomoku;

        public AIPlayerVeryHard(int playerNumber, Board board) : base(playerNumber, board)
        {
            this._playerNumber = playerNumber;
            this._board = board;
        }


        public override (int row, int column) GetMove(Gomoku gomoku)
        {
            var availableMoves = GetAvailableMoves();

            // Gets the opponent's previous move
            var opponentPreviousMove = GetOpponentPreviousMove();

            // Get available moves based on their distance to the opponent's previous moves
            availableMoves.Sort((move1, move2) =>
            {
                var distance1 = CalculateDistance(move1, opponentPreviousMove);
                var distance2 = CalculateDistance(move2, opponentPreviousMove);
                return distance1.CompareTo(distance2);
            });

            // Check if any move can block the opponent's row of 4
            foreach (var move in availableMoves)
            {
                if (HasOpponentPotentialWinningRow(move.row, move.column, 4))
                {
                    return move; // Return the move to block the opponent's row of 4
                }
            }

            // Check if any move can block the opponent's row of 3
            foreach (var move in availableMoves)
            {
                if (HasOpponentPotentialWinningRow(move.row, move.column, 3))
                {
                    return move; // Return the move to block the opponent's row of 3
                }
            }

            // Evaluate potential moves
            var bestMove = availableMoves[0];
            var bestMoveScore = int.MinValue;

            foreach (var move in availableMoves)
            {
                // Simulate the move
                _board.SetPosition(move.row, move.column, _playerNumber);

                // Check if the AI has a winning move
                if (HasPotentialWinningRow(move.row, move.column, _playerNumber, 5))
                {
                    _board.SetPosition(move.row, move.column, 0); // Undo the move
                    return move; //Winning Move
                }

                // Evaluating the game state
                var score = EvaluateGameState();

                // Check if the opponent has a potential winning row of 3
                if (HasOpponentPotentialWinningRow(move.row, move.column, 3))
                {
                    // Assign a higher score to moves that block the opponent's winning rows
                    score += BlockingPriorityScore;
                }

                // Update the best move when there exists one
                if (score > bestMoveScore)
                {
                    bestMove = move;
                    bestMoveScore = score;
                }

                // Undo the move
                _board.SetPosition(move.row, move.column, 0);
            }

            return bestMove;
        }




        private static double CalculateDistance((int row, int column) position1, (int row, int column) position2)
        {   //Prevents AI from wasting moves by placing their rocks at the corner in the beginning of the game
            int dx = position1.row - position2.row;
            int dy = position1.column - position2.column;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private (int row, int column) GetOpponentPreviousMove()
        {   //
            int opponentNumber = _playerNumber == 1 ? 2 : 1;

            // Iterate through the board to find the opponent's previous move
            for (int row = 1; row <= Board.Size; row++)
            {
                for (int column = 1; column <= Board.Size; column++)
                {
                    if (_board.GetPosition(row, column) == opponentNumber)
                    {
                        return (row, column);
                    }
                }
            }

            // Return a default move at the center of the board if opponent's previous move is not found.
            return (8, 8);
        }


        private bool HasOpponentPotentialWinningRow(int row, int column, int rowLength)
        {
            int opponentNumber = _playerNumber == 1 ? 2 : 1;

            // Check horizontal
            if (CheckDirection(row, column, (0, -1), opponentNumber, rowLength) ||
                CheckDirection(row, column, (0, 1), opponentNumber, rowLength))
            {
                return true;
            }

            // Check vertical
            if (CheckDirection(row, column, (-1, 0), opponentNumber, rowLength) ||
                CheckDirection(row, column, (1, 0), opponentNumber, rowLength))
            {
                return true;
            }

            // Check diagonals
            if (CheckDirection(row, column, (-1, -1), opponentNumber, rowLength) ||
                CheckDirection(row, column, (1, 1), opponentNumber, rowLength) ||
                CheckDirection(row, column, (-1, 1), opponentNumber, rowLength) ||
                CheckDirection(row, column, (1, -1), opponentNumber, rowLength))
            {
                return true;
            }

            return false;
        }




        private bool CheckDirection(int row, int column, (int rowDirection, int columnDirection) direction, int playerNumber, int rowLength)
        {
            int count = 1;

            // Check in the positive direction
            for (int i = 1; i < rowLength; i++)
            {
                int newRow = row + (i * direction.rowDirection);
                int newColumn = column + (i * direction.columnDirection);

                if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size)
                {
                    break;
                }

                if (_board.GetPosition(newRow, newColumn) == playerNumber)
                {
                    count++;

                    if (count == rowLength)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }

            // Check in the negative direction
            for (int i = 1; i < rowLength; i++)
            {
                int newRow = row - (i * direction.rowDirection);
                int newColumn = column - (i * direction.columnDirection);

                if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size)
                {
                    break;
                }

                if (_board.GetPosition(newRow, newColumn) == playerNumber)
                {
                    count++;

                    if (count == rowLength)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }

            return false;
        }

        private int EvaluateGameState() //Evaluation Phase
        {
            int playerNumber = _playerNumber;
            int opponentNumber = _playerNumber == 1 ? 2 : 1;

            int playerPotentialWinningRows = CountPotentialWinningRows(playerNumber);
            int opponentPotentialWinningRows = CountPotentialWinningRows(opponentNumber);

            int playerThreats = CountThreats(playerNumber);
            int opponentThreats = CountThreats(opponentNumber);

            int blockingPriority = GetBlockingPriority();
            int winningMovePriority = GetWinningMovePriority();

            int score = (playerPotentialWinningRows * winningMovePriority) + (playerThreats * 50) + blockingPriority -
                        (opponentPotentialWinningRows * 100) - (opponentThreats * 50);

            return score;
        }

        private static int GetWinningMovePriority() //Gives the highest evaluation score when the AI finds the winning move
        {
            return 500;
        }



        private int CountPotentialWinningRows(int playerNumber)
        {
            int potentialWinningRows = 0;

            for (int row = 1; row <= Board.Size; row++)
            {
                for (int column = 1; column <= Board.Size; column++)
                {
                    if (_board.GetPosition(row, column) == 0)
                    {
                        if (HasPotentialWinningRow(row, column, playerNumber, 5)) // Check for potential winning row of length 5
                        {
                            potentialWinningRows++;
                        }
                    }
                }
            }

            return potentialWinningRows;
        }

        private bool HasPotentialWinningRow(int row, int column, int playerNumber, int rowLength)
        {
            // Check for potential winning rows of different lengths
            const int minLength = 4;
            if (rowLength < minLength)
                return false;

            // Check horizontal
            if (CheckDirection(row, column, (0, -1), playerNumber, rowLength) ||
                CheckDirection(row, column, (0, 1), playerNumber, rowLength))
            {
                return true;
            }

            // Check vertical
            if (CheckDirection(row, column, (-1, 0), playerNumber, rowLength) ||
                CheckDirection(row, column, (1, 0), playerNumber, rowLength))
            {
                return true;
            }

            // Check diagonals
            if (CheckDirection(row, column, (-1, -1), playerNumber, rowLength) ||
                CheckDirection(row, column, (1, 1), playerNumber, rowLength) ||
                CheckDirection(row, column, (-1, 1), playerNumber, rowLength) ||
                CheckDirection(row, column, (1, -1), playerNumber, rowLength))
            {
                return true;
            }

            return false;
        }

        private int CountThreats(int playerNumber)
        {
            int threats = 0;

            for (int row = 1; row <= Board.Size; row++)
            {
                for (int column = 1; column <= Board.Size; column++)
                {
                    if (_board.GetPosition(row, column) == playerNumber)
                    {
                        threats += CountThreatsAtPosition(row, column, playerNumber);
                    }
                }
            }

            return threats;
        }

        private int CountThreatsAtPosition(int row, int column, int playerNumber)
        {
            int threats = 0;

            // Check horizontal
            threats += CountThreatsInDirection(row, column, (0, -1), playerNumber) +
                       CountThreatsInDirection(row, column, (0, 1), playerNumber);

            // Check vertical
            threats += CountThreatsInDirection(row, column, (-1, 0), playerNumber) +
                       CountThreatsInDirection(row, column, (1, 0), playerNumber);

            // Check diagonals
            threats += CountThreatsInDirection(row, column, (-1, -1), playerNumber) +
                       CountThreatsInDirection(row, column, (1, 1), playerNumber) +
                       CountThreatsInDirection(row, column, (-1, 1), playerNumber) +
                       CountThreatsInDirection(row, column, (1, -1), playerNumber);

            return threats;
        }

        private int CountThreatsInDirection(int row, int column, (int rowDirection, int columnDirection) direction, int playerNumber)
        {
            const int threatLength = 4;

            int threats = 0;

            // Check in the positive direction
            int count = 0;
            for (int i = 1; i < threatLength; i++)
            {
                int newRow = row + (i * direction.rowDirection);
                int newColumn = column + (i * direction.columnDirection);

                if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size)
                {
                    break;
                }

                if (_board.GetPosition(newRow, newColumn) == playerNumber)
                {
                    count++;

                    if (count == threatLength - 2)
                    {
                        threats++;
                        break;
                    }
                }
                else if (_board.GetPosition(newRow, newColumn) != 0)
                {
                    break;
                }
            }

            // Check in the negative direction
            count = 0;
            for (int i = 1; i < threatLength; i++)
            {
                int newRow = row - (i * direction.rowDirection);
                int newColumn = column - (i * direction.columnDirection);

                if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size)
                {
                    break;
                }

                if (_board.GetPosition(newRow, newColumn) == playerNumber)
                {
                    count++;

                    if (count == threatLength - 2)
                    {
                        threats++;
                        break;
                    }
                }
                else if (_board.GetPosition(newRow, newColumn) != 0)
                {
                    break;
                }
            }

            return threats;
        }


        private int GetBlockingPriority()
        {   //Using the Well known Gomoku Strategies
            const int maxPriorityScore = 100;
            const int minPriorityScore = 10;
            int blockingPriority = maxPriorityScore;

            int opponentNumber = _playerNumber == 1 ? 2 : 1;
            int playerPotentialWinningRows = CountPotentialWinningRows(_playerNumber);
            int opponentPotentialWinningRows = CountPotentialWinningRows(opponentNumber);

            // Adjust the blocking priority based on the game state
            if (opponentPotentialWinningRows > playerPotentialWinningRows)
            {
                // If the opponent has more potential winning rows, decrease the priority score
                blockingPriority = minPriorityScore;
            }
            else if (opponentPotentialWinningRows < playerPotentialWinningRows)
            {
                // If the opponent has fewer potential winning rows, increase the priority score
                blockingPriority = maxPriorityScore;
            }
            // Add more conditions and adjust the priority score as needed

            return blockingPriority;
        }



        private List<(int row, int column)> GetAvailableMoves()
        {
            // List of available moves
            var availableMoves = new List<(int row, int column)>();

            for (int row = 1; row <= Board.Size; row++)
            {
                for (int column = 1; column <= Board.Size; column++)
                {
                    if (_board.GetPosition(row, column) == 0)
                    {
                        availableMoves.Add((row, column));
                    }
                }
            }

            return availableMoves;
        }
    }
}