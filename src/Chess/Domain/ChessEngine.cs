using Chess.Extensions;
using System.Diagnostics;

namespace Chess.Domain;

public enum PlayMode
{
    Dual,
    SingleWhite,
    SingleBlack
}

public class ChessEngine
{
    public readonly List<SquareItem> SquareItems;
    public Color CurrentPlayer = Color.White;
    public Stopwatch WhiteTime = Stopwatch.StartNew();
    public Stopwatch BlackTime = Stopwatch.StartNew();

    public ChessEngine()
    {
        SquareItems = BoardInitiator.GenerateSquares(this);
        CurrentPlayer = Color.White;
        WhiteTime.Reset();
        BlackTime.Reset();
    }

    public SquareItem GetSquare(Column col, Row row)
    {
        return SquareItems.Single(s => s.Column == col && s.Row == row);
    }

    public SquareItem GetSquare(PieceItem piece)
    {
        return SquareItems.Single(s => s.Piece != null && s.Piece.Id == piece.Id);
    }

    public PerformedMove GenerateMove(Color color)
    {
        var suggestedMove = PickBestMove(color);
        return Move(suggestedMove.Piece, suggestedMove.End);
    }

    public bool CanMove(PieceItem piece, SquareItem endSquare)
    {
        if (piece.Color != CurrentPlayer)
        {
            return false;
        }

        var potentialMoves = GetPotentialMoves(piece).ToList();
        var potentialMovesWithoutHinder = potentialMoves.Where(p => HasFreeWay(p)).ToList();
        var thisSquareHasPotential = potentialMovesWithoutHinder.Any(pm => pm.End.Equals(endSquare));
        return thisSquareHasPotential;
    }

    private PotentialMove PickBestMove(Color color)
    {
        var pieces = SquareItems.Where(s => s.Piece.Color == color);
        var potentialMoves = pieces.SelectMany(s => GetPotentialMoves(s.Piece));
        var potentialMovesWithoutHinder = potentialMoves.Where(p => HasFreeWay(p));
        return potentialMoves.OrderByDescending(s => s.Value).FirstOrDefault();
    }

    public List<PotentialMove> GetPotentialMoves(PieceItem piece)
    {
        var startSquare = GetSquare(piece);
        var potentialMoves = GetAllPotentialMoves(piece, startSquare).ToList();

        switch (piece.PieceType)
        {
            case PieceType.King:
                // One step in any direction OR two step if castling is allowed
                return potentialMoves.Where(p => (p.ColSteps <= 1 && p.RowSteps <= 1) || IsCastlingMove(p)).ToList();

            case PieceType.Queen:
                // Any number of linear steps as long as it is either horizontally, vertically or diagonally
                var res = potentialMoves.Where(p => p.ColSteps == 0 || p.RowSteps == 0 || p.ColSteps == p.RowSteps).ToList();
                return res;

            case PieceType.Knight:
                // Two step in one direction and one on the other
                return potentialMoves.Where(p => p.ColSteps == 1 && p.RowSteps == 2 || p.ColSteps == 2 && p.RowSteps == 1).ToList();

            case PieceType.Bishop:
                // Diagonal
                return potentialMoves.Where(p => p.ColSteps == p.RowSteps).ToList();

            case PieceType.Rook:
                // Straight in either direction
                return potentialMoves.Where(p => p.ColSteps == 0 || p.RowSteps == 0).ToList();

            case PieceType.Pawn:
                // One step forward to empty square
                // Two steps forward to empty square if first move
                // One step forward diagonal if hitting
                var p1 = potentialMoves.Where(p => p.ColSteps == 0 && p.ForwardRowSteps == 1 && p.End.IsEmpty).ToList();
                var p2 = potentialMoves.Where(p => p.ColSteps == 0 && p.ForwardRowSteps == 2 && p.End.IsEmpty && !piece.HasMoved).ToList();
                var p3 = potentialMoves.Where(p => p.ColSteps == 1 && p.ForwardRowSteps == 1 && p.End.IsOccupied).ToList();
                return p1.Union(p2).Union(p3).ToList();

            default:
                throw new ArgumentOutOfRangeException($"PieceType={piece.PieceType} is not handled");
        }
    }

    private IEnumerable<PotentialMove> GetAllPotentialMoves(PieceItem piece, SquareItem startSquare)
    {
        var mySquares = SquareItems.Where(s => s.IsOccupied && s.Piece.Color == piece.Color).ToList();
        var potentialSquares = SquareItems.Except(mySquares).ToList();
        var potentialMoves = potentialSquares.Select(s => new PotentialMove(piece, startSquare, s, s.Piece, GetValue(s.Piece))).ToList();
        return potentialMoves;
    }


