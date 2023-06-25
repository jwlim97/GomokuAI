namespace GomokuAI;

public class Go
{
    private Board _board;
    private int _currentPlayer;

    public Go(int boardSize)
    {
        _board = new Board(boardSize);
        _currentPlayer = 1;
    }

    public void Play()
    {
        var isGameFinished = false;

        while (!isGameFinished)
        {
            PrintBoard();
            Console.Write($"Player {_currentPlayer}, where would you like to put your piece? (row, column): ");
            var (row, column) = GetMove();
            SetMove(row, column);

            if (!IsGameOver(row, column)) continue;
            isGameFinished = true;
            PrintBoard();
            Console.WriteLine($"Player {_currentPlayer} wins!");
        }
    }

    private (int row, int column) GetMove()
    {
        var row = -1;
        var column = -1;
        var isValid = false;

        while (!isValid)
        {
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.Write("Invalid input. Where would you like to put your piece? (row, column): ");
                continue;
            }

            var coordinates = input.Split(',');

            if (coordinates.Length != 2 || !int.TryParse(coordinates[0], out row) ||
                !int.TryParse(coordinates[1], out column))
            {
                Console.Write("Invalid input. Where would you like to put your piece? (row, column): ");
                continue;
            }

            if (row < 1 || row > _board.Size )
            {
                Console.Write("Row input is invalid. Where would you like to put your piece? (row, column): ");
                continue;
            }
               
            if (column < 1 || column > _board.Size)
            {
                Console.Write("Column input is invalid. Where would you like to put your piece? (row, column): ");
                continue;
            }

            if (_board.GetPosition(row, column) != 0)
            {
                Console.WriteLine("Invalid move. Selected position is taken.");
                Console.Write("Where would you like to put your piece? (row, column): ");
                continue;
            }
               
            isValid = true;
        }
          
        return (row, column);
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

    // Naive approach that scans entire board
    private bool IsGameOverNaive()
    {
        var winCount = 5;
        
        // Horizontal
        for (var row = 1; row <= _board.Size; row++)
        {
            var count = 0;
            
            for (var column = 1; column <= _board.Size; column++)
            {
                if (_board.GetPosition(row, column) == _currentPlayer)
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
        }
        
        // Vertical
        for (var column = 1; column <= _board.Size; column++)
        {
            var count = 0;
            
            for (var row = 1; row <= _board.Size; row++)
            {
                if (_board.GetPosition(row, column) == _currentPlayer)
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
        }

        // Left to right diagonal
        for (var row = 1; row <= _board.Size - winCount + 1; row++)
        {
            for (var column = 1; column <= _board.Size - winCount + 1; column++)
            {
                var pieceFound = true;

                for (var i = 0; i < winCount; i++)
                {
                    if (_board.GetPosition(row + i, column + i) == _currentPlayer) continue;
                    pieceFound = false;
                    break;
                }
                if (pieceFound) return true;
            }
        }
        
        // Right to left diagonal
        for (var row = 1; row <= _board.Size - winCount + 1; row++)
        {
            for (var column = winCount; column <= _board.Size; column++)
            {
                var pieceFound = true;

                for (var i = 0; i < winCount; i++)
                {
                    if (_board.GetPosition(row + i, column - i) == _currentPlayer) continue;
                    pieceFound = false;
                    break;
                }
                if (pieceFound) return true;
            }
        }

        return false;
    }

    private bool IsGameOver(int row, int column)
    {
        var winCount = 5;

        var directions = new (int rowDirection, int columnDirection)[]
        {
            (1, 0), // Horizontal 
            (0, 1), // Vertical
            (1, 1), // Left to right
            (1, -1) // Right to left
        };

        foreach (var direction in directions)
        {
            var count = 0;

            for (var i = -winCount + 1; i < winCount; i++)
            {
                var newRow = row + i * direction.rowDirection;
                var newColumn = column + i * direction.columnDirection;

                if (newRow < 1 || newRow > _board.Size || newColumn < 1 || newColumn > _board.Size)
                {
                    count = 0;
                    continue;
                }

                if (_board.GetPosition(newRow, newColumn) == _currentPlayer)
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
        }

        return false;
    }


    private void PrintBoard()
    {
        _board.PrintBoard();
    }
}