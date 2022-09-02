using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFChess
{

    internal static class Variables
    {
        public static Canvas? boardCanvas;
        public static Board? board;
        public static WPFChess.MainWindow.MouseMoveEventHandler? mouseHandler;
        public static int sizeOfOffset;
        public static int sizeOfField;
        private static string lastID = "a";
        public static int widthOfBoard;
        public static int heightOfBoard;

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

            if(lastID.Length > 10)
            {
                lastID = Convert.ToChar(lastID[0] + 1).ToString();
            }
            return lastID;
        }
    }

    internal class Board
    {
        private Canvas boardCanvas;

        private Field[,] fields;

        public Piece duringPromotion;

        public Board(int sizeOfField, int sizeOfOffset, int sizeOfBoard, int setup, Canvas boardCanvas, WPFChess.MainWindow.MouseMoveEventHandler handler)
        {
            Variables.sizeOfOffset = sizeOfOffset;
            Variables.sizeOfField = sizeOfField;
            Variables.widthOfBoard = sizeOfBoard;
            Variables.heightOfBoard = sizeOfBoard;
            Variables.boardCanvas = boardCanvas;
            Variables.mouseHandler = handler;
            Variables.board = this;
            duringPromotion = null;
            this.initializeBoard(boardCanvas, handler, sizeOfBoard);
            this.initalizeFields(sizeOfField, sizeOfOffset, setup);            
        }

        private void initializeBoard(Canvas boardCanvas, WPFChess.MainWindow.MouseMoveEventHandler handler, int sizeOfBoard)
        {          
            fields = new Field[sizeOfBoard, sizeOfBoard];
            Image boardImage = new Image();
            boardImage.Source = new BitmapImage(new Uri("static/Board2.jpg", UriKind.Relative));
            Panel.SetZIndex(boardImage, 0);
            Canvas.SetLeft(boardImage, 0);
            Canvas.SetTop(boardImage, 0);
            boardCanvas.Children.Add(boardImage);
        }

        private void initalizeFields(int sizeOfField, int sizeOfOffset, int setup)
        {          
            int height = sizeOfOffset + (fields.GetLength(0) - 1) * sizeOfField + (sizeOfField / 2);
            int width = sizeOfOffset + (sizeOfField / 2);
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    fields[j, i] = new Field(j + 1, i + 1, width, height, null);
                    width += sizeOfField;
                }
                height -= sizeOfField;
                width = sizeOfOffset + (sizeOfField / 2);
            }

            for (int i = 0; i < fields.GetLength(1); i++)
            {
                new Pawn("white", fields[i, 1]);
            }
            for (int i = 0; i < fields.GetLength(1); i++)
            {
                new Pawn("black", fields[i, 6]);
            }
            new Rook("white", fields[0, 0]);
            new Rook("white", fields[7, 0]);
            new Rook("black", fields[0, 7]);
            new Rook("black", fields[7, 7]);

            new Knight("white", fields[1, 0]);
            new Knight("white", fields[6, 0]);
            new Knight("black", fields[6, 7]);
            new Knight("black", fields[1, 7]);

            new Bishop("white", fields[2, 0]);
            new Bishop("white", fields[5, 0]);
            new Bishop("black", fields[5, 7]);
            new Bishop("black", fields[2, 7]);

            new Queen("white", fields[3, 0]);
            new Queen("black", fields[3, 7]);

            new King("white", fields[4, 0]);
            new King("black", fields[4, 7]);
        }

        public bool placePiece(Piece piece, int x, int y)
        {
            if (onBoard(x, y))
            {
                fields[x - 1, y - 1].piece = piece;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Field findNearestField(double x, double y)
        {
            Field result = (from Field item in fields select item).MinBy(a => a.cartesianDistance(x, y));
            return result;
        }

        public Piece findPieceById(string id)
        {
            var partialResult = from Field field in fields group field by new { piece = field.piece } into p where p.Key.piece is not null select p.Key.piece;
            var result = from item in partialResult where item.id == id select item;
            return result.First();
        }

        public void dropPiece(Image img, Point point)
        {
            Piece piece = findPieceById(img.Name);
            Field newField = findNearestField(point.X, point.Y);
            piece.move(newField);
            hideAllRectangles();
        }

        public bool onBoard(int x, int y)
        {
            if (x >= 1 && y >= 1 && x <= fields.GetLength(0) && y <= fields.GetLength(1))
            {
                return true;
            }
            return false;
        }

        public Field getField(int x, int y)
        {
            return fields[x-1, y-1];
        }

        private int whichDiagonal(Field firstField, Field secondField)
        {
            //right up
            int i = firstField.x;
            int j = firstField.y;
            while (onBoard(i, j))
            {
                if (fields[i - 1, j - 1] == secondField)
                {
                    return 1;
                }
                i++;
                j++;
            }
            //right down
            i = firstField.x;
            j = firstField.y;
            while (onBoard(i, j))
            {
                if (fields[i - 1, j - 1] == secondField)
                {
                    return 2;
                }
                i++;
                j--;
            }
            //left down
            i = firstField.x;
            j = firstField.y;
            while (onBoard(i, j))
            {
                if (fields[i - 1, j - 1] == secondField)
                {
                    return 3;
                }
                i--;
                j--;
            }
            //left up
            i = firstField.x;
            j = firstField.y;
            while (onBoard(i, j))
            {
                if (fields[i - 1, j - 1] == secondField)
                {
                    return 4;
                }
                i--;
                j++;
            }
            return 0;
        }

        public bool isFreeBetween(Field firstField, Field secondField, string mode = "all")
        {
            int a, b;
            if (mode == "vertically")
            {
                if (firstField.x == secondField.x)
                {
                    a = firstField.y > secondField.y ? -1 : 1;
                    for (int i = firstField.y + a; i != secondField.y; i += a)
                    {
                        if (fields[firstField.x - 1, i - 1].piece != null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (mode == "horizontally")
            {
                if (firstField.y == secondField.y)
                {
                    a = firstField.x > secondField.x ? -1 : 1;
                    for (int i = firstField.x + a; i != secondField.x; i += a)
                    {
                        if (fields[i - 1, firstField.y - 1].piece != null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (mode == "diagonally")
            {
                switch (whichDiagonal(firstField, secondField))
                {
                    case 0:
                        return false;
                    case 1:
                        a = 1;
                        b = 1;
                        break;
                    case 2:
                        a = 1;
                        b = -1;
                        break;
                    case 3:
                        a = -1;
                        b = -1;
                        break;
                    default:
                        a = -1;
                        b = 1;
                        break;
                }
                int i = firstField.x;
                int j = firstField.y;
                while (onBoard(i, j))
                {
                    i += a;
                    j += b;
                    if (fields[i - 1, j - 1] == secondField)
                    {
                        break;
                    }
                    if (fields[i - 1, j - 1].piece != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                if (isFreeBetween(firstField, secondField, "vertically")
                    || isFreeBetween(firstField, secondField, "horizontally")
                    || isFreeBetween(firstField, secondField, "diagonally"))
                {
                    return true;
                }
                return false;
            }
        }

        public void hideAllRectangles()
        {
            for(int i=0; i<fields.GetLength(0); i++)
            {
                for(int j=0; j<fields.GetLength(1); j++)
                {
                    fields[i, j].hideRectangle();
                }
            }
        }

        public void showMoves(Image image)
        {
            findPieceById(image.Name).showPossibleMoves();
        }

        private Field getKing(string color)
        {
            foreach(Field field in fields)
            {
                if(field.piece is not null)
                {
                    if(field.piece is King && field.piece.color == color)
                    {
                        return field;
                    }
                }
            }
            return null;
        }

        public bool isCheck(string color)
        {
            Field kingsField = getKing(color);
            King king = (King)kingsField.piece;
            kingsField.piece=null;

            foreach(Field field in fields)
            {
                if (field.piece is not null)
                {
                    if (field.piece.color != color)
                    {
                        if (field.piece.canAttack(kingsField))
                        {
                            kingsField.piece = king;
                            return true;
                        }
                    }
                }
            }
            kingsField.piece = king;
            return false;
        }
    }
}
