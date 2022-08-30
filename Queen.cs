using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Queen : Piece
    {
        public Queen(string type, Field field) : base(type, field)
        {
        }

        public override bool isMovePossible(Field newField, bool checkMode = true)
        {
            if (!base.isMovePossible(newField, checkMode))
            {
                return false;
            }
            if (Variables.board.isFreeBetween(field, newField))
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
