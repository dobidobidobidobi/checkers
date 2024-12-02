namespace CheckersLogic
{
    internal class Piece
    {
        public List<Position> AvailableMoves = new List<Position>();
        public Position Position { get; set; }
        public Player Player { get; }
        public bool IsKing { get; set; }
        
        public bool CanCapture {  get; private set; } 
        public Color Color { get; private set; }
        public Piece(Position position, Player player)
        {
            Position = position;
            Player = player;
            IsKing = false;
            Color = player.Color;
            CanCapture = false;
        }

        public void Promote() { IsKing = true; }

        public void Move(Position newPositon) { Position = newPositon;}

        public Color GetColor() { return Player.Color; }

        public void GetMoves(Board currentBoard)
        {
            AvailableMoves.Clear();
            CanCapture = false;
            Color color = GetColor();
            if (color == Color.White)
            {
                AvailableMoves = WhiteMoves(currentBoard);
            }
            else { AvailableMoves = BlackMoves(currentBoard); }
        }

        private List<Position> WhiteMoves(Board currentBoard)
        {
            List<Position> positions = new List<Position>();
            positions = WhiteCaptures(currentBoard);
            
            if ((IsKing) && (Position.Row<7) && (!CanCapture)) 
            {
                if ((Position.Column < 7) && (currentBoard.Grid[Position.Row + 1, Position.Column + 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 1, Position.Column + 1));
                }
                if ((Position.Column > 0) && (currentBoard.Grid[Position.Row + 1, Position.Column - 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 1, Position.Column - 1));
                }        
            }

            if ((Position.Row > 0) && (!CanCapture))
            {
                if ((Position.Column < 7) && (currentBoard.Grid[Position.Row - 1, Position.Column + 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 1, Position.Column + 1));
                }
                if ((Position.Column > 0) && (currentBoard.Grid[Position.Row - 1 , Position.Column - 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 1, Position.Column - 1));
                }
            }
            

            return positions;
        }
        internal List<Position> WhiteCaptures(Board currentBoard)
        {
            List<Position> positions = new List<Position>();

            if ((IsKing) && (Position.Row < 6))
            {
                if ((Position.Column < 6) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column + 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column + 1].OccupiedBy.GetColor() == Color.Black) &&
                    (currentBoard.Grid[Position.Row + 2, Position.Column + 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 2, Position.Column + 2));
                }

                if ((Position.Column > 1) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column - 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column - 1].OccupiedBy.GetColor() == Color.Black) &&
                    (currentBoard.Grid[Position.Row + 2, Position.Column - 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 2, Position.Column - 2));
                }
            }

            if (Position.Row > 1)
            {
                if ((Position.Column < 6) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column + 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column + 1].OccupiedBy.GetColor() == Color.Black) &&
                    (currentBoard.Grid[Position.Row - 2, Position.Column + 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 2, Position.Column + 2));
                }

                if ((Position.Column > 1) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column - 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column - 1].OccupiedBy.GetColor() == Color.Black) &&
                    (currentBoard.Grid[Position.Row - 2, Position.Column - 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 2, Position.Column - 2));
                }
            }
            if (positions.Count > 0) {CanCapture = true;}
            return positions;
        }
        private List<Position> BlackMoves(Board currentBoard)
        {
            List<Position> positions = new List<Position>();
            positions = BlackCaptures(currentBoard);
            if ((IsKing) && (Position.Row > 0) && (!CanCapture))
            {
                if ((Position.Column < 7) && (currentBoard.Grid[Position.Row - 1, Position.Column + 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 1, Position.Column + 1));
                }
                if ((Position.Column > 0) && (currentBoard.Grid[Position.Row - 1, Position.Column - 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 1, Position.Column - 1));
                }
            }

            if ((Position.Row < 7) && (!CanCapture))
            {
                if ((Position.Column < 7) && (currentBoard.Grid[Position.Row + 1, Position.Column + 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 1, Position.Column + 1));
                }
                if ((Position.Column > 0) && (currentBoard.Grid[Position.Row + 1, Position.Column - 1].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 1, Position.Column - 1));
                }
            }


            return positions;
        }
        internal List<Position> BlackCaptures(Board currentBoard)
        {
            List<Position> positions = new List<Position>();

            if ((IsKing) && (Position.Row > 1))
            {
                if ((Position.Column < 6) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column + 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column + 1].OccupiedBy.GetColor() == Color.White) &&
                    (currentBoard.Grid[Position.Row - 2, Position.Column + 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 2, Position.Column + 2));
                }

                if ((Position.Column > 1) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column - 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row - 1, Position.Column - 1].OccupiedBy.GetColor() == Color.White) &&
                    (currentBoard.Grid[Position.Row - 2, Position.Column - 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row - 2, Position.Column - 2));
                }
            }

            if (Position.Row < 6)
            {
                if ((Position.Column < 6) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column + 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column + 1].OccupiedBy.GetColor() == Color.White) &&
                    (currentBoard.Grid[Position.Row + 2, Position.Column + 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 2, Position.Column + 2));
                }

                if ((Position.Column > 1) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column - 1].OccupiedBy != null) &&
                    (currentBoard.Grid[Position.Row + 1, Position.Column - 1].OccupiedBy.GetColor() == Color.White) &&
                    (currentBoard.Grid[Position.Row + 2, Position.Column - 2].IsEmpty()))
                {
                    positions.Add(new Position(Position.Row + 2, Position.Column - 2));
                }
            }
            if (positions.Count > 0) { CanCapture = true; }
            return positions;
        }

    }
}
