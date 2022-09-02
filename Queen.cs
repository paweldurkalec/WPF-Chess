﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Queen : Piece
    {
        public Queen(string type, Field field, int z = 2) : base(type, field, z)
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
            for (int k = 1; k <= Variables.widthOfBoard; k++)
            {
                if (Variables.board.onBoard(field.x, k))
                {
                    if (isMovePossible(Variables.board.getField(k, field.y)))
                    {
                        possibleMoves.Add(Variables.board.getField(k, field.y));
                    }
                }
                if (Variables.board.onBoard(field.x, k))
                {
                    if (isMovePossible(Variables.board.getField(field.x, k)))
                    {
                        possibleMoves.Add(Variables.board.getField(field.x, k));
                    }
                }
            }
            //right up
            int i = field.x;
            int j = field.y;
            while (Variables.board.onBoard(i, j))
            {
                if (isMovePossible(Variables.board.getField(i, j)))
                {
                    possibleMoves.Add(Variables.board.getField(i, j));
                }
                i++;
                j++;
            }
            //right down
            i = field.x;
            j = field.y;
            while (Variables.board.onBoard(i, j))
            {
                if (isMovePossible(Variables.board.getField(i, j)))
                {
                    possibleMoves.Add(Variables.board.getField(i, j));
                }
                i++;
                j--;
            }
            //left down
            i = field.x;
            j = field.y;
            while (Variables.board.onBoard(i, j))
            {
                if (isMovePossible(Variables.board.getField(i, j)))
                {
                    possibleMoves.Add(Variables.board.getField(i, j));
                }
                i--;
                j--;
            }
            //left up
            i = field.x;
            j = field.y;
            while (Variables.board.onBoard(i, j))
            {
                if (isMovePossible(Variables.board.getField(i, j)))
                {
                    possibleMoves.Add(Variables.board.getField(i, j));
                }
                i--;
                j++;
            }
            return possibleMoves;
        }
    }
}
