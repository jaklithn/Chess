using Chess.Extensions;

namespace Chess.Domain;

public static class BoardInitiator
{
    public static List<SquareItem> GenerateSquares(ChessEngine chessEngine)
    {
        var squareItems = new List<SquareItem>();

        foreach (var col in EnumExtensions.ToList<Column>())
        {
            foreach (var row in EnumExtensions.ToList<Row>())
            {
                var color = (int)col % 2 == (int)row % 2 ? Color.Black : Color.White;
                squareItems.Add(new SquareItem { Column = col, Row = row, Color = color, ChessEngine = chessEngine });
            }
        }

        AddPieces(squareItems);

        return squareItems;
    }

    private static void AddPieces(List<SquareItem> squareItems)
    {
        squareItems.GetSquare(Column.A, Row.Row8).CreatePiece(1, Color.Black, PieceType.Rook);
        squareItems.GetSquare(Column.B, Row.Row8).CreatePiece(2, Color.Black, PieceType.Knight);
        squareItems.GetSquare(Column.C, Row.Row8).CreatePiece(3, Color.Black, PieceType.Bishop);
        squareItems.GetSquare(Column.D, Row.Row8).CreatePiece(4, Color.Black, PieceType.Queen);
        squareItems.GetSquare(Column.E, Row.Row8).CreatePiece(5, Color.Black, PieceType.King);
        squareItems.GetSquare(Column.F, Row.Row8).CreatePiece(6, Color.Black, PieceType.Bishop);
        squareItems.GetSquare(Column.G, Row.Row8).CreatePiece(7, Color.Black, PieceType.Knight);
        squareItems.GetSquare(Column.H, Row.Row8).CreatePiece(8, Color.Black, PieceType.Rook);

        squareItems.GetSquare(Column.A, Row.Row7).CreatePiece(9, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.B, Row.Row7).CreatePiece(10, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.C, Row.Row7).CreatePiece(11, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.D, Row.Row7).CreatePiece(12, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.E, Row.Row7).CreatePiece(13, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.F, Row.Row7).CreatePiece(14, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.G, Row.Row7).CreatePiece(15, Color.Black, PieceType.Pawn);
        squareItems.GetSquare(Column.H, Row.Row7).CreatePiece(16, Color.Black, PieceType.Pawn);

        squareItems.GetSquare(Column.A, Row.Row2).CreatePiece(17, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.B, Row.Row2).CreatePiece(18, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.C, Row.Row2).CreatePiece(19, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.D, Row.Row2).CreatePiece(20, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.E, Row.Row2).CreatePiece(21, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.F, Row.Row2).CreatePiece(22, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.G, Row.Row2).CreatePiece(23, Color.White, PieceType.Pawn);
        squareItems.GetSquare(Column.H, Row.Row2).CreatePiece(24, Color.White, PieceType.Pawn);

        squareItems.GetSquare(Column.A, Row.Row1).CreatePiece(25, Color.White, PieceType.Rook);
        squareItems.GetSquare(Column.B, Row.Row1).CreatePiece(26, Color.White, PieceType.Knight);
        squareItems.GetSquare(Column.C, Row.Row1).CreatePiece(27, Color.White, PieceType.Bishop);
        squareItems.GetSquare(Column.D, Row.Row1).CreatePiece(28, Color.White, PieceType.Queen);
        squareItems.GetSquare(Column.E, Row.Row1).CreatePiece(29, Color.White, PieceType.King);
        squareItems.GetSquare(Column.F, Row.Row1).CreatePiece(30, Color.White, PieceType.Bishop);
        squareItems.GetSquare(Column.G, Row.Row1).CreatePiece(31, Color.White, PieceType.Knight);
        squareItems.GetSquare(Column.H, Row.Row1).CreatePiece(32, Color.White, PieceType.Rook);
    }
}

