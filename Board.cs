using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPFChess
{

    internal static class Variables
    {
        public static Canvas? boardCanvas;
        public static Board? board;
        public static WPFChess.MainWindow.MouseMoveEventHandler? mouseHandler;
        private static string lastID = "a";

        public static Dictionary<string, string> piecePaths = new Dictionary<string, string>()
        {
            { "pawn_w", "static/pawn_w.png" },
            { "pawn_b", "static/pawn_b.png" },
            { "knight_w", "static/knight_w.png" },
            { "knight_b", "static/knight_b.png" },
            { "bishop_w", "static/bishop_w.png" },
            { "bishop_b", "static/bishop_b.png" },
            { "rook_w", "static/rook_w.png" },
            { "rook_b", "static/rook_b.png" },
            { "king_w", "static/king_w.png" },
            { "king_b", "static/king_b.png" },
            { "queen_w", "static/queen_w.png" },
            { "queen_b", "static/queen_b.png" }
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

        public Board(int sizeOfField, int sizeOfOffset, int setup, Canvas boardCanvas, WPFChess.MainWindow.MouseMoveEventHandler handler)
        {
            this.initializeBoard(boardCanvas, handler);
            this.initalizeFields(sizeOfField, sizeOfOffset, setup);
        }

        private void initializeBoard(Canvas boardCanvas, WPFChess.MainWindow.MouseMoveEventHandler handler)
        {
            Variables.boardCanvas = boardCanvas;
            Variables.mouseHandler = handler;
            Variables.board = this;
            Image boardImage = new Image();
            boardImage.Source = new BitmapImage(new Uri("static/Board2.jpg", UriKind.Relative));
            Canvas.SetLeft(boardImage, 0);
            Canvas.SetTop(boardImage, 0);
            boardCanvas.Children.Add(boardImage);
        }

        private void initalizeFields(int sizeOfField, int sizeOfOffset, int setup)
        {
            fields = new Field[8, 8];
            int height = sizeOfOffset + (fields.GetLength(0)-1)*sizeOfField+(sizeOfField / 2);
            int width = sizeOfOffset + (sizeOfField / 2);         
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    fields[j, i] = new Field(j+1,i+1,width, height, null);
                    width += sizeOfField;
                }
                height -= sizeOfField;
                width = sizeOfOffset + (sizeOfField / 2);
            }

            for (int i = 0; i < fields.GetLength(1); i++)
            {
                fields[i, 1].piece = new Pawn("pawn_w", fields[i, 1]);
            }
            for (int i = 0; i < fields.GetLength(1); i++)
            {
                fields[i, 6].piece = new Pawn("pawn_b", fields[i, 6]);
            }
            fields[0, 0].piece = new Piece("rook_w", fields[0, 0]);
            fields[7, 0].piece = new Piece("rook_w", fields[7, 0]);
            fields[0, 7].piece = new Piece("rook_b", fields[0, 7]);
            fields[7, 7].piece = new Piece("rook_b", fields[7, 7]);

            fields[1, 0].piece = new Piece("knight_w", fields[1, 0]);
            fields[6, 0].piece = new Piece("knight_w", fields[6, 0]);
            fields[6, 7].piece = new Piece("knight_b", fields[6, 7]);
            fields[1, 7].piece = new Piece("knight_b", fields[1, 7]);

            fields[2, 0].piece = new Piece("bishop_w", fields[2, 0]);
            fields[5, 0].piece = new Piece("bishop_w", fields[5, 0]);
            fields[5, 7].piece = new Piece("bishop_b", fields[5, 7]);
            fields[2, 7].piece = new Piece("bishop_b", fields[2, 7]);

            fields[3, 0].piece = new Piece("queen_w", fields[3, 0]);
            fields[3, 7].piece = new Piece("queen_b", fields[3, 7]);

            fields[4, 0].piece = new Piece("king_w", fields[4, 0]);
            fields[4, 7].piece = new Piece("king_b", fields[4, 7]);
        }

        public Field findNearestField(double x, double y)
        {
            Field result = (from Field item in fields select item).MinBy(a => a.cartesianDistance(x,y));
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
        }

        private bool onBoard(int x, int y)
        {
            if(x>=1 && y>=1 && x<=fields.GetLength(0) && y <= fields.GetLength(0))
            {
                return true;
            }
            return false;
        }

        private int whichDiagonal(Field firstField, Field secondField)
        {
            //right up
            int i = firstField.x;
            int j = firstField.y;
            while(onBoard(i, j))
            {               
                if (fields[i-1, j-1] == secondField)
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

        public bool isFreeBetween(Field firstField, Field secondField, string mode="all")
        {
            int a, b;
            if (mode == "vertically")
            {
                if(firstField.x == secondField.x)
                {
                    a = firstField.y > secondField.y ? -1 : 1;
                    for (int i = firstField.y+a; i != secondField.y; i += a)
                    {
                        if (fields[firstField.x-1, i-1].piece != null)
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
                        if (fields[firstField.y-1, i-1].piece != null)
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
                while (onBoard(i, j) && fields[i - 1, j - 1]!=secondField)
                {
                    i += a;
                    j += b;
                    if (fields[i - 1, j - 1].piece!=null)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                if(isFreeBetween(firstField, secondField, "vertically") 
                    || isFreeBetween(firstField, secondField, "horizontally") 
                    || isFreeBetween(firstField, secondField, "diagonally"))
                {
                    return true;
                }
                return false;
            }
        }
    }

    internal class Field
    {
        public int x { get; set; }
        public int y { get; set; }
        public int xOnCanvas { get; set; }
        public int yOnCanvas { get; set; }
        public Piece? piece { get; set; }

        public Field(int x, int y, int xPosition, int yPosition, Piece piece)
        {   
            this.x = x;
            this.y = y;
            this.xOnCanvas = xPosition;
            this.yOnCanvas = yPosition;
            this.piece = piece;
        }

        public double cartesianDistance(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a - xOnCanvas, 2) + Math.Pow(b - yOnCanvas, 2));
        }
    }

    internal class Piece
    {
        public Image image { get; set; } = null;

        public string type { get; set; }

        public Field field { get; set; }

        public string id { get; set; }

        public char color { get; set; }


        public Piece(string type, Field field)
        {   
            this.id = Variables.getNewId();
            this.color = type[type.Length - 1];
            this.type = type.Remove(type.Length - 2);
            this.field = field;
            image = new Image();
            image.Source = new BitmapImage(new Uri(Variables.piecePaths[type], UriKind.Relative));
            image.Name = id;
            image.Width = 50;
            image.Height = 80;
            updateImage();
            image.MouseMove += new MouseEventHandler(Variables.mouseHandler);
            Variables.boardCanvas.Children.Add(image);
        }

        protected virtual bool isMovePossible(Field newField)
        {
            if (newField.piece != null)
            {
                if (newField.piece.color == color || newField.piece.type == "king")
                {
                    return false;
                }
            }
            return true;
        }

        public void move(Field newField)
        {
            if (!isMovePossible(newField))
            {
                updateImage();
                return;
            }
            if (newField.piece != null)
            {
                newField.piece.destroy();
            }
            newField.piece = this;
            field.piece = null;
            field = newField;
            updateImage();
        }

        public void destroy()
        {
            Variables.boardCanvas.Children.Remove(image);
            field.piece = null;
        }

        public void updateImage()
        {
            Canvas.SetLeft(image, field.xOnCanvas - image.Width / 2);
            Canvas.SetTop(image, field.yOnCanvas - image.Height / 2);
        }

    }

    internal class Pawn : Piece
    {
        private bool firstMove;
        public Pawn(string type, Field field) : base(type, field)
        {
            firstMove = true;
        }

        protected override bool isMovePossible(Field newField)
        {
            if (!base.isMovePossible(newField))
            {
                return false;
            }
            if (firstMove)           
            {
                //white 2 forward
                if(color=='w' && newField.y == field.y + 2 && Variables.board.isFreeBetween(field, newField))
                {
                    firstMove=false;
                    return true;
                }
                //black 2 forward
                if (color == 'b' && newField.y == field.y - 2 && Variables.board.isFreeBetween(field, newField))
                {
                    firstMove = false;
                    return true;
                }
            }
            //white 1 forward
            if (color == 'w' && newField.y == field.y + 1 && newField.x==field.x)
            {
                firstMove = false;
                return true;
            }
            //black 1 forward
            if (color == 'b' && newField.y == field.y - 1 && newField.x == field.x)
            {
                firstMove = false;
                return true;
            }
            
            if (newField.piece != null)
            {
                //white attack
                if (color == 'w' && newField.y == field.y + 1 && Math.Abs(field.x - newField.x)==1)
                {
                    firstMove = false;
                    return true;
                }
                //black attack
                if (color == 'b' && newField.y == field.y - 1 && Math.Abs(field.x - newField.x) == 1)
                {
                    firstMove = false;
                    return true;
                }
            }
            return false;
        }
    }



}
