using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFChess
{
    internal class Pawn : Piece
    {

        private Field[] promotionFields;

        public Pawn(string type, Field field, int z = 2) : base(type, field, z)
        {
        }

        public override void move(Field newField)
        {
            int oldY = field.y;
            bool tookEnPassant = (newField.piece == null && newField.x != field.x) ? true : false;
            base.move(newField);
            if(Math.Abs(field.y - oldY) == 1 && tookEnPassant)
            {
                Pawn toDestroy = (Pawn)Variables.board.getField(field.x, oldY).piece;
                Variables.board.history.addMove(null, Variables.board.getField(field.x, oldY), null, toDestroy, "destroy");
                toDestroy.destroy();
            }
            if (reachedEndOfBoard() && oldY!=field.y)
            {
                //Variables.board.changeTurn();
                promote();
            }
        }

        private bool reachedEndOfBoard()
        {
            string color = Variables.board.rotated ? Variables.board.getSecondColor(this.color) : this.color;
            if (color == "white" && field.y == Variables.heightOfBoard)
            {
                return true;
            }
            else if (color == "black" && field.y == 1)
            {
                return true;
            }
            return false;
        }

        private void promote()
        {
            Variables.board.duringPromotion = this;
            string color = Variables.board.rotated ? Variables.board.getSecondColor(this.color) : this.color;
            Rectangle backgroundRectangle = new Rectangle()
            {
                Width = 1000,
                Height = 1000,
                Fill = Brushes.Transparent,
                Name = "promotionBackground"
            };
            Panel.SetZIndex(backgroundRectangle, 3);
            Variables.boardCanvas.Children.Add(backgroundRectangle);
            Canvas.SetTop(backgroundRectangle, 0);
            Canvas.SetLeft(backgroundRectangle, 0);

            Image promotionImage = new Image();
            promotionImage.Source = new BitmapImage(new Uri("static/promotion_board.jpg", UriKind.Relative));
            promotionImage.Name = "promotionImage";
            Panel.SetZIndex(promotionImage, 4);
            int promotionImageY;
            int promotionImageX = (Variables.widthOfBoard * Variables.sizeOfField + 2 * Variables.sizeOfOffset) / 2 - 2 * Variables.sizeOfField;
            Canvas.SetLeft(promotionImage, promotionImageX);
            if (color == "white")
            {
                promotionImageY = 0;   
            }
            else
            {
                promotionImageY = (int)(Variables.heightOfBoard * Variables.sizeOfField + Variables.sizeOfOffset - promotionImage.ActualHeight);
            }
            Canvas.SetTop(promotionImage, promotionImageY);
            Variables.boardCanvas.Children.Add(promotionImage);
            color = this.color;

            promotionFields = new Field[4];
            int temp = 0;
            for (int j = 1; j < 5; j++)
            {
                promotionFields[j-1] = new Field(-1, -1, promotionImageX+temp+Variables.sizeOfField/2, promotionImageY+Variables.sizeOfField / 2, null);
                temp += Variables.sizeOfField;
            }               
            promotionFields[0].piece = new Rook(color, promotionFields[0],5);
            promotionFields[1].piece = new Knight(color, promotionFields[1],5);
            promotionFields[2].piece = new Bishop(color, promotionFields[2],5);
            promotionFields[3].piece = new Queen(color, promotionFields[3],5);
        }

        public void endPromotion(Image img)
        {
            string id = img.Name;
            var partialResult = from Field field in promotionFields group field by new { piece = field.piece } into p where p.Key.piece is not null select p.Key.piece;
            var result = from item in partialResult where item.id == id select item;
            Piece newPiece = result.First();
            Variables.board.history.lastMoveWasPromotion();

            this.field.piece = newPiece;
            newPiece.field = this.field;
            newPiece.updateImage();
            newPiece.setZIndex(2);
            Variables.boardCanvas.Children.Remove(image);
            Variables.board.duringPromotion = null;
            for (int i = 0; i < 4; i++)
            {
                if (promotionFields[i].piece != newPiece)
                {
                    Variables.boardCanvas.Children.Remove(promotionFields[i].piece.image);
                }
                promotionFields[i] = null;
            }
            promotionFields = null;
            for (int i = 0; i < 2; i++)
            {
                foreach (UIElement child in Variables.boardCanvas.Children)
                {
                    if (child is Image)
                    {
                        Image image = (Image)child;
                        if (image.Name == "promotionImage" || image.Name == "promotionBackground")
                        {
                            Variables.boardCanvas.Children.Remove(image);
                            break;
                        }
                    }
                    else if (child is Rectangle)
                    {
                        Rectangle rect = (Rectangle)child;
                        if (rect.Name == "promotionImage" || rect.Name == "promotionBackground")
                        {
                            Variables.boardCanvas.Children.Remove(rect);
                            break;
                        }
                    }
                }
            }
            if (Variables.autoRotation.IsChecked ?? false)
            {
                Variables.board.rotateBoard();
            }

        }

        public override bool canAttack(Field newField)
        {
            string color;
            if (Variables.board.rotated)
            {
                color = Variables.board.getSecondColor(this.color);
            }
            else
            {
                color = this.color;
            }
            if (!base.isMovePossible(newField, false))
            {
                return false;
            }
            //white attack
            if (color == "white" && newField.y == field.y + 1 && Math.Abs(field.x - newField.x) == 1)
            {
               return true;
            }
            //black attack
            if (color == "black" && newField.y == field.y - 1 && Math.Abs(field.x - newField.x) == 1)
            {
               return true;
            }
            return false;
        }

        public override bool isMovePossible(Field newField, bool checkMode = true)
        {
            string color;
            if (Variables.board.rotated)
            {
                color = Variables.board.getSecondColor(this.color);
            }
            else
            {
                color = this.color;
            }
            if (!base.isMovePossible(newField, checkMode))
            {
                return false;
            }
            if (newField.piece == null)
            {
                if (firstMove)
                {
                    //white 2 forward
                    if (color == "white" && newField.y == field.y + 2 && Variables.board.isFreeBetween(field, newField))
                    {
                        return true;
                    }
                    //black 2 forward
                    if (color == "black" && newField.y == field.y - 2 && Variables.board.isFreeBetween(field, newField))
                    {
                        return true;
                    }
                }
                //white 1 forward
                if (color == "white" && newField.y == field.y + 1 && newField.x == field.x)
                {
                    return true;
                }
                //black 1 forward
                if (color == "black" && newField.y == field.y - 1 && newField.x == field.x)
                {
                    return true;
                }
            }
            if (newField.piece != null)
            {
                //white attack
                if (color == "white" && newField.y == field.y + 1 && Math.Abs(field.x - newField.x) == 1)
                {
                    return true;
                }
                //black attack
                if (color == "black" && newField.y == field.y - 1 && Math.Abs(field.x - newField.x) == 1)
                {
                    return true;
                }
            }
            else if(newField.piece == null)
            {
                Piece piece = Variables.board.getField(newField.x, field.y).piece;
                if (piece != null) 
                {
                    if (piece is Pawn)
                    {
                        if (((Pawn)piece)==Variables.board.pawnJumpedTwoFields)
                        {
                            //white attack
                            if (color == "white" && newField.y == field.y + 1 && Math.Abs(field.x - newField.x) == 1)
                            {
                                return true;
                            }
                            //black attack
                            if (color == "black" && newField.y == field.y - 1 && Math.Abs(field.x - newField.x) == 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override List<Field> getPossibleMoves()
        {
            List<Field> possibleMoves = new List<Field>();
            string color;
            if (Variables.board.rotated)
            {
                color = Variables.board.getSecondColor(this.color);
            }
            else
            {
                color = this.color;
            }
            if (color == "white")
            {
                if (Variables.board.onBoard(field.x - 1, field.y + 1))
                {
                    if(isMovePossible(Variables.board.getField(field.x - 1, field.y + 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x - 1, field.y + 1));
                    }
                }
                if (Variables.board.onBoard(field.x, field.y + 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x, field.y + 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, field.y + 1));
                    }
                }
                if (Variables.board.onBoard(field.x + 1, field.y + 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x + 1, field.y + 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x + 1, field.y + 1));
                    }
                }
                if (Variables.board.onBoard(field.x, field.y + 2))
                {
                    if (isMovePossible(Variables.board.getField(field.x, field.y + 2)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, field.y + 2));
                    }
                }
            }
            else
            {
                if (Variables.board.onBoard(field.x - 1, field.y - 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x - 1, field.y - 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x - 1, field.y - 1));
                    }
                }
                if (Variables.board.onBoard(field.x, field.y - 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x, field.y - 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, field.y - 1));
                    }
                }
                if (Variables.board.onBoard(field.x + 1, field.y - 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x + 1, field.y - 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x + 1, field.y - 1));
                    }
                }
                if (Variables.board.onBoard(field.x, field.y - 2))
                {
                    if (isMovePossible(Variables.board.getField(field.x, field.y - 2)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, field.y - 2));
                    }
                }
            }
            return possibleMoves;
        }
    }
}
