using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Pawn : Piece
    {
        private bool firstMove;
        public Pawn(string type, Field field) : base(type, field)
        {
            firstMove = true;
        }

        protected override bool isMovePossible(Field newField)
        {
            if (!base.isMovePossible(newField))
            {
                return false;
            }
            if (firstMove)
            {
                //white 2 forward
                if (color == "white" && newField.y == field.y + 2 && Variables.board.isFreeBetween(field, newField))
                {
                    firstMove = false;
                    return true;
                }
                //black 2 forward
                if (color == "black" && newField.y == field.y - 2 && Variables.board.isFreeBetween(field, newField))
                {
                    firstMove = false;
                    return true;
                }
            }
            //white 1 forward
            if (color == "white" && newField.y == field.y + 1 && newField.x == field.x)
            {
                firstMove = false;
                return true;
            }
            //black 1 forward
            if (color == "black" && newField.y == field.y - 1 && newField.x == field.x)
            {
                firstMove = false;
                return true;
            }

            if (newField.piece != null)
            {
                //white attack
                if (color == "white" && newField.y == field.y + 1 && Math.Abs(field.x - newField.x) == 1)
                {
                    firstMove = false;
                    return true;
                }
                //black attack
                if (color == "black" && newField.y == field.y - 1 && Math.Abs(field.x - newField.x) == 1)
                {
                    firstMove = false;
                    return true;
                }
            }
            return false;
        }
    }
}
