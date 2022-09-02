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

        protected bool firstMove;

        public Piece(string color, Field field, int z=2)
        {
            if (!Variables.board.placePiece(this, field.x, field.y) && !(field.x==-1))
            {
                return;
            }
            firstMove = true;
            id = Variables.getNewId();
            this.color = color;
            this.field = field;
            image = new Image();
            image.Source = new BitmapImage(new Uri(Variables.piecePaths[this.GetType().Name + "_" + this.color], UriKind.Relative));
            Panel.SetZIndex(image, z);
            image.Name = id;
            image.Width = 50;
            image.Height = 80;
            updateImage();
            image.MouseMove += new MouseEventHandler(Variables.mouseHandler);
            Variables.boardCanvas.Children.Add(image);
        }

        public virtual bool canAttack(Field newField)
        {
            return isMovePossible(newField, false);
        }

        public virtual bool isMovePossible(Field newField, bool checkMode=true)
        {
            if (newField.piece != null)
            {
                if (newField.piece.color == color || newField.piece is King)
                {
                    return false;
                }
            }
            bool isCheck = false;
            if (checkMode)
            {
                // temporary changes to check if king is under attack after move
                Piece secondPiece = newField.piece;
                Field oldField = field;
                newField.piece = this;
                field.piece = null;
                field = newField;

                isCheck = Variables.board.isCheck(color);

                // restoring changes
                field.piece = secondPiece;
                field = oldField;
                field.piece = this;
            }

            return !isCheck;
        }

        public virtual void move(Field newField)
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
            this.firstMove = false;
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

        public void showPossibleMoves()
        {
            List<Field> possibleMoves = getPossibleMoves();
            foreach(Field possibleMove in possibleMoves)
            {
                possibleMove.showRectangle();
            }
        }

        public virtual List<Field> getPossibleMoves()
        {
            return null;
        }

    }
}
