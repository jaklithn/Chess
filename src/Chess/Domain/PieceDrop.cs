namespace Chess.Domain;

public class PieceDrop
{
    public PieceItem Piece { get; init; }
    public SquareItem End { get; init; }


    public PieceDrop(PieceItem piece, SquareItem end)
    {
        Piece = piece;
        End = end;
    }
}
