using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPFChess
{

    internal static class Variables
    {
        public static Canvas? boardCanvas;
        public static WPFChess.MainWindow.MouseMoveEventHandler mouseHandler =;
    }

    internal class Board
    {
        private Canvas boardCanvas;

        private Field[,] fields;

        public Board(int sizeOfField, int sizeOfOffset, int setup, Canvas boardCanvas, WPFChess.MainWindow.MouseMoveEventHandler handler)
        {
            this.initializeBoard(boardCanvas);
            this.initalizeFields(sizeOfField, sizeOfOffset, setup);
            Variables.mouseHandler = handler;
        }

        private void initializeBoard(Canvas boardCanvas)
        {
            Variables.boardCanvas = boardCanvas;
            Image boardImage = new Image();
            boardImage.Source = new BitmapImage(new Uri("static/Board.jpg", UriKind.Relative));
            Canvas.SetLeft(boardImage, 0);
            Canvas.SetTop(boardImage, 0);
            boardCanvas.Children.Add(boardImage);
        }

        private void initalizeFields(int sizeOfField, int sizeOfOffset, int setup)
        {
            int height = sizeOfOffset + (sizeOfField / 2);
            int width = sizeOfField + (sizeOfField / 2);
            fields = new Field[8, 8];
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    fields[j, i] = new Field(width, height, null);
                    width += sizeOfField;
                }
                height += sizeOfField;
            }

            fields[0, 0].piece = new Piece("static/knight_b.png", "knight", fields[0, 0]);

        }

        private void findNearestField(int x, int y)
        {

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
    }

    internal class Piece
    {
        public Image image { get; set; } = null;

        public string type { get; set; }

        public Field field { get; set; }


        public Piece(string imagePath, string type, Field field)
        {
            this.type = type;
            this.field = field;
            image = new Image();
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            Canvas.SetLeft(image, field.x);
            Canvas.SetTop(image, field.y);
            image.MouseMove += new MouseEventHandler(Variables.mouseHandler);
            Variables.boardCanvas.Children.Add(image);
        }

    }



}
