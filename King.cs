using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class King : Piece
    {

        public string castle;
        public King(string type, Field field, int z = 2) : base(type, field, z)
        {
            castle = "none";
        }

        public override void move(Field newField)
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

            if(this.firstMove && castle != "none")
            {
                int a = castle == "left" ? 1 : -1;
                string color;
                if (Variables.board.rotated)
                {
                    color = Variables.board.getSecondColor(this.color);
                }
                else
                {
                    color = this.color;
                }
                Field rookField = getRookField(color, castle);
                rookField.piece.move(Variables.board.getField(newField.x+a, newField.y));              
            }

            this.firstMove = false;
            Piece? removedPiece = newField.piece;
            newField.piece = this;
            field.piece = null;
            Field prevField = field;
            field = newField;
            updateImage();
            Variables.board.changeTurn();
            Variables.board.history.addMove(prevField, newField, this, removedPiece, "castle");
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
            if ((Math.Abs(field.x - newField.x) <= 1 && Math.Abs(field.y - newField.y) <= 1))
            {
                return true;
            }

            //castle
            if (this.firstMove && Math.Abs(field.x - newField.x) == 2 && field.y == newField.y)
            {
                string castleSide = newField.x < field.x ? "left" : "right";
                Field rookField = getRookField(color, castleSide);

                if (rookField.piece != null)
                {
                    if (rookField.piece is Rook 
                        && rookField.piece.firstMove
                        && Variables.board.isFreeBetween(this.field, rookField, "horizontally")
                        && isCastleUnderCheck(newField, this.color)==false)
                    {
                        castle = castleSide;
                        return true;
                    }
                }
            }

            return false;
        }

        private Field getRookField(string color, string side)
        {
            Field result;
            if(color == "white")
            {
                result = side == "left" ? Variables.board.getField(1, 1) : Variables.board.getField(8, 1);
            }
            else
            {
                result = side == "left" ? Variables.board.getField(1, 8) : Variables.board.getField(8, 8);
            }
            return result;
        }

        private bool isCastleUnderCheck(Field newField, string color)
        {
            int a;
            a = newField.x > field.x ? 1 : -1;
            for(int i = a; Math.Abs(i) < 3; i+=a)
            {
                if (Variables.board.isUnderAttack(Variables.board.getField(field.x + i, field.y),color))
                {
                    return true;
                }
            }
            return false;
        }

        public override List<Field> getPossibleMoves()
        {
            List<Field> possibleMoves = new List<Field>();
            for (int i = field.x - 1; i <= field.x + 1; i++)
            {
                for (int j = field.y - 1; j <= field.y + 1; j++)
                {
                    if (Variables.board.onBoard(i, j))
                    {
                        if (isMovePossible(Variables.board.getField(i, j)))
                        {
                            possibleMoves.Add(Variables.board.getField(i, j));
                        }
                    }
                }
            }
            //castle
            if (Variables.board.onBoard(field.x-2, field.y))
            {
                if (isMovePossible(Variables.board.getField(field.x - 2, field.y)))
                {
                    possibleMoves.Add(Variables.board.getField(field.x - 2, field.y));
                }
            }
            if (Variables.board.onBoard(field.x + 2, field.y))
            {
                if (isMovePossible(Variables.board.getField(field.x + 2, field.y)))
                {
                    possibleMoves.Add(Variables.board.getField(field.x + 2, field.y));
                }
            }

            return possibleMoves;
        }
    }
}
