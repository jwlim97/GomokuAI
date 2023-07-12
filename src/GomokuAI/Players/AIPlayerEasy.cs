using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerEasy : BaseAIPlayer
{
    // Initial values of
    // Alpha and Beta
    static int MAX = int.MaxValue;
    static int MIN = int.MinValue;

    private int _playerNumber;
    private Board _board;
    private Gomoku _gomoku;

    public AIPlayerEasy(int playerNumber, Board board) : base(playerNumber, board)
    {
        this._playerNumber = playerNumber;
        this._board = board;
    }

    // Returns optimal value for
    // current player (Initially called
    // for root and maximizer)
    private int MinMax(int depth, int row, int column, bool maximizingPlayer, int alpha, int beta)
    {
        // Terminating condition. i.e
        // leaf node is reached
        if (depth == 0 || _gomoku.IsGameOver(row, column))
        {
            return EvaluateBoard();
        }

        if (maximizingPlayer)
        {
            var best = MIN;

            // Recur for left and
            // right children
            for (var newRow = 1; newRow <= Board.Size; newRow++)
            {
                for (var newColumn = 1; newColumn <= Board.Size; newColumn++)
                {
                    if (_board.GetPosition(newRow, newColumn) == 0)
                    {
                        _board.SetPosition(newRow, newColumn, _playerNumber);

                        // Call minmax recursively and choose the maximum value
                        best = Math.Max(best, MinMax(depth - 1, newRow, newColumn, false, alpha, beta));

                        // Undo the move
                        _board.SetPosition(newRow, newColumn, 0);

                        // Alpha Beta Pruning
                        alpha = Math.Max(alpha, best);
                        if (beta <= alpha)
                            break;
                    }
                }
                if (beta <= alpha)
                    break;
            }
            return best;
        }
        else
        {
            var best = MAX;
            
            for (var newRow = 1; newRow <= Board.Size; newRow++)
            {
                for (var newColumn = 1; newColumn <= Board.Size; newColumn++)
                {
                    if (_board.GetPosition(newRow, newColumn) == 0)
                    {
                        _board.SetPosition(newRow, newColumn, 3 - _playerNumber);

                        // Call minmax recursively and choose the minimum value
                        best = Math.Min(best, MinMax(depth - 1, newRow, newColumn, true, alpha, beta));

                        // Undo the move
                        _board.SetPosition(newRow, newColumn, 0);

                        // Alpha Beta Pruning
                        beta = Math.Min(beta, best);
                        if (beta <= alpha)
                            break;
                    }
                }
                if (beta <= alpha)
                    break;
            }
            return best;
        }
    }

    public override (int row, int column) GetMove(Gomoku gomoku)
    {
        var bestValue = MIN;
        var bestRow = -1;
        var bestColumn = -1;

        // Traverse all cells, evaluate minimax function for all empty cells.
        // Return the cell with optimal value.
        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                // Check cell is empty
                if (_board.GetPosition(row, column) == 0)
                {
                    // Try the move
                    _board.SetPosition(row, column, _playerNumber);

                    // Compute evaluation function for this move.
                    var moveValue = MinMax(0, row, column, false, MIN, MAX);

                    // Undo the move
                    _board.SetPosition(row, column, 0);

                    // Update bestValue if the newly computed moveValue is better.
                    if (moveValue > bestValue)
                    {
                        bestRow = row;
                        bestColumn = column;
                        bestValue = moveValue;
                    }
                }
            }
        }

        // If no move found (which should never be the case), throw an error
        if (bestRow == -1 || bestColumn == -1)
            throw new Exception("No valid moves found!");

        return (bestRow, bestColumn);
    }
    
    /// <summary>
    /// Creates a score based on Min-Max Alpha Beta pruning 
    /// </summary>
    /// <returns>Score to push AI to a certain direction</returns>
    private int EvaluateBoard()
    {
        var score = 0;
    
        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                if (_board.GetPosition(row, column) != _playerNumber) continue;
                
                var directions = new (int rowDirection, int columnDirection)[]
                {
                    (1, 0), // Horizontal 
                    (0, 1), // Vertical
                    (1, 1), // Left to right
                    (1, -1) // Right to left
                };
                foreach (var direction in directions)
                {
                    var consecutiveCount = 1;
                    
                    for (var i = 1; i < 5; i++)
                    {
                        var newRow = row + i * direction.rowDirection;
                        var newColumn = column + i * direction.columnDirection;
                        
                        if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size ||
                            _board.GetPosition(newRow, newColumn) != _playerNumber)
                        {
                            break;
                        }

                        consecutiveCount++;
                    }

                    switch (consecutiveCount)
                    {
                        case 2:
                            score += 10;
                            break;
                        case 3:
                            score += 100;
                            break;
                        case 4:
                            score += 1000;
                            break;
                        case 5:
                            score += 10000;
                            break;
                    }
                }
            }
        }

        return score;
    }
}