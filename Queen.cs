﻿using System;
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

        protected override bool isMovePossible(Field newField)
        {
            if (!base.isMovePossible(newField))
            {
                return false;
            }
            if (Variables.board.isFreeBetween(field, newField))
            {
                return true;
            }
            return false;
        }
    }
}
