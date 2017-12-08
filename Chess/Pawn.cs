using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Pawn : Piece
    {
        // Constructor
        public Pawn(bool color, Square position) : base("Pawn", 1, color, position) { }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public override bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            List<int> moves = this.Position.distance(destinationSquare);
            int startRow = this.Color ? 6 : 1;     // 6: black; 1: white    (lines 7 and 2)
            int moveDirection = this.Color ? -1 : 1;    // -1: black; 1: white

            if ((this.Position.Row == startRow && destinationSquare.Row == startRow + 2 * moveDirection && this.Position.Column == destinationSquare.Column) || (moves[0] == 0 && Math.Abs(moves[1]) == 1))     // Pawn movement - note that it cannot take any Piece while moving
            {
                return destinationSquare.OccupyingPiece == null;
            }
            else if (Math.Abs(moves[0]) == 1 && moves[1] == moveDirection && Math.Abs(moves[1]) == 1)  // Capturing enemy Piece
            {
                return base.IsValidMove(destinationSquare, board);
            }
            else if (board.LastPiecePlayed is Pawn
                && Math.Abs(board.LastPiecePlayedStartPosition.Column - this.Position.Column) == 1 && board.LastPiecePlayedStartPosition.Row - this.Position.Row == moveDirection * 2 && board.LastPiecePlayed.Color != this.Color)      // En passant capture
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the squares between the current Piece's Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Returns a list of Square</returns>
        public override List<Square> SquaresBetween(Square destinationSquare, Chessboard board)
        {
            List<Square> squares = new List<Square>();
            if (this.Color && this.Position.Column < 6 || !this.Color && this.Position.Column > 1)
            {
                int moveDirection = this.Color ? -1 : 1;
                squares.Add(board.Board[this.Position.Column + moveDirection, this.Position.Row + moveDirection]);
            }
            return squares;
        }
    }
}
