using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Knight:Piece
    {
        // Constructor
        public Knight(bool color, Square position) : base("Knight", 3, color, position) { }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public override bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            List<int> moves = this.Position.distance(destinationSquare);
            if ((Math.Abs(moves[0]) + Math.Abs(moves[1])) == 3 && Math.Abs(moves[0]) < 3 && Math.Abs(moves[1]) < 3)  // Knight move
            {
                return base.IsValidMove(destinationSquare, board);
            }
            return false;
        }
    }
}
