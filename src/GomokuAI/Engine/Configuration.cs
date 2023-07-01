namespace GomokuAI.Engine;

public class Configuration
{
    public (int, int) GetPlayerTypes()
    {
        Console.Write("Choose between PvsP (1), PvsAI (2), or AIvsAI (3): ");

        int gameMode;
        
        while (!int.TryParse(Console.ReadLine(), out gameMode) || gameMode < 1 || gameMode > 3)
        {
            Console.Write("Invalid input. Choose between PvsP (1), PvsAI (2), or AIvsAI (3): ");
        }

        int player1Type;
        int player2Type;

        switch (gameMode)
        {
            case 1:
                player1Type = 1;
                player2Type = 1;
                break;

            // GetAIDifficulty + 1 is done to ensure there's a difference between playerType's
            case 2:
                player1Type = 1;
                player2Type = GetAIDifficulty() + 1; 
                break;

            case 3:
                player1Type = GetAIDifficulty() + 1; 
                player2Type = GetAIDifficulty() + 1; 
                break;

            default:
                throw new ArgumentException("Invalid game mode.");
        }

        return (player1Type, player2Type);
    }

    private int GetAIDifficulty()
    {
        Console.Write("Choose difficulty level for AI: Easy (1), Medium (2), Hard (3): ");
        int difficulty;
        while (!int.TryParse(Console.ReadLine(), out difficulty) || difficulty < 1 || difficulty > 3)
        {
            Console.Write("Invalid input. Please enter 1, 2, or 3: ");
        }

        return difficulty; 
    }
}