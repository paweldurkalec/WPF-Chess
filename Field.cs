using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFChess
{
    internal class Field
    {
        public int x { get; set; }
        public int y { get; set; }
        public int xOnCanvas { get; set; }
        public int yOnCanvas { get; set; }
        public Piece? piece { get; set; }

        private bool isRectangleShown { get; set; }

        private Rectangle borderRectangle;

        public Field(int x, int y, int xPosition, int yPosition, Piece piece)
        {
            this.x = x;
            this.y = y;
            xOnCanvas = xPosition;
            yOnCanvas = yPosition;
            this.piece = piece;
            borderRectangle = new Rectangle()
            {
                Width = Variables.sizeOfField-10,
                Height = Variables.sizeOfField-10,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Green,
                StrokeThickness = 2,
            };
            Panel.SetZIndex(borderRectangle, 1);
            isRectangleShown = false;

        }
        public void showRectangle()
        {
            if (!isRectangleShown)
            {
                Variables.boardCanvas.Children.Add(borderRectangle);
                Canvas.SetTop(borderRectangle, yOnCanvas - Variables.sizeOfField / 2 + 5);
                Canvas.SetLeft(borderRectangle, xOnCanvas - Variables.sizeOfField / 2 + 5);
                isRectangleShown = true;
            }
        }

        public void hideRectangle()
        {
            if (isRectangleShown)
            {
                Variables.boardCanvas.Children.Remove(borderRectangle);
                isRectangleShown = false;
            }
        }

        public double cartesianDistance(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a - xOnCanvas, 2) + Math.Pow(b - yOnCanvas, 2));
        }
    }
}
