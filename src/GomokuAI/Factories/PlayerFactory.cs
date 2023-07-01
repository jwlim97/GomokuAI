using GomokuAI.Engine;
using GomokuAI.Players;

namespace GomokuAI.Factories;

public class PlayerFactory
{
    public static IPlayer CreatePlayer(int playerType, int playerNumber, Board board)
    {
        return playerType switch
        {
            1 => new HumanPlayer(playerNumber, board),
            2 => new AIPlayerEasy(playerNumber, board),
            // TODO: Place holder for min-max and monte carlo
            // 3 => new AIPlayerMedium(playerNumber, board),
            // 4 => new AIPlayerHard(playerNumber, board),
            _ => throw new Exception($"Wrong player type: {playerType}")
        };
    }
}