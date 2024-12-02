
namespace CheckersLogic
{
    class Position
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
        //potřebuji být schopen pozice porovnávat
        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
}
