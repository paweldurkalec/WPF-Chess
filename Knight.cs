using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Knight : Piece
    {
        public Knight(string type, Field field) : base(type, field)
        {
        }

        public override bool isMovePossible(Field newField, bool checkMode = true)
        {
            if (!base.isMovePossible(newField, checkMode))
            {
                return false;
            }
            if ((Math.Abs(field.x - newField.x) == 1 && Math.Abs(field.y - newField.y) == 2) ||
                (Math.Abs(field.x - newField.x) == 2 && Math.Abs(field.y - newField.y) == 1))
            {
                return true;
            }
            return false;
        }
        public override List<Field> getPossibleMoves()
        {
            List<Field> possibleMoves = new List<Field>();
            int[] offsets = new int[16] {-2, 1, -1, 2, 1, 2, 2, 1, 2, -1, 1, -2, -1, -2, -2, -1};
            if (Variables.board.onBoard(field.x - 1, field.y + 1))
            {
                for (int i = 0; i < offsets.Length; i += 2)
                {
                    if (isMovePossible(Variables.board.getField(field.x + offsets[i], field.y + offsets[i+1])))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x + offsets[i], field.y + offsets[i + 1]));
                    }
                }
            }
            return possibleMoves;
        }
    }
}
