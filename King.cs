using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class King : Piece
    {
        public King(string type, Field field, int z = 2) : base(type, field, z)
        {
        }

        public override bool isMovePossible(Field newField, bool checkMode = true)
        {
            if (!base.isMovePossible(newField, checkMode))
            {
                return false;
            }
            if ((Math.Abs(field.x - newField.x) <= 1 && Math.Abs(field.y - newField.y) <= 1))
            {
                return true;
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
            return possibleMoves;
        }
    }
}
