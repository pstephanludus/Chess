using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Bishop:Piece
    {
        // Constructor
        public Bishop(bool color, Square position) : base("Bishop", 3, color, position) { }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public override bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            List<int> moves = this.Position.distance(destinationSquare);
            if (Math.Abs(moves[0]) == Math.Abs(moves[1])) // Bishop move
            {
                foreach (Square betweenSquare in this.SquaresBetween(destinationSquare, board))
                {
                    if (betweenSquare.OccupyingPiece != null) return false;
                }
                return base.IsValidMove(destinationSquare, board);
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
            List<int> moves = this.Position.distance(destinationSquare);
            List<Square> squares = new List<Square>();
            for (int i = 1; i < Math.Abs(moves[0]); i++)
            {
                squares.Add(board.Board[
                    this.Position.Column + moves[0] / Math.Abs(moves[0]) * i,
                    this.Position.Row + moves[1] / Math.Abs(moves[1]) * i]);
            }
            return squares;
        }
    }
}
