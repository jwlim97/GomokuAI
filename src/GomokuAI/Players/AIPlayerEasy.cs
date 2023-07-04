using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerEasy : BaseAIPlayer
{
    // Values used for initial alpha and beta in Alpha-Beta Pruning
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

    // MinMax function with Alpha-Beta pruning.
    private int MinMax(int depth, int row, int column, bool maximizingPlayer, int alpha, int beta)
    {
        // Check if we reached the desired depth or if the game is over
        if (depth == 0 || _gomoku.IsGameOver(row, column))
        {
            // Evaluate board state
            return EvaluateBoard();
        }

        if (maximizingPlayer)
        {
            var best = MIN;

            // Iterate over all possible moves
            for (var r = 1; r <= Board.Size; r++)
            {
                for (var c = 1; c <= Board.Size; c++)
                {
                    // If the spot is empty
                    if (_board.GetPosition(r, c) == 0)
                    {
                        _board.SetPosition(r, c, _playerNumber); // Try the move

                        // Call minimax recursively and choose the maximum value
                        best = Math.Max(best, MinMax(depth - 1, r, c, false, alpha, beta));

                        // Undo the move
                        _board.SetPosition(r, c, 0);

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

            // Iterate over all possible moves
            for (var r = 1; r <= Board.Size; r++)
            {
                for (var c = 1; c <= Board.Size; c++)
                {
                    // If the spot is empty
                    if (_board.GetPosition(r, c) == 0)
                    {
                        _board.SetPosition(r, c, 3 - _playerNumber); // Try the move

                        // Call minimax recursively and choose the minimum value
                        best = Math.Min(best, MinMax(depth - 1, r, c, true, alpha, beta));

                        // Undo the move
                        _board.SetPosition(r, c, 0);

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
        for (var r = 1; r <= Board.Size; r++)
        {
            for (var c = 1; c <= Board.Size; c++)
            {
                // Check cell is empty
                if (_board.GetPosition(r, c) == 0)
                {
                    // Try the move
                    _board.SetPosition(r, c, _playerNumber);

                    // Compute evaluation function for this move.
                    var moveValue = MinMax(0, r, c, false, MIN, MAX);

                    // Undo the move
                    _board.SetPosition(r, c, 0);

                    // Update bestValue if the newly computed moveValue is better.
                    if (moveValue > bestValue)
                    {
                        bestRow = r;
                        bestColumn = c;
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