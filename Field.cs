using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
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
}
