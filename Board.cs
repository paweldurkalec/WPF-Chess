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
            Image boardImage = new Image();
            boardImage.Source = new BitmapImage(new Uri("static/Board2.jpg", UriKind.Relative));
            Canvas.SetLeft(boardImage, 0);
            Canvas.SetTop(boardImage, 0);
            boardCanvas.Children.Add(boardImage);
        }

        private void initalizeFields(int sizeOfField, int sizeOfOffset, int setup)
        {
            int height = sizeOfOffset + (sizeOfField / 2);
            int width = sizeOfOffset + (sizeOfField / 2);
            fields = new Field[8, 8];
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    fields[j, i] = new Field(width, height, null);
                    width += sizeOfField;
                }
                height += sizeOfField;
                width = sizeOfOffset + (sizeOfField / 2);
            }

            for (int i = 0; i < fields.GetLength(1); i++)
            {
                fields[i, 1].piece = new Piece("pawn_b", fields[i, 1]);
            }
            for (int i = 0; i < fields.GetLength(1); i++)
            {
                fields[i, 6].piece = new Piece("pawn_w", fields[i, 6]);
            }
            fields[0, 0].piece = new Piece("rook_b", fields[0, 0]);
            fields[7, 0].piece = new Piece("rook_b", fields[7, 0]);
            fields[0, 7].piece = new Piece("rook_w", fields[0, 7]);
            fields[7, 7].piece = new Piece("rook_w", fields[7, 7]);

            fields[1, 0].piece = new Piece("knight_b", fields[1, 0]);
            fields[6, 0].piece = new Piece("knight_b", fields[6, 0]);
            fields[6, 7].piece = new Piece("knight_w", fields[6, 7]);
            fields[1, 7].piece = new Piece("knight_w", fields[1, 7]);

            fields[2, 0].piece = new Piece("bishop_b", fields[2, 0]);
            fields[5, 0].piece = new Piece("bishop_b", fields[5, 0]);
            fields[5, 7].piece = new Piece("bishop_w", fields[5, 7]);
            fields[2, 7].piece = new Piece("bishop_w", fields[2, 7]);

            fields[3, 0].piece = new Piece("queen_b", fields[3, 0]);
            fields[3, 7].piece = new Piece("queen_w", fields[3, 7]);

            fields[4, 0].piece = new Piece("king_b", fields[4, 0]);
            fields[4, 7].piece = new Piece("king_w", fields[4, 7]);
        }

        public Field findNearestField(double x, double y)
        {
            Field result = (from Field item in fields select item).MinBy(a => a.cartesianDistance(x,y));
            return result;
        }

        private Piece findPieceById(string id)
        {
            var partialResult = from Field field in fields group field by new { piece = field.piece } into p where p.Key.piece is not null select p.Key.piece;
            var result = from item in partialResult where item.id == id select item;
            return result.First();
        }

        public void dropPiece(Image img, Point point)
        {
            Piece piece = findPieceById(img.Name);
            Field newField = findNearestField(point.X, point.Y);
            newField.piece = piece;
            piece.field = newField;
            piece.updateImage();
        }

    }

    internal class Field
    {
        public int x { get; set; }
        public int y { get; set; }
        public Piece piece { get; set; }

        public Field(int x, int y, Piece piece)
        {
            this.x = x;
            this.y = y;
            this.piece = piece;
        }

        public double cartesianDistance(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2));
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

        public void updateImage()
        {
            Canvas.SetLeft(image, field.x - image.Width / 2);
            Canvas.SetTop(image, field.y - image.Height / 2);
        }

    }



}
