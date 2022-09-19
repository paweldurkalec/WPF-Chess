using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChess
{
    internal class Move
    {
        Field startField;
        Field endField;
        Piece movingPiece;
        Piece? removedPiece;
        public string type { get; set; }

        public Pawn pawnJumpedTwoFields { get; set; }

        public Move(Field startField, Field endField, Piece movingPiece, Piece? removedPiece, string type)
        {
            this.startField = startField;
            this.endField = endField;
            this.movingPiece = movingPiece;
            this.removedPiece = removedPiece;
            this.type = type;
            pawnJumpedTwoFields = Variables.board.pawnJumpedTwoFields;
        }

        public void doMove()
        {
            if(type == "destroy")
            {
                endField.piece = null;
                removedPiece.destroy();
                return;
            }
            this.startField.piece = null;
            this.endField.piece = movingPiece;
            this.movingPiece.field = endField;
            this.movingPiece.updateImage();
            this.movingPiece.firstMove = false;
            if(removedPiece != null)
            {
                removedPiece.destroy();
            }
            Variables.board.pawnJumpedTwoFields = pawnJumpedTwoFields;
            Variables.board.changeTurn();
        }

        public void undoMove()
        {
            if (type == "destroy")
            {
                Variables.board.pawnJumpedTwoFields = (Pawn)removedPiece;
                endField.piece = removedPiece;
                Variables.boardCanvas.Children.Add(removedPiece.image);
                removedPiece.updateImage();
                return;
            }
            this.startField.piece = movingPiece;
            movingPiece.field = this.startField;
            this.endField.piece = removedPiece;
            if (removedPiece is not null)
            {
                removedPiece.field = this.endField;
                Variables.boardCanvas.Children.Add(removedPiece.image);
                removedPiece.updateImage();
            }
            if (type != "normal")
            {
                movingPiece.firstMove = true;
            }
            movingPiece.updateImage();
            Variables.board.changeTurn();
        }
    }

    internal class MovesHistory
    {
        Stack<Move> previousMoves;
        Stack<Move> nextMoves;

        public MovesHistory()
        {
            previousMoves = new Stack<Move>();
            nextMoves = new Stack<Move>();
        }

        public void addMove(Field startField, Field endField, Piece movingPiece, Piece? removedPiece, string type="normal")
        {
            nextMoves = new Stack<Move>();
            previousMoves.Push(new Move(startField, endField, movingPiece, removedPiece, type));
        }

        public void previousMove()
        {
            Move move;
            if (previousMoves.Count > 0)
            {
                move = previousMoves.Pop();
                move.undoMove();
                nextMoves.Push(move);
                Variables.board.pawnJumpedTwoFields = null;
                if (move.type == "castle" || move.type == "destroy")
                {
                    move = previousMoves.Pop();
                    move.undoMove();
                    nextMoves.Push(move);
                }
            }

            if (previousMoves.Count > 0)
            {
                move = previousMoves.Pop();
                Variables.board.pawnJumpedTwoFields = move.pawnJumpedTwoFields;
                previousMoves.Push(move);
            }
        }

        public void nextMove()
        {
            Move move;
            if (nextMoves.Count > 0)
            {
                move = nextMoves.Pop();
                move.doMove();
                previousMoves.Push(move);
            }

            //castle
            if (nextMoves.Count > 0)
            {
                move = nextMoves.Pop();
                if (move.type == "castle" || move.type=="destroy")
                {
                    move.doMove();
                    previousMoves.Push(move);
                }
                else
                {
                    nextMoves.Push(move);
                }
            }
        }
    }


}
