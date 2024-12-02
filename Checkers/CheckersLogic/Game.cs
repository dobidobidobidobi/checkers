namespace CheckersLogic
{
    class Game
    {
        public Board Board { get; private set; }
        public Player[] Players { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public bool IsActive { get; private set; }

        public List<Piece> MoveablePieces { get; private set; }

        public Player? Winner { get; private set; }

        public Game()
        {
            Board = new Board();
            
            Players = new Player[] { new Player(Color.White),new Player(Color.Black) };
            CurrentPlayer = Players[0];
            Board.InitialBoard(Players[0], Players[1]);
            IsActive = true;

            MoveablePieces = PiecesThatCanMove();
            Winner = null;          
        }

        public void Start()
        {
            //setup zacina od cernych
            Board.InitialBoard(Players[1], Players[0]);
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer == Players[0]) { CurrentPlayer = Players[1]; }
            else { CurrentPlayer = Players[0]; }
        }

        public Color? CheckEnd()
        {
            if (!Players[0].HasPiecesLeft()) { return Color.Black; }
            else if (!Players[1].HasPiecesLeft()) { return Color.White; }
            return null;
        }

        public void ResetGame()
        {
            Board = new Board();
            Start();
            CurrentPlayer = Players[0];
            IsActive = true;
        }

        public List<Piece> PiecesThatCanMove()
        {
            List<Piece> pieces = new List<Piece>();
            bool capture = false;
            foreach (Piece piece in CurrentPlayer.Pieces)
            {
                piece.GetMoves(Board);
            }
            foreach (Piece piece in CurrentPlayer.Pieces)
            {
                if (piece.AvailableMoves.Count > 0)
                {
                    pieces.Add(piece);
                    if (piece.CanCapture) { capture = true; }
                }
            }
            if (capture)
            {
                for (int i = pieces.Count - 1; i >= 0; i--)
                {
                    if (!pieces[i].CanCapture)
                    {
                        pieces.RemoveAt(i);
                    }
                }
            }
            return pieces;
        }
        private void Capture(Piece piece)
        {
            Board.RemovePiece(piece.Position);
            piece.Player.RemovePiece(piece);
        }

        private void CheckKingship(Piece piece)
        {
            if ((piece.Color == Color.White) && (piece.Position.Row == 0))
            {
                piece.IsKing = true;
            }
            else if (piece.Position.Row == 7)
            {
                piece.IsKing = true;
            }
        }

        private Player CheckWinner()
        {
            if (!Players[0].HasPiecesLeft()) { return Players[1]; }
            if (!Players[1].HasPiecesLeft()) { return Players[0]; }
            if (MoveablePieces.Count == 0)
            {
                if (CurrentPlayer == Players[0]) { return Players[1]; }
                else { return Players[0]; }
            }
            return null;
        }

        public void MakeMove(Piece piece, Position endPosition)
        {
            MoveablePieces.Clear();

            if (piece.CanCapture)
            {
                //aritmeticky prumer mi da pozici mezi nimi
                Position positionOfCapturable = new Position((endPosition.Row + piece.Position.Row) / 2, (endPosition.Column + piece.Position.Column) / 2);
                Capture(Board.Grid[positionOfCapturable.Row, positionOfCapturable.Column].OccupiedBy);
                Board.MovePiece(piece.Position, endPosition);
                CheckKingship(piece);
                piece.GetMoves(Board);
                if (piece.CanCapture)
                {
                    MoveablePieces.Add(piece);
                }
                else
                {
                    if (CurrentPlayer == Players[0]) { CurrentPlayer = Players[1]; }
                    else { CurrentPlayer = Players[0]; }
                    MoveablePieces = PiecesThatCanMove();
                }
            }
            else 
            { 
                Board.MovePiece(piece.Position, endPosition);

                if (CurrentPlayer == Players[0]) { CurrentPlayer = Players[1]; }
                else { CurrentPlayer = Players[0]; }
                MoveablePieces = PiecesThatCanMove();
            }

            CheckKingship(piece);
            Winner = CheckWinner();           
        }
    }
}
