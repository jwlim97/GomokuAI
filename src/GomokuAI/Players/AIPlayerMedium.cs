using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerMedium : BaseAIPlayer
{
    private const int Max = int.MaxValue;
    private const int Min = int.MinValue;
    private const int MaxDepth = 3;
    private const int SearchDistance = 2;

    private int _playerNumber;
    private Board _board;
    private Gomoku _gomoku;
    private readonly List<(int row, int column)> _activePoints;

    public AIPlayerMedium(int playerNumber, Board board) : base(playerNumber, board)
    {
        this._playerNumber = playerNumber;
        this._board = board;
        this._activePoints = new List<(int row, int column)>();
    }

    public override (int row, int column) GetMove(Gomoku gomoku)
    {
        _gomoku = gomoku;
        UpdateActivePoints();

        // If the board is empty (i.e., no active points), return the center point
        if (_activePoints.Count == 0)
        {
            return (Board.Size / 2, Board.Size / 2);
        }

        var (row, column) = CheckOpponentWinningMove();

        if (row != 0 || column != 0)
        {
            return (row, column);
        }

        var bestScore = Min;
        var bestRow = -1;
        var bestColumn = -1;

        foreach (var (newRow, newColumn) in _activePoints)
        {
            if (_board.GetPosition(newRow, newColumn) != 0) continue;

            _board.SetPosition(newRow, newColumn, _playerNumber);

            var moveValue = Minimax(MaxDepth, newRow, newColumn, false, Min, Max);

            _board.SetPosition(newRow, newColumn, 0);

            if (moveValue <= bestScore) continue;
            bestRow = newRow;
            bestColumn = newColumn;
            bestScore = moveValue;
        }

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
            
            for (var newRow = 1; newRow <= Board.Size; newRow++)
            {
                for (var newColumn = 1; newColumn <= Board.Size; newColumn++)
                {
                    if (_board.GetPosition(newRow, newColumn) != 0) continue;
                    
                    _board.SetPosition(newRow, newColumn, _playerNumber);
                    
                    best = Math.Max(best, Minimax(depth - 1, newRow, newColumn, false, alpha, beta));
                    
                    _board.SetPosition(newRow, newColumn, 0);
                    
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
            
            for (var newRow = 1; newRow <= Board.Size; newRow++)
            {
                for (var newColumn = 1; newColumn <= Board.Size; newColumn++)
                {
                    if (_board.GetPosition(newRow, newColumn) != 0) continue;
                    
                    _board.SetPosition(newRow, newColumn, 3 - _playerNumber); 
                    
                    best = Math.Min(best, Minimax(depth - 1, newRow, newColumn, true, alpha, beta));
                    
                    _board.SetPosition(newRow, newColumn, 0);
                    
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
        
        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                if (_board.GetPosition(row, column) == 0) continue;
                
                score += (_board.GetPosition(row, column) == _playerNumber) ? 1 : -1;
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
    
    private void UpdateActivePoints()
    {
        _activePoints.Clear();
        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                if (_board.GetPosition(row, column) == 0) continue;

                for (var rowDistance = -SearchDistance; rowDistance <= SearchDistance; rowDistance++)
                {
                    for (var columnDistance = -SearchDistance; columnDistance <= SearchDistance; columnDistance++)
                    {
                        var newRow = row + rowDistance;
                        var newColumn = column + columnDistance;
                        
                        if (newRow >= 1 && newRow <= Board.Size && newColumn >= 1 && newColumn <= Board.Size && _board.GetPosition(newRow, newColumn) == 0)
                        {
                            _activePoints.Add((newRow, newColumn));
                        }
                    }
                }
            }
        }
    }
}