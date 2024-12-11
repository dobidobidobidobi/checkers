
namespace CheckersLogic
{
    //pro minimax
    struct Move
    {
        public int From {  get; set; }
        public int To {  get;  set; }
        public List<int> PiecesCaptured {  get;  set; }

        public Move(int from, int to, List<int>? piecesCaptured)
        {
            From = from;
            To = to;
            PiecesCaptured = piecesCaptured;
        }
        //ulehci to praci v SubsuquentCaptures()
        public override bool Equals(object? obj)
        {
            return obj is Move move &&
                   From == move.From &&
                   To == move.To &&
                   EqualityComparer<List<int>>.Default.Equals(PiecesCaptured, move.PiecesCaptured);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To, PiecesCaptured);
        }
    }
}
