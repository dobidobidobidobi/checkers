namespace CheckersLogic
{
    //pro minimax
    struct Move
    {
        public int From {  get; private set; }
        public int To {  get; private set; }
        public List<int> PiecesCaptured {  get; private set; }

        public Move(int from, int to, List<int>? piecesCaptured)
        {
            From = from;
            To = to;
            PiecesCaptured = piecesCaptured;
        }

    }
}
