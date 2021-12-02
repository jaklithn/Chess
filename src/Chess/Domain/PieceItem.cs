namespace Chess.Domain;

public enum PieceType
{
    King,
    Queen,
    Rook,
    Knight,
    Bishop,
    Pawn
}

public enum Color
{
    Black,
    White
}

public enum Row
{
    Row1 = 1,
    Row2 = 2,
    Row3 = 3,
    Row4 = 4,
    Row5 = 5,
    Row6 = 6,
    Row7 = 7,
    Row8 = 8
}
public enum Column
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    E = 5,
    F = 6,
    G = 7,
    H = 8
}

public class PieceItem
{
    public ChessEngine ChessEngine { get; init; }
    public int Id { get; init; }
    public Color Color { get; init; }
    public PieceType PieceType { get; set; }
    public bool HasMoved { get; set; }

    public bool CanMoveTo(SquareItem square)
    {
        return ChessEngine.CanMove(this, square);
    }

    public PieceItem Copy()
    {
        return new PieceItem
        {
            ChessEngine = ChessEngine,
            Id = ChessEngine.SquareItems.Where(s => s.IsOccupied).Max(s => s.Piece.Id) + 1,
            Color = Color,
            PieceType = PieceType,
            HasMoved = HasMoved
        };
    }
}
