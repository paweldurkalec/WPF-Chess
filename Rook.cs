using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Rook : Piece
    {
        public Rook(string type, Field field, int z = 2) : base(type, field, z)
        {
        }

        public override bool isMovePossible(Field newField, bool checkMode = true)
        {
            if (!base.isMovePossible(newField, checkMode))
            {
                return false;
            }
            if (Variables.board.isFreeBetween(field, newField, "vertically") || Variables.board.isFreeBetween(field, newField, "horizontally"))
            {
                return true;
            }

            return false;
        }

        public override List<Field> getPossibleMoves()
        {
            List<Field> possibleMoves = new List<Field>();
            for(int i=1; i<=Variables.widthOfBoard; i++)
            {
                if(Variables.board.onBoard(field.x, i))
                {
                    if (isMovePossible(Variables.board.getField(i, field.y)))
                    {
                        possibleMoves.Add(Variables.board.getField(i, field.y));
                    }
                }
                if (Variables.board.onBoard(field.x, i))
                {
                    if (isMovePossible(Variables.board.getField(field.x, i)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, i));
                    }
                }
            }
            return possibleMoves;
        }
    }
}
