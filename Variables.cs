using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFChess
{
    internal static class Variables
    {
        public static Canvas? boardCanvas;
        public static Board? board;
        public static WPFChess.MainWindow.MouseMoveEventHandler? dragHandler;
        public static WPFChess.MainWindow.MouseMoveEventHandler? clickHandler;
        public static int sizeOfOffset;
        public static int sizeOfField;
        private static string lastID = "a";
        public static int widthOfBoard;
        public static int heightOfBoard;
        public static CheckBox? autoRotation;

        public static Dictionary<string, string> piecePaths = new Dictionary<string, string>()
        {
            { "Pawn_white", "static/pawn_w.png" },
            { "Pawn_black", "static/pawn_b.png" },
            { "Knight_white", "static/knight_w.png" },
            { "Knight_black", "static/knight_b.png" },
            { "Bishop_white", "static/bishop_w.png" },
            { "Bishop_black", "static/bishop_b.png" },
            { "Rook_white", "static/rook_w.png" },
            { "Rook_black", "static/rook_b.png" },
            { "King_white", "static/king_w.png" },
            { "King_black", "static/king_b.png" },
            { "Queen_white", "static/queen_w.png" },
            { "Queen_black", "static/queen_b.png" }
        };

        public static string getNewId()
        {
            lastID = lastID + lastID[0];

            if (lastID.Length > 10)
            {
                lastID = Convert.ToChar(lastID[0] + 1).ToString();
            }
            return lastID;
        }
    }
}
