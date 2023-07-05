using System.Diagnostics;
using GomokuAI.Engine;
using GomokuAI.Interfaces;

namespace GomokuAI.Players;

public class AIPlayerHard : BaseAIPlayer
{
    private const int Max = int.MaxValue;
    private const int Min = int.MinValue;
    private const int MaxDepth = 3;
    private const int SearchDistance = 2;

    private int _playerNumber;
    private Board _board;
    private Gomoku _gomoku;
    private List<(int row, int column)> _activePoints;

    public AIPlayerHard(int playerNumber, Board board) : base(playerNumber, board)
    {
        this._playerNumber = playerNumber;
        this._board = board;
        this._activePoints = new List<(int row, int column)>();
    }

    public override (int row, int column) GetMove(Gomoku gomoku)
    {
        _gomoku = gomoku;
    
        _activePoints = GetNearbyEmptyPoints(SearchDistance).ToList();

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
        var watch = Stopwatch.StartNew();  
    
        _activePoints = MoveOrder(_activePoints);

        for (var depth = 1; depth <= MaxDepth; depth++)
        {
            foreach (var (newRow, newColumn) in _activePoints)
            {
                if (_board.GetPosition(newRow, newColumn) != 0) continue;

                _board.SetPosition(newRow, newColumn, _playerNumber);

                var moveValue = MinMax(depth, newRow, newColumn, false, Min, Max);

                _board.SetPosition(newRow, newColumn, 0);

                if (moveValue <= bestScore) continue;
                bestRow = newRow;
                bestColumn = newColumn;
                bestScore = moveValue;
            }

            var elapsedMs = watch.ElapsedMilliseconds;

            if (elapsedMs > 15000) 
            {
                break;
            }

            Console.WriteLine($"Depth {depth} completed in {elapsedMs} ms"); 
        }
    
        watch.Stop();

        if (bestRow == -1 || bestColumn == -1)
            throw new Exception("No valid moves found!");

        return (bestRow, bestColumn);
    }


    private int MinMax(int depth, int row, int column, bool maximizingPlayer, int alpha, int beta)
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
                    
                    best = Math.Max(best, MinMax(depth - 1, newRow, newColumn, false, alpha, beta));
                    
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
                    
                    best = Math.Min(best, MinMax(depth - 1, newRow, newColumn, true, alpha, beta));
                    
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
            
                if (_board.GetPosition(row, column) == _playerNumber)
                {
                    // Checks AI's winning sequence to be added to score
                    score += CheckSequence(row, column, 1, 0) * 100;  // Horizontal
                    score += CheckSequence(row, column, 0, 1) * 100;  // Vertical
                    score += CheckSequence(row, column, 1, 1) * 100;  // Left to right diagonal
                    score += CheckSequence(row, column, -1, 1) * 100; // Right to left diagonal
                }
                else
                {
                    // Checks opponents winning sequence to be subtracted from score
                    score -= CheckSequence(row, column, 1, 0) * 100;  // Horizontal
                    score -= CheckSequence(row, column, 0, 1) * 100;  // Vertical
                    score -= CheckSequence(row, column, 1, 1) * 100;  // Left to right diagonal
                    score -= CheckSequence(row, column, -1, 1) * 100; // Right to left diagonal
                }
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

    // Using Euclidean distance to help calculate which moves should be done
    private static double DistanceToCenter((int row, int column) move)
    {
        var center = Board.Size / 2.0;
        return Math.Sqrt(Math.Pow(move.row - center, 2) + Math.Pow(move.column - center, 2));
    }

    private List<(int row, int column)> MoveOrder(IEnumerable<(int row, int column)> moves)
    {
        return moves.OrderBy(move => 
        {
            _board.SetPosition(move.row, move.column, _playerNumber);
            var score = EvaluateBoard();
            _board.SetPosition(move.row, move.column, 0); 
            var distance = DistanceToCenter(move);
            return score - distance; 
        }).ToList();
    }

    
    private int CheckSequence(int row, int column, int rowDirection, int columnDirection)
    {
        var playerNumber = _board.GetPosition(row, column);

        for (var i = 1; i < 4; i++)
        {
            var newRow = row + i * rowDirection;
            var newColumn = column + i * columnDirection;

            if (newRow < 1 || newRow > Board.Size || newColumn < 1 || newColumn > Board.Size || _board.GetPosition(newRow, newColumn) != playerNumber)
            {
                return 0;
            }
        }

        return 1;
    }
    
    private IEnumerable<(int, int)> GetNearbyEmptyPoints(int searchDistance)
    {
        var nearbyEmptyPoints = new HashSet<(int, int)>();

        for (var row = 1; row <= Board.Size; row++)
        {
            for (var column = 1; column <= Board.Size; column++)
            {
                if (_board.GetPosition(row, column) == 0) continue; 

                for (var x = Math.Max(1, row - searchDistance); x <= Math.Min(Board.Size, row + searchDistance); x++)
                {
                    for (var y = Math.Max(1, column - searchDistance); y <= Math.Min(Board.Size, column + searchDistance); y++)
                    {
                        if (_board.GetPosition(x, y) == 0) 
                        {
                            nearbyEmptyPoints.Add((x, y));
                        }
                    }
                }
            }
        }

        return nearbyEmptyPoints;
    }
}