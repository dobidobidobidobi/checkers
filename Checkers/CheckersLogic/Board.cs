namespace CheckersLogic
{
    class Board
    {
        public Square[,] Grid { get; private set; }

        public Board()
        {
            Grid = new Square[8, 8];


            //přiřadí všem Square správnou barvu a pozici
            for (int row =0; row < 8; row++)
            {
                for (int column =0; column < 8; column++)
                {
                    Grid[row, column] = new Square (new Position(row, column), FindSquareColorByRowAndColumn(row,column)); 
                }
            }
      
        }

        private Color FindSquareColorByRowAndColumn(int row, int column)
        {
            if ( (row + column) % 2 == 0 ) { return Color.White; }
            else { return Color.Black; }
        }

        public void InitialBoard(Player white, Player black)
        {
            for (int row = 0; row <3; row++)
            {
                for (int column = 0; column < 8 ; column++)
                {
                    if (Grid[row, column].Color == Color.Black)
                    {
                        AddPiece(row, column, black);
                    }
                }
            }

            for (int row = 5;  row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (Grid[row, column].Color == Color.Black)
                    {
                        AddPiece(row, column, white);
                    }
                }
            }

        }

        private void AddPiece(int row, int column, Player player)
        {
            Piece piece = new Piece(new Position(row, column), player);
            Grid[row, column].AddPiece(piece);
            player.Pieces.Add(piece);
            
        }

        public Square GetSquare(Position position) { return Grid[position.Row, position.Column]; }

        public void MovePiece(Position start, Position end)
        {
            Square startSquare = GetSquare(start);
            Square endSquare = GetSquare(end);
            Piece piece = startSquare.OccupiedBy;

            startSquare.RemovePiece();
            endSquare.AddPiece(piece);
            piece.Move(end);
        }

        public void RemovePiece(Position position)
        { 
            Square square = GetSquare(position);
            square.RemovePiece();
        }
         
    }
}
