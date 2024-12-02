namespace CheckersLogic
{ 
    class Square
    {
        public Position Position { get; }
        public Color Color { get; }
        public Piece? OccupiedBy { get; private set; }

        public Square(Position position, Color color)
        {
            Position = position;
            Color = color;
        }

        public bool IsEmpty() { return OccupiedBy == null; }
        
        public void AddPiece(Piece piece) { OccupiedBy = piece; }

        public void RemovePiece() { OccupiedBy = null; }

    }
}
