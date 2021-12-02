namespace Chess.Domain;

public static class SquareListExtensions
{
    public static SquareItem GetSquare(this List<SquareItem> squareItems, Column col, Row row)
    {
        return squareItems.Single(i => i.Column == col && i.Row == row);
    }
}
