namespace Chess.Domain;

public class PotentialMove
{
    public SquareItem Start { get; init; }
    public SquareItem End { get; init; }
    public PieceItem Piece { get; init; }
    public PieceItem RemovedPiece { get; init; }
    public int Value { get; init; }

    public int RowSteps => Math.Abs(Start.Row - End.Row);
    public int ColSteps => Math.Abs(Start.Column - End.Column);
    public int ForwardRowSteps => Piece.Color == Color.White ? End.Row - Start.Row : Start.Row - End.Row;

    public string Name => $"{Piece.PieceType.ToString()[..1]} {Start.Name}:{End.Name}";

    public PotentialMove(PieceItem piece, SquareItem start, SquareItem end, PieceItem removedPiece, int value)
    {
        Piece = piece;
        Start = start;
        End = end;
        RemovedPiece = removedPiece;
        Value = value;
    }
}


