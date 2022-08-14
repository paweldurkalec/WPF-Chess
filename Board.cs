using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFChess
{

    internal static class Variables
    {
        public static Canvas? boardCanvas;
    }

    internal class Board
    {
        private Canvas boardCanvas;

        private Field[,] fields = new Field[8,8];

        public Board(int sizeOfField, int sizeOfOffset, int setup, Canvas boardCanvas)
        {
            this.initializeBoard(boardCanvas);
            this.initalizeFields(sizeOfField, sizeOfOffset, setup);
            
            
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

        }

        private void findNearestField(int x, int y)
        {

        }


    }

    internal class Field
    {
        public int x { get; set; }
        public int y { get; set; }
        Piece piece;

        Field(int x, int y, Piece piece)
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


        Piece(string imagePath, string type, Field field)
        {
            this.type = type;
            this.field = field;
            this.image = new Image();
            image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            Canvas.SetLeft(image, field.x);
            Canvas.SetTop(image, field.y);
            Variables.boardCanvas.Children.Add(this.image);
        }
    }

}
