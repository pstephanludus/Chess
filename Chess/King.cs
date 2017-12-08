using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class King:Piece
    {
        // Constructor
        public King(bool color, Square position) : base("King", int.MaxValue, color, position) { }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public override bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            List<int> moves = this.Position.distance(destinationSquare);

            // We temporarily "kill" the Piece occupying the tested Square, and moves our King there
            Piece pieceOriginelle = destinationSquare.OccupyingPiece;
            Square caseOriginelle = this.Position;
            if (pieceOriginelle != null) pieceOriginelle.Position = null;

            if (Math.Abs(moves[0]) < 2 && Math.Abs(moves[1]) < 2 && Math.Abs(moves[0]) + Math.Abs(moves[1]) < 3) // King's Movement
            {
                this.Position = destinationSquare;
                destinationSquare.OccupyingPiece = this;
                if (this.isChecked(board) == false)  // On ne peut pas déplacer son roi vers une case où il serait en échec
                {
                    // We rollback
                    destinationSquare.OccupyingPiece = pieceOriginelle;
                    if (pieceOriginelle != null) pieceOriginelle.Position = destinationSquare;
                    this.Position = caseOriginelle;
                    return base.IsValidMove(destinationSquare, board);
                }
            }
            // We rollback
            destinationSquare.OccupyingPiece = pieceOriginelle;
            if (pieceOriginelle != null) pieceOriginelle.Position = destinationSquare;
            this.Position = caseOriginelle;
            return false;
        }

        /// <summary>
        /// Gets valid moves for the King
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns>Returns a list of Square, which are all the possible candidates for the King's move</returns>
        public List<Square> ValidMoves(Chessboard board)
        {
            List<Square> moves = new List<Square>();
            if (this.Position.Column > 0)
            {
                if (this.Position.Row > 0)
                {
                    Square northEast = board.Board[this.Position.Column - 1, this.Position.Row - 1];
                    if (IsValidMove(northEast, board)) moves.Add(northEast);
                }
                else if (this.Position.Row < board.Board.GetLength(1) - 1)
                {
                    Square southEast = board.Board[this.Position.Column - 1, this.Position.Row + 1];
                    if (IsValidMove(southEast, board)) moves.Add(southEast);
                }
                Square east = board.Board[this.Position.Column - 1, this.Position.Row];
                if (IsValidMove(east, board)) moves.Add(east);
            }
            if (this.Position.Column < board.Board.GetLength(0) - 1)
            {
                if (this.Position.Row > 0)
                {
                    Square northWest = board.Board[this.Position.Column + 1, this.Position.Row - 1];
                    if (IsValidMove(northWest, board)) moves.Add(northWest);
                }
                else if (this.Position.Row < board.Board.GetLength(1) - 1)
                {
                    Square southWest = board.Board[this.Position.Column + 1, this.Position.Row + 1];
                    if (IsValidMove(southWest, board)) moves.Add(southWest);
                }
                Square west = board.Board[this.Position.Column + 1, this.Position.Row];
                if (IsValidMove(west, board)) moves.Add(west);
            }
            if (this.Position.Row > 0)
            {
                Square north = board.Board[this.Position.Column, this.Position.Row - 1];
                if (IsValidMove(north, board)) moves.Add(north);
            }
            else if (this.Position.Row < board.Board.GetLength(1) - 1)
            {
                Square south = board.Board[this.Position.Column, this.Position.Row + 1];
                if (IsValidMove(south, board)) moves.Add(south);
            }
            return moves;
        }

        /// <summary>
        /// Checks if the King is checked
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns>Returns true if a hostile Piece can take the King, otherwise returns false</returns>
        public bool isChecked(Chessboard board)
        {
            List<Piece> checkedPieces = this.Color ? board.WhitePieces : board.BlackPieces;
            foreach (Piece piece in checkedPieces)
            {
                if (piece.Position != null)
                {
                    if (piece.IsValidMove(this.Position, board))
                    {
                        return true;
                    }
                }
            }            
            return false;
        }

        /// <summary>
        /// Checks if the King can be saved either by taking the hostile Piece, intercepting its movement, or fleeing
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns>Returns true if the King can not be saved, otherwise returns false</returns>
        public bool isCheckMate(Chessboard board)
        {
            if (!TestCheckedCanTake(board) && !TestCheckedCanIntercept(board) && this.ValidMoves(board).Count == 0) return true;
            return false;
        }

        /// <summary>
        /// Checks if a Piece can take the hostile Piece
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns>Returns true if a Piece can go to the Square of the attacking Piece, otherwise returns false</returns>
        public bool TestCheckedCanTake(Chessboard board)
        {
            List<Piece> defensivePieces = this.Color ? board.BlackPieces : board.WhitePieces;
            foreach (Piece piece in defensivePieces)
            {
                if (piece.Position != null)
                {
                    if (piece.IsValidMove(board.LastPiecePlayed.Position, board))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a Piece can intercept the hotsile Piece's movement
        /// </summary>
        /// <param name="board">The board</param>
        /// <returns>REturns true if a Piece can move between the attacking Piece and the King, otherwise returns false</returns>
        public bool TestCheckedCanIntercept(Chessboard board)
        {
            List<Piece> defensivePieces = this.Color ? board.BlackPieces : board.WhitePieces;
            List<Square> squaresBetweenPieceAndKing = board.LastPiecePlayed.SquaresBetween(this.Position, board);
            foreach (Piece piece in defensivePieces)
            {
                foreach (Square intermediateSquare in squaresBetweenPieceAndKing)
                {
                    if (piece.Position != null)
                    {
                        if (piece.IsValidMove(intermediateSquare, board)) return true;
                    }
                }
            }
            return false;
        }
    }
}
