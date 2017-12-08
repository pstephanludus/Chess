using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Queen:Piece
    {
        // Constructor
        public Queen(bool color, Square position) : base("Queen", 10, color, position) { }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public override bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            //We check if a Bishop or a Rook could go to the given Square - if yes, the Queen can do it too !
            bool res = new Bishop(this.Color, this.Position).IsValidMove(destinationSquare, board) || new Rook(this.Color, this.Position).IsValidMove(destinationSquare, board);
            this.Position.OccupyingPiece = this;    // Piece's constructor modifies the Square occupied by the Queen, we need to set it bakc to it's correct value
            return res;
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
            if (moves[0] == moves[1])
            {
                return new Bishop(this.Color, this.Position).SquaresBetween(destinationSquare, board);
            }
            else
            {
                return new Bishop(this.Color, this.Position).SquaresBetween(destinationSquare, board);
            }
        }
    }
}
