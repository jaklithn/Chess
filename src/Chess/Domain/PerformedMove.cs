namespace Chess.Domain;

public class PerformedMove : PotentialMove
{
    public DateTimeOffset DateTime { get; init; }

    public PerformedMove(PieceItem piece, SquareItem start, SquareItem end, PieceItem removedPiece, int value) : base(piece, start, end, removedPiece, value)
    {
        DateTime = DateTimeOffset.UtcNow;
        Piece = piece.Copy();
        Start = start;
        End = end;
        RemovedPiece = removedPiece;
        Value = value;
    }
}

