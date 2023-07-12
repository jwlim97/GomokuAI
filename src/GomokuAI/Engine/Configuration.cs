using System;

namespace GomokuAI.Engine;

public class Configuration
{
    /// <summary>
    /// Gets user input to determine if it should be PvsP, PvsAI, or AIvsAI
    /// Gets user input to determine the difficulty of the AI if AI is chosen
    /// </summary>
    /// <returns>PlayerTypes to be used by Gomoku constructor</returns>
    /// <exception cref="ArgumentException">For error validation.</exception>
    public (int, int) GetPlayerTypes()
    {
        Console.Write("   ('-.         .-') _               ('-.                       ('-.                 ('-.   \n");
        Console.Write("  ( OO ).-.    ( OO ) )            _(  OO)                    _(  OO)              _(  OO)  \n");
        Console.Write("  / . --. /,--./ ,--,'  ,----.    (,------.,--.              (,------. ,--.   ,--.(,------. \n");
        Console.Write("  | \\-.  \\ |   \\ |  |\\ '  .-./-')  |  .---'|  |.-')    .-')   |  .---'  \\  `.'  /  |  .---' \n");
        Console.Write(".-'-'  |  ||    \\|  | )|  |_( O- ) |  |    |  | OO ) _(  OO)  |  |    .-')     /   |  |     \n");
        Console.Write(" \\| |_.'  ||  .     |/ |  | .--, \\(|  '--. |  |`-' |(,------.(|  '--.(OO  \\   /   (|  '--.  \n");
        Console.Write("  |  .-.  ||  |\\    | (|  | '. (_/ |  .--'(|  '---.' '------' |  .--' |   /  /\\_   |  .--'  \n");
        Console.Write("  |  | |  ||  | \\   |  |  '--'  |  |  `---.|      |           |  `---.`-./  /.__)  |  `---. \n");
        Console.Write("  `--' `--'`--'  `--'   `------'   `------'`------'           `------'  `--'       `------' \n");

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

    /// <summary>
    /// Gets the difficulty level of the AI to be used in the playerType function
    /// </summary>
    /// <returns>Difficulty level</returns>
    private int GetAIDifficulty()
    {

        Console.Write("Choose difficulty level for AI: Very Easy (1), Easy (2), Medium (3), Hard (4), Very Hard(5): ");
        int difficulty;
        while (!int.TryParse(Console.ReadLine(), out difficulty) || difficulty < 1 || difficulty > 5)
        {
            Console.Write("Invalid input. Please enter 1, 2, 3, 4 or 5: ");
        }

        return difficulty; 
    }
}