using GomokuAI.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GomokuAI.Interfaces
{
    public class MinMax : BaseAIPlayer
    {
        // Initial values of
        // Alpha and Beta
        static int MAX = 1000;
        static int MIN = 0;
        private int playerNumber;
        private Board board;
        private int best;

        public MinMax(int playerNumber, Board board) : base(playerNumber, board)
        {
            this.playerNumber = playerNumber;
            this.board = board;
        }

        // Returns optimal value for
        // current player (Initially called
        // for root and maximizer)
        static int minimax(int depth, int nodeIndex,
                        Boolean maximizingPlayer,
                        int[] values, int alpha,
                        int beta)
        {
            // Terminating condition. i.e
            // leaf node is reached
            if (depth == 5)
                return values[nodeIndex];

            if (maximizingPlayer)
            {
                int best = MIN;

                // Recur for left and
                // right children
                for (int i = 0; i < 4; i++)
                {
                    int val = minimax(depth + 1, nodeIndex * 2 + i,
                                    false, values, alpha, beta);
                    best = Math.Max(best, val);
                    alpha = Math.Max(alpha, best);

                    // Alpha Beta Pruning
                    if (beta <= alpha)
                        break;
                }
                return best;
            }
            else
            {
                int best = MAX;

                // Recur for left and
                // right children
                for (int i = 0; i < 4; i++)
                {

                    int val = minimax(depth + 1, nodeIndex * 2 + i,
                                    true, values, alpha, beta);
                    best = Math.Min(best, val);
                    beta = Math.Min(beta, best);

                    // Alpha Beta Pruning
                    if (alpha >= beta)
                        break;
                }
                return best;
            }
        }

        public override (int row, int column) GetMove()
        {
            //var random = new Random();
            int row;
            int column; 
            //int diagonal;

            do
            {
                row = best;
                //row = MAX;
                //row = MIN;
                column = best;
                column = MAX;
                column = MIN;
                //diagonal = MIN;
                //diagonal = MAX;
                //row = random.Next(1, Board.Size + 1);
                //column = random.Next(1, Board.Size + 1);
            } while (_board.GetPosition(row, column) != 0);

            return (row, column);
        }

        // Driver Code
        //public static void Main(String[] args)
        //{

            //int[] values = { 3, 5, 6, 9, 1, 2, 0, -1 };
            //Console.WriteLine("The optimal value is : " +
                                //minimax(0, 0, true, values, MIN, MAX));

        //}
    }
}
