using GomokuAI.Engine;
using GomokuAI.Interfaces;
using GomokuAI.Players;

namespace GomokuAI.Factories;

public class PlayerFactory
{
    public static IPlayer CreatePlayer(int playerType, int playerNumber, Board board)
    {
        return playerType switch
        {
            1 => new HumanPlayer(playerNumber, board),
            2 => new AIPlayerVeryEasy(playerNumber, board),
            3 => new AIPlayerEasy(playerNumber, board),
            4 => new AIPlayerMedium(playerNumber, board),
            // TODO: Place holder for monte carlo
            // 5 => new AIPlayerHard(playerNumber, board),
            _ => throw new Exception($"Wrong player type: {playerType}")
        };
    }
}