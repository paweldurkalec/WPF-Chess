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
    internal abstract class Piece
    {
        public Image image { get; set; } = null;

        public Field field { get; set; }

        public string id { get; set; }

        public string color { get; set; }


        public Piece(string color, Field field)
        {
            if (!Variables.board.placePiece(this, field.x, field.y))
            {
                return;
            }
            this.id = Variables.getNewId();
            this.color = color;
            this.field = field;
            image = new Image();
            image.Source = new BitmapImage(new Uri(Variables.piecePaths[this.GetType().Name + "_" + this.color], UriKind.Relative));
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
                if (newField.piece.color == color || newField.piece is King)
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
}
