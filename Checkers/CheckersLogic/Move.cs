namespace CheckersLogic
{
    class Move
    {
        public Piece Piece {  get; }
        public List<Position> Positions;
        bool IsCapture;

        Move(Piece piece, bool isCapture, Position position, Board curentBoard)
        {
            Piece = piece;
            IsCapture = isCapture;
            Positions = new List<Position>();
            Positions.Add(position);

            //hledá další možné capture
            if (IsCapture)
            {
                
                //když se takto přidá další pozice tak ji automaticky projede
                for (int i = 0; i < Positions.Count(); i++)
                {
                    int row = Positions[i].Row;
                    int column = Positions[i].Column;

                }
                     
            }

            
        }

        private List<Position> CheckMoveMan(Color color, Position position, Board currentBoard)
        {
            int row = position.Row;
            int column = position.Column;
            var moves = new List<Position>();
          
            if ((color == Color.White) && (row > 1))
            {
                if
                (
                    (column > 1)
                    &&
                    !(currentBoard.GetSquare(new Position(row-1, column-1)).IsEmpty())
                    &&
                    (currentBoard.GetSquare(new Position(row - 1, column - 1)).OccupiedBy.GetColor()==Color.Black)
                    &&
                    (currentBoard.GetSquare(new Position(row - 2, column - 2)).IsEmpty())
                )

                {
                    moves.Add(new Position(row - 2, column - 2));
                }
                if
                (
                    (column > 1)
                    &&
                    !(currentBoard.GetSquare(new Position(row - 1, column + 1)).IsEmpty())
                    &&
                    (currentBoard.GetSquare(new Position(row - 1, column + 1)).OccupiedBy.GetColor() == Color.Black)
                    &&
                    (currentBoard.GetSquare(new Position(row - 2, column + 2)).IsEmpty())
                )
                {
                    moves.Add(new Position(row - 2, column + 2));
                }

            }

            if ((color == Color.Black) && (row < 6))
            {
                if
                (
                    (column > 1)
                    &&
                    !(currentBoard.GetSquare(new Position(row + 1, column - 1)).IsEmpty())
                    &&
                    (currentBoard.GetSquare(new Position(row + 1, column - 1)).OccupiedBy.GetColor() == Color.Black)
                    &&
                    (currentBoard.GetSquare(new Position(row + 2, column - 2)).IsEmpty())
                )

                {
                    moves.Add(new Position(row + 2, column - 2));
                }
                if
                (
                    (column > 1)
                    &&
                    !(currentBoard.GetSquare(new Position(row + 1, column + 1)).IsEmpty())
                    &&
                    (currentBoard.GetSquare(new Position(row + 1, column + 1)).OccupiedBy.GetColor() == Color.Black)
                    &&
                    (currentBoard.GetSquare(new Position(row + 2, column + 2)).IsEmpty())
                )
                {
                    moves.Add(new Position(row + 2, column + 2));
                }

            }
            
            return moves;
        }


        private List<Position> CheckMoveKing(Color color, Position position, Board currentBoard)
        {
            return null;
        }


       


    }
}
