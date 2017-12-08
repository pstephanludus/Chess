using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Piece
    {

        // Attributs
        private string name;
        private int pieceValue;
        private bool color;     // true: black  -   false: white
        private Square position;

        // Accesseurs
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public int PieceValue
        {
            get
            {
                return pieceValue;
            }

            set
            {
                pieceValue = value;
            }
        }
        public bool Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }
        internal Square Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        // Constructeur
        public Piece(string name, int pieceValue, bool color, Square position)
        {
            this.Name = name;
            this.PieceValue = pieceValue;
            this.Color = color;
            this.Position = position;
            this.Position.OccupyingPiece = this;
        }
        
        public override string ToString()
        {
            return this.Name + "     -  position: " + this.Position.ToString() + "  -  couleur: " + this.Color + "  -  valeur: " + this.PieceValue;
        }

        /// <summary>
        /// Checks if the current Piece can go from its current Square to the given Square
        /// </summary>
        /// <remarks>This method is redefined by children, and is often called at the end of this redefinition</remarks>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Return true if the move is allowed, and false otherwise</returns>
        public virtual bool IsValidMove(Square destinationSquare, Chessboard board)
        {
            if (destinationSquare.OccupyingPiece == null)
            {
                return true;
            }
            else
            {
                return destinationSquare.OccupyingPiece.Color != this.Color;
            }
        }

        /// <summary>
        /// Gets the squares between the current Piece's Square
        /// </summary>
        /// <remarks>This method is redefined by children, and is often called at the end of this redefinition</remarks>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        /// <returns>Returns a list of Square</returns>
        public virtual List<Square> SquaresBetween(Square destinationSquare, Chessboard board)
        {
            return new List<Square>();
        }

        /// <summary>
        /// Moves a Piece from its current Square to the given Square
        /// </summary>
        /// <param name="destinationSquare">The destination square</param>
        /// <param name="board">The board</param>
        public void Move(Square destinationSquare, Chessboard board)
        {
            if (this.Color != board.LastPiecePlayed.Color)     // A player cannot play two times in a row !
            {
                if (IsValidMove(destinationSquare, board))
                {
                    // Piece passed movement validity check
                    Console.WriteLine(this + " moving to " + destinationSquare);


                    // En passant capture case
                    if (this is Pawn && board.LastPiecePlayed is Pawn && 
                        Math.Abs(board.LastPiecePlayedStartPosition.Column - this.Position.Column) == 1 && 
                        (board.LastPiecePlayedStartPosition.Row - this.Position.Row) == ((this.Color? -1 : 1) * 2))
                    {
                        Console.WriteLine("     LOG: Capturing Piece " + board.LastPiecePlayed);
                        board.LastPiecePlayed.Position.OccupyingPiece = null;
                    }

                    if (destinationSquare.OccupyingPiece != null)
                    {
                        Console.WriteLine("     LOG: Capturing Piece " + destinationSquare.OccupyingPiece);
                    }
                    // Updating last played Piece data
                    board.LastPiecePlayed = this;
                    board.LastPiecePlayedStartPosition = this.Position;

                    // Updating just played Piece data
                    this.Position.OccupyingPiece = null;
                    this.Position = destinationSquare;
                    if (this.Position.OccupyingPiece != null)   //  If Square was already occupied - its Piece is captured - we "kill" the piece by setting its Position to null
                    {
                        this.Position.OccupyingPiece.Position = null;
                    }
                    this.Position.OccupyingPiece = this;

                }
                else
                {
                    Console.WriteLine("     LOG: Incorrect movement " + this.ToString() + " to " + destinationSquare);
                }
            }
            else
            {
                Console.WriteLine("     LOG: Cannot play Piece " + this.Name + " (" + this.Position + ") : it's not " + this.Color + "'s turn!");
            }
        }

    }
}
