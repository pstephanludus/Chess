using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// A Square object is a component of the board of a Chessboard object
    /// It has a column, a row, and an occupying Piece object
    /// </summary>
    class Square
    {
        // Attributes
        private int column;
        private int row;
        private Piece occupyingPiece;

        // Accessors
        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }
        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }
        public Piece OccupyingPiece
        {
            get
            {
                return occupyingPiece;
            }

            set
            {
                occupyingPiece = value;
            }
        }

        // Constructor
        public Square(int column, int row)
        {
            this.Column = column;
            this.Row = row;
            this.OccupyingPiece = null;
        }
        
        /// <summary>
        /// Returns a srtring representing the Square object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Square.getColumnName(this.Column) + (this.Row + 1).ToString() + ": " + (this.OccupyingPiece!=null? this.OccupyingPiece.Name: "empty");
        }

        /// <summary>Returns an integer corresponding to the number of a column given it's letter.</summary>
        /// <remarks>The integer returned DOESN'T take into account the offset 
        /// between the user-inputted column number (starting at 1) 
        /// and the object model column number (starting at 0)</remarks>
        /// <param name="columnName">the char corresponding to a column letter</param>
        /// <returns>Returns an integer corresponding to a column number</returns>
        public static int getColumn(char columnName)
        {
            Dictionary<char, int> res = new Dictionary<char, int>();
            for(int i = 65; i<91; i++)
            {
                res.Add((char)i, i - 64);
            }

            return res[char.ToUpper(columnName)];
        }


        /// <summary>Returns a char corresponding to the letter of a column given it's number</summary>
        /// <remarks>The char returned DOES take into account the offset 
        /// between the user-inputted column number (starting at 1) 
        /// and the object model column number (starting at 0)</remarks>
        /// <param name="columnName">the integer corresponding to a column number</param>
        /// <returns>Returns a char corresponding to a column letter</returns>
        public static char getColumnName(int column)
        {
            Dictionary<int, char> res = new Dictionary<int, char>();
            for (int i = 65; i < 91; i++)
            {
                res.Add(i - 64, (char)i);
            }
            return res[column];
        }


        /// <summary>
        /// Tests if a Square is black or white
        /// </summary>
        /// <returns>Returns true if the Square is black, and white otherwise</returns>
        public bool isBlack()
        {
            return (((this.Column + this.Row) % 2) == 0);
        }


        /// <summary>
        /// Calculates the distance between the current Square and another given Square
        /// </summary>
        /// <param name="destinationSquare">Another Square</param>
        /// <returns>Returns a two-item integer list representing the offsets in columns and rows respectively</returns>
        public List<int> distance(Square destinationSquare)
        {
            return new List<int>()
            {
                destinationSquare.Column - this.Column,
                destinationSquare.Row - this.Row
            };
        }
    }
}
