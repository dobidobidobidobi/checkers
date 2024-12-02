namespace CheckersLogic
{
    class Player
    {
        public Color Color { get; }
        public List <Piece> Pieces { get; private set; }

        public Player(Color color)
        {
            this.Color = color;
            this.Pieces = new List<Piece>();
        }

        public bool HasPiecesLeft() { return Pieces.Count > 0;}

        public void RemovePiece(Piece piece)
        {
            Pieces.Remove(piece);
        }
    }
}
