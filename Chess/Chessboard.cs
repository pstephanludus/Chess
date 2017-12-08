using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// A Chessboard object keeps the game's datas and handles user input
    /// </summary>
    class Chessboard
    {
        // Attributes
        private Square[,] board;

            // Allows sizes other than 8x8

        private List<Piece> blackPieces;
        private List<Piece> whitePieces;

            // We often use kings, may as well stock them for easy access
        private King blackKing;
        private King whiteKing;


            // Used to enable en passant captures
        private Piece lastPiecePlayed;
        private Square lastPiecePlayedPosition;


        // Accessors
        internal Square[,] Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }
        internal List<Piece> BlackPieces
        {
            get
            {
                return blackPieces;
            }

            set
            {
                blackPieces = value;
            }
        }
        internal List<Piece> WhitePieces
        {
            get
            {
                return whitePieces;
            }

            set
            {
                whitePieces = value;
            }
        }
        internal King BlackKing
        {
            get
            {
                return blackKing;
            }

            set
            {
                blackKing = value;
            }
        }
        internal King WhiteKing
        {
            get
            {
                return whiteKing;
            }

            set
            {
                whiteKing = value;
            }
        }
        internal Piece LastPiecePlayed
        {
            get
            {
                return lastPiecePlayed;
            }

            set
            {
                lastPiecePlayed = value;
            }
        }
        internal Square LastPiecePlayedStartPosition
        {
            get
            {
                return lastPiecePlayedPosition;
            }

            set
            {
                lastPiecePlayedPosition = value;
            }
        }

        
        // Constructor
        public Chessboard(int columnNumber = 8, int rowNumber = 8, int gameModeID = 0)
        {
            this.Board = new Square[columnNumber, rowNumber];
            for (int i = 0; i < columnNumber; i++)
            {
                for (int j = 0; j < rowNumber; j++)
                {
                    this.Board[i, j] = new Square(i, j);
                }
            }
            if (gameModeID == 0)
            {
                this.InitStandard();
            }
        }
        
        /// <summary>Gets the number of points of a given player</summary>
        /// <remarks>Only alive Pieces (Pieces whose Position is not null) are counted</remarks>
        /// <param name="color">A boolean representing a player - false for black, true for white</param>
        /// <returns>Returns an integer corresponding to the sum of a player's remaining Pieces</returns>
        public int PlayerPoints(bool color)
        {
            int res = 0;
            List<Piece> playerPieces = color ? this.BlackPieces : WhitePieces;
            foreach (Piece piece in playerPieces)
            {
                if (piece.Position != null) res += piece.PieceValue;
            }
            return res;
        }

        /// <summary>
        /// Inits a standard chess play
        /// </summary>
        public void InitStandard()
        {
            // Listes des pièces
            this.WhitePieces = new List<Piece>();
            this.BlackPieces = new List<Piece>();

            // Pièces blanches
            this.WhitePieces.Add(new Rook(false, this.Board[0, 0]));
            this.WhitePieces.Add(new Knight(false, this.Board[1, 0]));
            this.WhitePieces.Add(new Bishop(false, this.Board[2, 0]));
            this.WhitePieces.Add(new Queen(false, this.Board[3, 0]));
            this.WhitePieces.Add(new King(false, this.Board[4, 0]));
            this.WhitePieces.Add(new Bishop(false, this.Board[5, 0]));
            this.WhitePieces.Add(new Knight(false, this.Board[6, 0]));
            this.WhitePieces.Add(new Rook(false, this.Board[7, 0]));
            // Pions blancs
            for (int i = 0; i < 8; i++)
            {
                this.WhitePieces.Add(new Pawn(false, this.Board[i, 1]));
            }

            // Pièces noires
            this.BlackPieces.Add(new Rook(true, this.Board[0, 7]));
            this.BlackPieces.Add(new Knight(true, this.Board[1, 7]));
            this.BlackPieces.Add(new Bishop(true, this.Board[2, 7]));
            this.BlackPieces.Add(new Queen(true, this.Board[3, 7]));
            this.BlackPieces.Add(new King(true, this.Board[4, 7]));
            this.BlackPieces.Add(new Bishop(true, this.Board[5, 7]));
            this.BlackPieces.Add(new Knight(true, this.Board[6, 7]));
            this.BlackPieces.Add(new Rook(true, this.Board[7, 7]));
            // Pions noirs
            for (int i = 0; i < 8; i++)
            {
                this.BlackPieces.Add(new Pawn(true, this.Board[i, 6]));
            }

            // Rois
            this.WhiteKing = (King)this.Board[4, 0].OccupyingPiece;
            this.BlackKing = (King)this.Board[4, 7].OccupyingPiece;

            // Gestion du tout premier coup joué - on part du principe que la dernière pièce à avoir joué est le roi noir (par ex qui a fait une déclaration de guerre aux blancs) - pour prendre en charge le test sur la dernière pièce jouée
            this.LastPiecePlayed = this.BlackKing;
            this.LastPiecePlayedStartPosition = LastPiecePlayed.Position;
        }

        /// <summary>
        /// Moves a Piece to a designated int-located Square
        /// </summary>
        /// <param name="originColumn">Column of Square of origin</param>
        /// <param name="originRow">Row of Square of origin</param>
        /// <param name="destinationColumn">Column of Square of destination</param>
        /// <param name="destinationRow">Row of Square of destination</param>
        public void MovePiece(int originColumn, int originRow, int destinationColumn, int destinationRow)
        {
            Square destinationSquare = this.Board[destinationColumn - 1, destinationRow - 1];
            Piece movedPiece = this.Board[originColumn - 1, originRow - 1].OccupyingPiece;
            if (movedPiece != null)
            {
                movedPiece.Move(destinationSquare, this);
            }
            else
            {
                Console.WriteLine("     LOG: Aucune pièce sur cette case");
            }
        }

        /// <summary>
        /// Moves a Piece to a designated char-and-int-located Square
        /// </summary>
        /// <param name="originColumnName"></param>
        /// <param name="originRow"></param>
        /// <param name="destinationColumnName"></param>
        /// <param name="destinationRow"></param>
        public void MovePiece(char originColumnName, int originRow, char destinationColumnName, int destinationRow)
        {
            MovePiece(
                Square.getColumn(originColumnName), originRow, 
                Square.getColumn(destinationColumnName), destinationRow
            );
        }
        /// <summary>
        /// Moves a Piece to a designated string-and-int-located Square
        /// </summary>
        /// <param name="originColumnName"></param>
        /// <param name="originRow"></param>
        /// <param name="destinationColumnName"></param>
        /// <param name="destinationRow"></param>
        public void MovePiece(string originColumnName, int originRow, string destinationColumnName, int destinationRow)
        {
            MovePiece(
                Square.getColumn(char.Parse(originColumnName)), originRow, 
                Square.getColumn(char.Parse(destinationColumnName)), destinationRow
            );
        }

    }
}