    private bool HasFreeWay(PotentialMove potentialMove)
    {
        // Knight can jump
        if (potentialMove.Piece.PieceType == PieceType.Knight)
        {
            return true;
        }

        // Straight vertical move
        if (potentialMove.ColSteps == 0 && potentialMove.RowSteps > 1)
        {
            var rowStep = potentialMove.Start.Row < potentialMove.End.Row ? 1 : -1;
            var row = potentialMove.Start.Row + rowStep;
            do
            {
                var square = GetSquare(potentialMove.Start.Column, row);
                if (square.IsOccupied)
                {
                    return false;
                }
                row += rowStep;
            } while (row != potentialMove.End.Row);
        }

        // Straight horizontal move
        if (potentialMove.RowSteps == 0 && potentialMove.ColSteps > 1)
        {
            var colStep = potentialMove.Start.Column < potentialMove.End.Column ? 1 : -1;
            var col = potentialMove.Start.Column + colStep;
            do
            {
                var square = GetSquare(col, potentialMove.Start.Row);
                if (square.IsOccupied)
                {
                    return false;
                }
                col += colStep;
            } while (col != potentialMove.End.Column);
        }

        // Straight diagonal move
        if (potentialMove.RowSteps == potentialMove.ColSteps && potentialMove.RowSteps > 1)
        {
            var colStep = potentialMove.Start.Column < potentialMove.End.Column ? 1 : -1;
            var rowStep = potentialMove.Start.Row < potentialMove.End.Row ? 1 : -1;
            var col = potentialMove.Start.Column + colStep;
            var row = potentialMove.Start.Row + rowStep;

            do
            {
                var square = GetSquare(col, row);
                if (square.IsOccupied)
                {
                    return false;
                }
                col += colStep;
                row += rowStep;
            } while (row != potentialMove.End.Row);
        }

        return true;
    }

    public PerformedMove Move(PieceItem piece, SquareItem endSquare)
    {
        var startSquare = GetSquare(piece);
        var colSteps = Math.Abs(startSquare.Column - endSquare.Column);
        var rowSteps = Math.Abs(startSquare.Row - endSquare.Row);

        var removedPiece = endSquare.Piece;
        var value = GetValue(endSquare.Piece);

        // Move
        endSquare.Piece = startSquare.Piece;
        startSquare.Piece = null;

        // Adjust piece
        piece.HasMoved = true;
        var finalizedPawn = piece.PieceType == PieceType.Pawn && (endSquare.Row == Row.Row1 || endSquare.Row == Row.Row8);
        if (finalizedPawn)
        {
            piece.PieceType = PieceType.Queen;
        }

        // Castling means we also need to move the corresponding rook
        var isCastlingMove = piece.PieceType == PieceType.King && rowSteps == 0 && colSteps == 2;
        if (isCastlingMove)
        {
            if (endSquare.Column == Column.C)
            {
                var rookStartSquare = GetSquare(Column.A, startSquare.Row);
                var rookEndSquare = GetSquare(Column.D, startSquare.Row);
                Move(rookStartSquare.Piece, rookEndSquare);
            }
            if (endSquare.Column == Column.G)
            {
                var rookStartSquare = GetSquare(Column.H, startSquare.Row);
                var rookEndSquare = GetSquare(Column.F, startSquare.Row);
                Move(rookStartSquare.Piece, rookEndSquare);
            }
        }

        SwapPlayer();

        return new PerformedMove(piece, startSquare, endSquare, removedPiece, value);
    }

    private void SwapPlayer()
    {
        if (CurrentPlayer == Color.White)
        {
            WhiteTime.Stop();
            CurrentPlayer = Color.Black;
            BlackTime.Start();
        }
        else
        {
            BlackTime.Stop();
            CurrentPlayer = Color.White;
            WhiteTime.Start();
        }
    }

    private bool IsCastlingMove(PotentialMove potentialMove)
    {
        // Moving piece must be a king
        if (potentialMove.Piece.PieceType != PieceType.King)
        {
            return false;
        }

        // King must be moved two steps on same row
        if (potentialMove.RowSteps != 0 || potentialMove.ColSteps != 2)
        {
            return false;
        }

        // Rook corner on that side must have a rook in place
        var king = potentialMove.Piece;
        var directionIsRight = potentialMove.End.Column > potentialMove.Start.Column;
        var rookSquare = directionIsRight ? SquareItems.GetSquare(Column.H, potentialMove.Start.Row) : SquareItems.GetSquare(Column.A, potentialMove.Start.Row);
        if (rookSquare.Piece == null || rookSquare.Piece.PieceType != PieceType.Rook)
        {
            return false;
        }

        // Neither king nor rook must have moved before
        var rook = rookSquare.Piece;
        if (king.HasMoved || rook.HasMoved)
        {
            return false;
        }

        // Positions in between must be empty
        var row = potentialMove.Start.Row;
        return directionIsRight ?
            SquareItems.GetSquare(Column.F, row).IsEmpty && SquareItems.GetSquare(Column.F, row).IsEmpty :
            SquareItems.GetSquare(Column.B, row).IsEmpty && SquareItems.GetSquare(Column.C, row).IsEmpty && SquareItems.GetSquare(Column.D, row).IsEmpty;
    }

    private static int GetValue(PieceItem piece)
    {
        if (piece == null)
        {
            return 0;
        }

        return piece.PieceType switch
        {
            PieceType.King => 0,
            PieceType.Queen => 9,
            PieceType.Knight => 3,
            PieceType.Bishop => 3,
            PieceType.Rook => 4,
            PieceType.Pawn => 1,
            _ => throw new ArgumentOutOfRangeException($"PieceType={piece.PieceType} is not handled"),
        };
    }
}
