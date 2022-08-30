using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class King : Piece
    {
        public King(string type, Field field) : base(type, field)
        {
        }

        protected override bool isMovePossible(Field newField)
        {
            if (!base.isMovePossible(newField))
            {
                return false;
            }
            if ((Math.Abs(field.x - newField.x) <= 1 && Math.Abs(field.y - newField.y) <= 1))
            {
                return true;
            }
            return false;
        }
    }
}
