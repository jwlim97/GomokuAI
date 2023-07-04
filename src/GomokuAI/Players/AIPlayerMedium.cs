using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerMedium : BaseAIPlayer
{
    private const int Max = int.MaxValue;
    private const int Min = int.MinValue;

    private const int MaxDepth = 3;
    
    private int _playerNumber;
    private Board _board;
    private Gomoku _gomoku;

    public AIPlayerMedium(int playerNumber, Board board) : base(playerNumber, board)
    {
        this._playerNumber = playerNumber;
        this._board = board;
    }

    public override (int row, int column) GetMove(Gomoku gomoku)
    {
        _gomoku = gomoku;
        var (row, column) = CheckOpponentWinningMove();
        
        if (row != 0 && column != 0)
        {
            return (row, column);
        }
        
        var bestScore = Min;
        var bestRow = -1;
        var bestColumn = -1;
        
        for (var r = 1; r <= Board.Size; r++)
        {
            for (var c = 1; c <= Board.Size; c++)
            {
                if (_board.GetPosition(r, c) != 0) continue;
                
                _board.SetPosition(r, c, _playerNumber);
                
                var moveValue = Minimax(MaxDepth, r, c, false, Min, Max);
                
                Console.WriteLine($"Considering move ({r}, {c}) with score {moveValue}...");

                _board.SetPosition(r, c, 0);
                
                if (moveValue <= bestScore) continue;
                bestRow = r;
                bestColumn = c;
                bestScore = moveValue;
            }
        }
        
        Console.WriteLine($"Best move: ({bestRow}, {bestColumn}) with score {bestScore}");
        
        if (bestRow == -1 || bestColumn == -1)
            throw new Exception("No valid moves found!");

        return (bestRow, bestColumn);
    }

    private int Minimax(int depth, int row, int column, bool maximizingPlayer, int alpha, int beta)
    {
        if (depth == 0 || _gomoku.IsGameOver(row, column))
        {
            return EvaluateBoard();
        }

        if (maximizingPlayer)
        {
            var best = Min;
            
            for (var r = 1; r <= Board.Size; r++)
            {
                for (var c = 1; c <= Board.Size; c++)
                {
                    if (_board.GetPosition(r, c) != 0) continue;
                    
                    _board.SetPosition(r, c, _playerNumber);
                    
                    best = Math.Max(best, Minimax(depth - 1, r, c, false, alpha, beta));
                    
                    _board.SetPosition(r, c, 0);
                    
                    alpha = Math.Max(alpha, best);
                    if (beta <= alpha)
                        break;
                }
                if (beta <= alpha)
                    break;
            }
            return best;
        }
        else
        {
            var best = Max;
            
            for (var r = 1; r <= Board.Size; r++)
            {
                for (var c = 1; c <= Board.Size; c++)
                {
                    if (_board.GetPosition(r, c) != 0) continue;
                    
                    _board.SetPosition(r, c, 3 - _playerNumber); 
                    
                    best = Math.Min(best, Minimax(depth - 1, r, c, true, alpha, beta));
                    
                    _board.SetPosition(r, c, 0);
                    
                    beta = Math.Min(beta, best);
                    if (alpha >= beta)
                        break;
                }
                if (alpha >= beta)
                    break;
            }
            return best;
        }
    }

    private int EvaluateBoard()
    {
        var score = 0;
        
        for (var r = 1; r <= Board.Size; r++)
        {
            for (var c = 1; c <= Board.Size; c++)
            {
                if (_board.GetPosition(r, c) == 0) continue;
                
                score += (_board.GetPosition(r, c) == _playerNumber) ? 1 : -1;
            }
        }

        return score;
    }

    private (int row, int column) CheckOpponentWinningMove()
    {
        var opponentNumber = 3 - _playerNumber;
        var directions = new (int rowDirection, int columnDirection)[]
        {
            (1, 0), // Horizontal 
            (0, 1), // Vertical
            (1, 1), // Diagonal left to right
            (-1, 1) // Diagonal right to left
        };
        
        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                foreach (var direction in directions)
                {
                    var consecutiveCount = 0;
                
                    for (var i = 0; i < 5; i++)
                    {
                        var newRow = row + i * direction.rowDirection;
                        var newColumn = column + i * direction.columnDirection;
                        
                        if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size ||
                            _board.GetPosition(newRow, newColumn) != opponentNumber)
                        {
                            break;
                        }

                        consecutiveCount++;
                    }
                    
                    if (consecutiveCount != 4) continue;
                    {
                        var newRow = row + 4 * direction.rowDirection;
                        var newColumn = column + 4 * direction.columnDirection;

                        if (newRow >= 1 && newRow <= Board.Size && newColumn >= 1 && newColumn <= Board.Size && 
                            _board.GetPosition(newRow, newColumn) == 0)
                        {
                            return (newRow, newColumn);
                        }
                    }
                }
            }
        }
        
        return (0, 0);
    }
}