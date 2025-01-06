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

            double eval = maxingPlayer ? -10000000 : 10000000;

            List<Move> moves = GetMoves(maxingPlayer, blackPieces, whitePieces, kings);
            
            Move bestMove = new Move();

            double alpha = -1000000;
            double beta = 1000000;

            foreach (Move move in moves)
            {
                double moveEval = MakeAMove(blackPieces, whitePieces, kings, depth, maxingPlayer, move, alpha, beta);

                if (((moveEval > eval) && maxingPlayer) || ((moveEval < eval) && !maxingPlayer))
                {
                    eval = moveEval;
                    bestMove = move;
                }

                if (maxingPlayer) { alpha = eval; }
                else { beta = eval; }
          
            }

            return bestMove;

        }

        private static double MakeAMove
            (ulong blackPieces, ulong whitePieces, ulong kings, int depth, bool maxingPlayer, Move move, double alpha, double beta)
        {
            foreach (int pos in move.PiecesCaptured)
            {
                ulong position = 1UL << pos;
                if ((kings & position) != 0) { kings -= position; }
                if (maxingPlayer) {blackPieces -= position; }
                else {whitePieces -= position; }
            }

            if (maxingPlayer)
            {
                whitePieces -= 1UL << move.From;
                whitePieces += 1UL << move.To;
            }
            else
            {
                blackPieces -= 1UL << move.From;
                blackPieces += 1UL << move.To;
            }

            return Minimax(blackPieces, whitePieces, kings, depth, !maxingPlayer, alpha, beta);
        }

        private static double Minimax
            (ulong blackPieces, ulong whitePieces, ulong kings, int depth, bool maxingPlayer, double alpha, double beta)
        {
            List<Move> moves = GetMoves(maxingPlayer, blackPieces, whitePieces, kings);
            double best;
            if ((depth == 0) || (CheckWin(blackPieces, whitePieces, moves)))
            {
                return Evaluation(whitePieces, blackPieces, kings);
            }


            if (maxingPlayer)
            {
                best = -100000;

                foreach (Move move in moves)
                {
                    double eval = MakeAMove(blackPieces, whitePieces, kings, depth - 1, maxingPlayer, move, alpha, beta);
                    best = Math.Max(best, eval);

                    if (eval > beta) { break; }
                    alpha = Math.Max(alpha, best);
                }
            }

            else
            {
                best = 100000;
                foreach (Move move in moves)
                {
                    double eval = MakeAMove(blackPieces, whitePieces, kings, depth - 1, maxingPlayer, move, alpha, beta);
                    best = Math.Min(best, eval);

                    if (eval > alpha) { break; }
                    beta = Math.Min(beta, best);

                }
            }


            return best;
        }

        private static bool CheckWin(ulong blackPieces, ulong whitePieces, List<Move> moves)
        {
            if (blackPieces == 0) return true;
            if (whitePieces == 0) return true;
            if (moves.Count == 0) return true;
            
            return false;
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
            List<Move> KingMoves = KingCaptureMoves(whiteKings, blackPieces, emptySquares);
            List<Move> captures = new List<Move>();
            captures.AddRange(LeftUpwardsCaptures(whiteStandardPieces, blackPieces, emptySquares));
            captures.AddRange(RightUpwardsCaptures(whiteStandardPieces, blackPieces, emptySquares));

            List<Move> moves = new List<Move>();

            foreach (Move move in KingMoves)
            {
                moves.AddRange(SubsequentCaptures(move, maxingPlayer, true,
                    RemoveOposingPiece(blackPieces, move), RemoveAndAddEmptySquares(emptySquares, move)));
            }
            foreach (Move move in captures)
            {
                moves.AddRange(SubsequentCaptures(move, maxingPlayer, SubsuquentCapturesKingship(move, maxingPlayer),
                    RemoveOposingPiece(blackPieces, move), RemoveAndAddEmptySquares(emptySquares, move)));
            }
            return moves;
        }
        private static List<Move> KingCaptureMoves(ulong kings, ulong oposingPieces, ulong emptySquares)
        {
            List<Move> captures = LeftDownwardsCaptures(kings, oposingPieces, emptySquares);
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

            List<Move> KingMoves = KingCaptureMoves(blackKings, whitePieces, emptySquares);
            List<Move> captures = new List<Move>();
            captures.AddRange(RightDownwardsCaptures(blackStandardPieces, whitePieces, emptySquares));
            captures.AddRange(LeftDownwardsCaptures(blackStandardPieces, whitePieces, emptySquares));
            
            List<Move> moves = new List<Move>();

            foreach (Move move in KingMoves)
            {
                moves.AddRange(SubsequentCaptures(move, maxingPlayer, true,
                    RemoveOposingPiece(whitePieces, move), RemoveAndAddEmptySquares(emptySquares, move)));
            }
            foreach (Move move in captures)
            {
                moves.AddRange(SubsequentCaptures(move, maxingPlayer, SubsuquentCapturesKingship(move, maxingPlayer),
                    RemoveOposingPiece(whitePieces, move), RemoveAndAddEmptySquares(emptySquares, move)));
            }
            return moves;
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
                if (((((1UL << capture.To + 9) & oposingPieces) << 9) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To + 9);
                    submove.To += 18;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, king, RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
                if (((((1UL << capture.To + 7) & oposingPieces) << 7) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To + 7);
                    submove.To += 14;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, king, RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
                if (((((1UL << capture.To - 7) & oposingPieces) >> 7) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To - 7);
                    submove.To -= 14;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, king, RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
                if (((((1UL << capture.To - 9) & oposingPieces) >> 9) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To - 9);
                    submove.To -= 18;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, king, RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
            }

            else if (maxingPlayer)
            {
                if (((((1UL << capture.To + 9) & oposingPieces) << 9) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To + 9);
                    submove.To += 18;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, SubsuquentCapturesKingship(submove, maxingPlayer), RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
                if (((((1UL << capture.To + 7) & oposingPieces) << 7) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To + 7);
                    submove.To += 14;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, SubsuquentCapturesKingship(submove, maxingPlayer), RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
            }
            else
            {
                if (((((1UL << capture.To - 7) & oposingPieces) >> 7) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To - 7);
                    submove.To -= 14;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, SubsuquentCapturesKingship(submove, maxingPlayer), RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
                if (((((1UL << capture.To - 9) & oposingPieces) >> 9) & emptySquares) != 0)
                {
                    Move submove = capture;
                    submove.PiecesCaptured.Add(submove.To - 9);
                    submove.To -= 18;

                    moves.AddRange(SubsequentCaptures
                        (submove, maxingPlayer, SubsuquentCapturesKingship(submove, maxingPlayer), RemoveOposingPiece(oposingPieces, submove), RemoveAndAddEmptySquares(emptySquares, submove)));
                }
            }
            if (moves.Count == 0)
            {
                moves.Add(capture);
            }
            return moves;
        }

        private static ulong RemoveOposingPiece(ulong oposingPieces, Move move)
        {
            ulong capturedPiece = 1UL << move.PiecesCaptured.Last();

            return oposingPieces - capturedPiece;
        }

        private static ulong RemoveAndAddEmptySquares(ulong emptySquares, Move move)
        {
            ulong capturedPiece = 1UL << move.PiecesCaptured.Last();
            ulong vacantSquare = 1UL << (move.PiecesCaptured.Last() - (move.To - move.PiecesCaptured.Last()));
            ulong removedSquare = 1UL << move.To;   
            emptySquares += vacantSquare;
            emptySquares += capturedPiece;
            emptySquares -= removedSquare;

            return emptySquares;
        }

        private static bool SubsuquentCapturesKingship(Move move, bool maxingPlayer)
        {
            if ((move.To % 8 > 6) & (maxingPlayer)) { return true; }
            if ((move.To % 8 < 1) & (!maxingPlayer)) { return true; }
            return false;
        }

        private const double kingValue = 7;
        private const double pieceValue = 4.5;
        private const double safeKing = 0.4;
        private const double safePiece = 0.22;
        private const double centerControl = 0.02;
        private const double promotionPossibility = 0.0045;
        private const double pieceRatioConstant = 0.02;

        private const int EndgamePieceThreshold = 8;

        private static double Evaluation(ulong whitePieces, ulong blackPieces, ulong kings)
        {
            double whitePieceValue = 0;
            double blackPieceValue = 0;

            ulong whiteKings = whitePieces & kings;
            ulong whiteStandartPieces = whitePieces - whiteKings;

            ulong blackKings = blackPieces & kings;
            ulong blackStandartPieces = blackPieces - blackKings;
            

            double evaluation = 0;

            bool IsEndgame = Endgame(whitePieces, blackPieces);


            for (int i = 0; i < 64; i++)
            {
                ulong currentPosition = 1UL << i;

                if ((whiteStandartPieces & currentPosition) != 0)
                {

                    if (!IsEndgame)
                    { 
                        evaluation += IsSafePiece(i) + CenterControl(i) + PromotionPossibility(i, true); 
                    }

                    else
                    {
                        evaluation += PromotionPossibility(i, true);
                    }
                    whitePieceValue += pieceValue;

                }
                else if ((blackStandartPieces & currentPosition) != 0)
                {
                    if (!IsEndgame)
                    {
                        evaluation -= +IsSafePiece(i) + CenterControl(i) + PromotionPossibility(i, false);
                    }

                    else
                    {
                        evaluation -= PromotionPossibility(i, false);
                    }

                    blackPieceValue += pieceValue;

                }
                else if ((blackKings & currentPosition) != 0)
                {
                    if (!IsEndgame)
                    {
                        evaluation -= IsSafeKing(i) + CenterControl(i);
                    }

                    blackPieceValue += kingValue;
                }
                else if ((whiteKings & currentPosition) != 0)
                {
                    if (!IsEndgame)
                    {
                        evaluation += IsSafeKing(i) + CenterControl(i);
                    }

                    whitePieceValue += kingValue;
                }

            }
            evaluation += whitePieceValue - blackPieceValue;
            evaluation += PieceRatio(whitePieceValue, blackPieceValue);

            return evaluation;
        }

        public static bool Endgame(ulong whitePieces, ulong blackPieces)
        {

            int Count = CountBits(whitePieces | blackPieces);
            return Count <= EndgamePieceThreshold;
        }

        private static int CountBits(ulong bitboard)
        {
            int count = 0;
            while (bitboard != 0)
            {
                count += (int)(bitboard & 1); 
                bitboard >>= 1; 
            }
            return count;
        }

        private static double IsSafePiece(int position)
        {
            if ((position% 8 == 0) || (position % 8 == 7))
            {
                return safePiece;
            }

            return 0;
        }
        private static double IsSafeKing(int position)
        {
            if ((position % 8 == 0) || (position % 8 == 7))
            {
                return kingValue;
            }

            return 0;
        }

        private static double CenterControl(int position)
        {
            int row = (int) (position / 8);
            int column = position % 8;

            return (centerControl * (Math.Pow((3.5 - Math.Abs(3.5 - row)) * (3.5 - Math.Abs(3.5 - column)), 1.25)));
        }

        private static double PromotionPossibility(int position, bool white)
        {
            int row = (int)(position / 8);

            return (promotionPossibility * (Math.Pow(white ? row : (7 - row), 2)));
        }

        //aby se snažil tradit v početní převaze
        private static double PieceRatio(double whitePieceValue, double blackPieceValue)
        {
            double pieceRatio = whitePieceValue / blackPieceValue;
            if ((whitePieceValue != 0) & (blackPieceValue != 0))
            {
                if (pieceRatio > 1)
                {
                    return Math.Pow(pieceRatio, 2) * pieceRatioConstant;
                }
                if (pieceRatio < 1)
                {
                    pieceRatio = blackPieceValue / whitePieceValue;
                    return Math.Pow(pieceRatio, 2) * pieceRatioConstant * (-1);
                }
            }
            return 0;
        }



    }
}   
