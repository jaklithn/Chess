using System.Diagnostics.CodeAnalysis;

namespace Chess.Domain;

public class SquareItem : IEqualityComparer<SquareItem>
{
    public ChessEngine ChessEngine { get; init; }
    public Row Row { get; init; }
    public Column Column { get; init; }
    public Color Color { get; init; }
    public PieceItem Piece { get; set; }

    public string Name => Column + Row.ToString()[3..];

    public bool IsEmpty => Piece == null;
    public bool IsOccupied => Piece != null;


    public void CreatePiece(int id, Color color, PieceType pieceType)
    {
        Piece = new PieceItem { Id = id, Color = color, PieceType = pieceType, ChessEngine = ChessEngine };
    }

    public bool Equals(SquareItem x, SquareItem y)
    {
        return x.Row == y.Row && x.Column == y.Column;
    }

    public int GetHashCode([DisallowNull] SquareItem obj)
    {
        return obj.Row.GetHashCode() + obj.Column.GetHashCode();
    }
}
