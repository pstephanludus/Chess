using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Chessboard board = new Chessboard();

            // Checking en passant capture
            Console.WriteLine("Checking en passant capture");
            board.MovePiece('e', 2, 'e', 4);
            board.MovePiece('b', 7, 'b', 6);
            board.MovePiece('b', 6, 'b', 5);  // Checking that player cannot play two times in a row

            board.MovePiece('e', 4, 'e', 6);  // Checking that player cannot play incorrect move
            board.MovePiece('e', 4, 'e', 5);
            board.MovePiece('f', 7, 'f', 5);

            board.MovePiece('e', 5, 'f', 6);  // Checking en passant capture - White Pawn takes Black Pawn
            Console.WriteLine("Pawn on F5 ? " + (board.Board[5, 4].OccupyingPiece != null ? "Yes" : "No"));




            //  Check historical game  -   Gioachino Greco vs NN   -   http://www.chessgames.com/perl/chessgame?gid=1243022

            board = new Chessboard();
            Console.WriteLine("\n\n\n\n\nCheck historical game Gioachino Greco vs NN");

            board.MovePiece('E', 2, 'E', 4);
            board.MovePiece('B', 7, 'b', 6);

            board.MovePiece('d', 2, 'd', 4);
            board.MovePiece('c', 8, 'b', 7);

            board.MovePiece('f', 1, 'D', 3);
            board.MovePiece('f', 7, 'f', 5);

            board.MovePiece('e', 4, 'f', 5);
            board.MovePiece('b', 7, 'g', 2);

            board.MovePiece('d', 1, 'h', 5);
            if (board.BlackKing.isChecked(board)) Console.WriteLine("Black king, check !");
            if (board.BlackKing.isCheckMate(board)) Console.WriteLine("Black king, checkMate !");
            board.MovePiece('g', 7, 'g', 6);

            board.MovePiece('f', 5, 'g', 6);
            board.MovePiece('g', 8, 'f', 6);

            board.MovePiece('g', 6, 'h', 7);
            board.MovePiece('f', 6, 'h', 5);

            board.MovePiece('d', 3, 'g', 6);
            if (board.BlackKing.isChecked(board)) Console.WriteLine("Black king, check !");
            if (board.BlackKing.isCheckMate(board)) Console.WriteLine("Black king, checkMate !");




            // Free play
            Console.WriteLine("\n\n\n\n\n\n\n");
            board = new Chessboard();
            bool play = true;

            Console.WriteLine("\n\n\n\n\nYou can now play on your own ! Please input your move like this: E2 E4\nYou can also provide column by number, which would result in 52 54");
            while (play)//(plateau.RoiBlanc.EstMat(plateau) != false && plateau.RoiNoir.EstMat(plateau) != false)
            {
                Console.WriteLine("Input your move: (ex: E2 E4)");
                string coup = Console.ReadLine();
                try
                {
                    board.MovePiece(coup.Split()[0][0], (int)char.GetNumericValue(coup.Split()[0][1]), coup.Split()[1][0], (int)char.GetNumericValue(coup.Split()[1][1]));
                    King checkedKing = board.LastPiecePlayed.Color ? board.BlackKing : board.WhiteKing;
                    if (checkedKing.isChecked(board))
                    {
                        if (checkedKing.isCheckMate(board))
                        {
                            Console.WriteLine("Checkmate !");
                            Console.WriteLine((checkedKing.Color ? "White" : "Black") + " wins!");
                            play = false;
                        }
                        else
                        {
                            Console.WriteLine((checkedKing.Color ? "Black" : "White") + "King checked !");
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Invalid move, please try again");
                }
            }
        }
    }
}
