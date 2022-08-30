using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Pawn : Piece
    {
        
        public Pawn(string type, Field field) : base(type, field)
        {
        }

        public override bool canAttack(Field newField)
        {
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
            return false;
        }

        public override List<Field> getPossibleMoves()
        {
            List<Field> possibleMoves = new List<Field>();  
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
                if (Variables.board.onBoard(field.x - 1, field.y - 1))
                {
                    if (isMovePossible(Variables.board.getField(field.x - 1, field.y - 1)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x - 1, field.y - 1));
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
