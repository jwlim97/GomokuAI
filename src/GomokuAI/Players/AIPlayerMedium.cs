using GomokuAI.Engine;
using GomokuAI.Interfaces;
using System.Data.Common;

namespace GomokuAI.Players
{
    public class AIPlayerMedium : MinMax
    {
        public AIPlayerMedium(int playerNumber, Board board) : base(playerNumber, board)
        {
        }
    }
}