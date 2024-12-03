using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    static internal class FindBestMove
    {
        static Move Minimax(Game game, int depth)
        {
            bool maxingPlayer = CheckMaxingPlayer(game);

            //vytvareni bitboard na niz budou rychlejsi operace
            ulong blackPieces = FindBlackPieces(game.Board);
            ulong whitePieces = FindWhitePieces(game.Board);
            ulong kings = FindKings(game.Board);

            return Minimax(blackPieces, whitePieces, kings, depth, maxingPlayer);
        }

        static Move Minimax(ulong blackPieces, ulong whitePieces, ulong kings, int depth, bool maxingPlayer)
        {
            return new Move();
        }

        private static bool CheckMaxingPlayer(Game game)
        {
            bool maxingPlayer = false;

            if (game.CurrentPlayer.Color == Color.White)
            {
                maxingPlayer = true;
            }

            return maxingPlayer;
        }

        private static ulong FindBlackPieces(Board board)
        {
            ulong blackPieces = 0;
            int i = 64;
            foreach (Square square in board.Grid)
            {
                i--;
                if ((!square.IsEmpty()) && (square.OccupiedBy.Color == Color.Black))
                {
                    blackPieces |= 1UL << i;
                }
            }

            return blackPieces;
        }

        private static ulong FindWhitePieces(Board board)
        {
            ulong whitePieces = 0;
            int i = 64;
            foreach (Square square in board.Grid)
            {
                i--;
                if ((!square.IsEmpty()) && (square.OccupiedBy.Color == Color.White))
                {
                    whitePieces |= 1UL << i;
                }
            }

            return whitePieces;
        }

        private static ulong FindKings(Board board)
        {
            ulong kings = 0;
            int i = 64;
            foreach (Square square in board.Grid)
            {
                i--;
                if ((!square.IsEmpty()) && (square.OccupiedBy.IsKing))
                {
                    kings |= 1UL << i;
                }
            }

            return kings;
        }
    }
}
