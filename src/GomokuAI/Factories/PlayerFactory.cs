using GomokuAI.Engine;
using GomokuAI.Interfaces;
using GomokuAI.Players;

namespace GomokuAI.Factories;

public class PlayerFactory
{
    /// <summary>
    /// A factory that creates the playerType based on user input from the Configuration
    /// </summary>
    /// <param name="playerType"></param>
    /// <param name="playerNumber"></param>
    /// <param name="board"></param>
    /// <returns>The playerType based on user input</returns>
    /// <exception cref="Exception">Throws a basic exception based on switch default</exception>
    public static IPlayer CreatePlayer(int playerType, int playerNumber, Board board)
    {
        return playerType switch
        {
            1 => new HumanPlayer(playerNumber, board),
            2 => new AIPlayerVeryEasy(playerNumber, board),
            3 => new AIPlayerEasy(playerNumber, board),
            4 => new AIPlayerMedium(playerNumber, board), 
            5 => new AIPlayerHard(playerNumber, board),
            6 => new AIPlayerVeryHard(playerNumber, board),
            _ => throw new Exception($"Wrong player type: {playerType}")
        };
    }
}