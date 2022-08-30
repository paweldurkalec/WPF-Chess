using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Rook : Piece
    {
        public Rook(string type, Field field) : base(type, field)
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

            return possibleMoves;
        }
    }
}
