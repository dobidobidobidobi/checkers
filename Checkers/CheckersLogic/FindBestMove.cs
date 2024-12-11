using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    static internal class FindBestMove
    {
        public static Move Minimax(Game game, int depth)
        {
            bool maxingPlayer = CheckMaxingPlayer(game);

            //vytvareni bitboard na niz budou rychlejsi operace
            ulong blackPieces = FindBlackPieces(game.Board);
            ulong whitePieces = FindWhitePieces(game.Board);
            ulong kings = FindKings(game.Board);

            return Minimax(blackPieces, whitePieces, kings, depth, maxingPlayer);
        }

        private static Move Minimax(ulong blackPieces, ulong whitePieces, ulong kings, int depth, bool maxingPlayer)
        {
            List<Move> moves = GetMoves(maxingPlayer, blackPieces, whitePieces, kings);

            var random = new Random();
            int moveIndex = random.Next(moves.Count);

            return moves[moveIndex];
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

        private static List<Move> GetMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            if (maxingPlayer) { return WhiteMoves(maxingPlayer, blackPieces, whitePieces, kings); }
            return BlackMoves(maxingPlayer, blackPieces, whitePieces, kings);
        }

        private static List<Move> WhiteMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            List<Move> result = WhiteCaptureMoves(maxingPlayer, blackPieces, whitePieces, kings);
            if (result.Count > 0) { return result; }

            result = WhiteStandardMoves(maxingPlayer, blackPieces, whitePieces, kings);

            return result;
        }
        private static List<Move> WhiteCaptureMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            ulong emptySquares = GetEmptySquares(blackPieces, whitePieces);
            ulong whiteKings = whitePieces & kings;
            ulong whiteStandardPieces = whitePieces - whiteKings;
            List<Move> captures = KingCaptureMoves(whiteKings, blackPieces, emptySquares);

            captures.AddRange(LeftUpwardsCaptures(whiteStandardPieces, blackPieces, emptySquares));
            captures.AddRange(RightUpwardsCaptures(whiteStandardPieces, blackPieces, emptySquares));

            return captures;
        }
        private static List<Move> KingCaptureMoves(ulong kings, ulong oposingPieces, ulong emptySquares)
        {
            List<Move > captures = LeftDownwardsCaptures(kings, oposingPieces, emptySquares);
            captures.AddRange(LeftUpwardsCaptures(kings, oposingPieces, emptySquares));
            captures.AddRange(RightUpwardsCaptures(kings, oposingPieces, emptySquares));
            captures.AddRange(RightDownwardsCaptures(kings, oposingPieces, emptySquares));

            return captures;

        }

        private static List<Move> WhiteStandardMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            List<Move> moves = KingStandardMoves(maxingPlayer, blackPieces, whitePieces, kings);
            ulong emptySquares = GetEmptySquares(blackPieces, whitePieces);
            ulong whiteKings = whitePieces & kings;
            ulong whiteStandardPieces = whitePieces - whiteKings;

            moves.AddRange(LeftUpwardsStandardMoves(whiteStandardPieces, emptySquares));
            moves.AddRange(RightUpwardsStandardMoves(whiteStandardPieces, emptySquares));

            return moves;
        }

        private static List<Move> KingStandardMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            List<Move> kingMoves = new List<Move>();
            ulong emptySquares = GetEmptySquares(blackPieces, whitePieces);

            if (maxingPlayer) { kingMoves = AllDirectionStandardMoves(kings & whitePieces, emptySquares); }
            else { kingMoves = AllDirectionStandardMoves(kings & blackPieces, emptySquares); }

            return kingMoves;
        }

        private static List<Move> AllDirectionStandardMoves(ulong kings, ulong emptySquares)
        {
            List<Move> kingMoves = new List<Move>();
            kingMoves = LeftUpwardsStandardMoves(kings, emptySquares);
            kingMoves.AddRange(LeftDownwardsStandardMoves(kings, emptySquares));
            kingMoves.AddRange(RightDownwardsStandardMoves(kings, emptySquares));
            kingMoves.AddRange(RightUpwardsStandardMoves(kings, emptySquares));

            return kingMoves;
        }

        private static List<Move> BlackMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            List<Move> moves = BlackCaptureMoves(maxingPlayer, blackPieces, whitePieces, kings);
            if (moves.Count > 0) { return moves; }

            moves = BlackStandardMoves(maxingPlayer, blackPieces, whitePieces, kings);

            return moves;
        }

        private static List<Move> BlackCaptureMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            ulong emptySquares = GetEmptySquares(blackPieces, whitePieces);
            ulong blackKings = blackPieces & kings;
            ulong blackStandardPieces = blackPieces - blackKings;

            List<Move> captures = KingCaptureMoves(blackKings, whitePieces, emptySquares);

            captures.AddRange(RightDownwardsCaptures(blackStandardPieces, whitePieces, emptySquares));
            captures.AddRange(LeftDownwardsCaptures(blackStandardPieces, whitePieces, emptySquares));

            return captures;
        }

        private static List<Move> BlackStandardMoves(bool maxingPlayer, ulong blackPieces, ulong whitePieces, ulong kings)
        {
            List<Move> moves = KingStandardMoves(maxingPlayer, blackPieces, whitePieces, kings);
            ulong emptySquares = GetEmptySquares(blackPieces, whitePieces);
            ulong blackKings = blackPieces & kings;
            ulong blackStandardPieces = blackPieces - blackKings;

            moves.AddRange(RightDownwardsStandardMoves(blackStandardPieces, emptySquares));
            moves.AddRange(LeftDownwardsStandardMoves(blackStandardPieces, emptySquares));

            return moves;
        }

        private static ulong GetEmptySquares(ulong blackPieces, ulong whitePieces)
        {
            //vsechny bila policka
            ulong illegalSquares = 12273903644374837845;

            ulong emptySquares = ~(blackPieces | whitePieces);
            emptySquares -= illegalSquares;
            return emptySquares;
        }

        private static List<Move> LeftDownwardsCaptures(ulong pieces, ulong oposingPieces, ulong emptySquares)
        {
            ulong leftDownwardsCaptures = (((pieces >> 7) & oposingPieces) >> 7) & emptySquares;
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 64; i++)
            {
                if ((leftDownwardsCaptures & 1) == 1)
                {
                    moves.Add(new Move(i + 14, i, new List<int> { i + 7 }));
                }
                leftDownwardsCaptures >>= 1;
            }

            return moves;
        }

        private static List<Move> RightDownwardsCaptures(ulong pieces, ulong oposingPieces, ulong emptySquares)
        {
            ulong leftDownwardsCaptures = (((pieces >> 9) & oposingPieces) >> 9) & emptySquares;
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 64; i++)
            {
                if ((leftDownwardsCaptures & 1) == 1)
                {
                    moves.Add(new Move(i + 18, i, new List<int> { i + 9 }));
                }
                leftDownwardsCaptures >>= 1;
            }

            return moves;
        }

        private static List<Move> LeftUpwardsCaptures(ulong pieces, ulong oposingPieces, ulong emptySquares)
        {
            ulong leftDownwardsCaptures = (((pieces << 9) & oposingPieces) << 9) & emptySquares;
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 64; i++)
            {
                if ((leftDownwardsCaptures & 1) == 1)
                {
                    moves.Add(new Move(i - 18, i, new List<int> { i - 9 }));
                }
                leftDownwardsCaptures >>= 1;
            }

            return moves;
        }

        private static List<Move> RightUpwardsCaptures(ulong pieces, ulong oposingPieces, ulong emptySquares)
        {
            ulong leftDownwardsCaptures = (((pieces << 7) & oposingPieces) << 7) & emptySquares;
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 64; i++)
            {
                if ((leftDownwardsCaptures & 1) == 1)
                {
                    moves.Add(new Move(i - 14, i, new List<int> { i - 7 }));
                }
                leftDownwardsCaptures >>= 1;
            }

            return moves;
        }

        private static List<Move> LeftUpwardsStandardMoves(ulong pieces, ulong emptySquares)
        {
            ulong leftUpwardsMoves = (pieces << 9) & emptySquares;
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 64; i++)
            {
                if ((leftUpwardsMoves & 1) == 1)
                {
                    moves.Add(new Move(i - 9, i, new List<int>()));
                }
                leftUpwardsMoves >>= 1;
            }

            return moves;
        }

        private static List<Move> RightUpwardsStandardMoves(ulong pieces, ulong emptySquares)
        {
            ulong rightUpwardsMoves = (pieces << 7) & emptySquares;
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 64; i++)
            {
                if ((rightUpwardsMoves & 1) == 1)
                {
                    moves.Add(new Move(i - 7, i, new List<int>()));
                }
                rightUpwardsMoves >>= 1;
            }

            return moves;
        }

        private static List<Move> LeftDownwardsStandardMoves(ulong pieces, ulong emptySquares)
        {
            ulong leftUpwardsMoves = (pieces >> 7) & emptySquares;
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 64; i++)
            {
                if ((leftUpwardsMoves & 1) == 1)
                {
                    moves.Add(new Move(i + 7, i, new List<int>()));
                }
                leftUpwardsMoves >>= 1;
            }

            return moves;
        }

        private static List<Move> RightDownwardsStandardMoves(ulong pieces, ulong emptySquares)
        {
            ulong leftUpwardsMoves = (pieces >> 9) & emptySquares;
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 64; i++)
            {
                if ((leftUpwardsMoves & 1) == 1)
                {
                    moves.Add(new Move(i + 9, i, new List<int>()));
                }
                leftUpwardsMoves >>= 1;
            }

            return moves;
        }

        private static List<Move> SubsequentCaptures
            (Move capture, bool maxingPlayer, bool king, ulong oposingPieces, ulong emptySquares)
        {
            List<Move> moves = new List<Move>();
            if (king)
            {
                if ((((( 1UL << capture.To + 9) & oposingPieces) << 9) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To + 9);
                    submove.To += 18;

                }
            }

            return moves;
        }

    }
}   
