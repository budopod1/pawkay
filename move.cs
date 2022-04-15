using System;
// using System.Collections;
using System.Collections.Generic;


class Move {
    public int[] startXs;
    public int[] startYs;
    public int[] endXs;
    public int[] endYs;
    public List<int[]> captures;
    public PieceType turnTo;
    public int[] enPassant;
    
    public Move(int[] startXs, int[] startYs, int[] endXs, int[] endYs, List<int[]> captures=null, PieceType turnTo=PieceType.Copy, int[] enPassant=null) {
        this.startXs = startXs;
        this.startYs = startYs;
        this.endXs = endXs;
        this.endYs = endYs;
        this.enPassant = enPassant;
        
        if (captures != null) {
            this.captures = captures;
        } else {
            this.captures = new List<int[]>();
        }

        this.turnTo = turnTo;
    }

    public Position Perform(Position oldPosition) {
        Position newPosition = oldPosition.CopyBoard();
        if (newPosition.turn == Turn.Black) {
            newPosition.turnNum++;
        }
        newPosition.SwapTurn();
        for (int i = 0; i < startXs.Length; i++) {
            int startX = startXs[i];
            int startY = startYs[i];
            int endX = endXs[i];
            int endY = endYs[i];
            Piece oldPiece = newPosition.GetPiece(startX, startY);
            newPosition.SetPiece(startX, startY, new EmptyPiece(newPosition));
            if (turnTo != PieceType.Copy) {
                oldPiece = Piece.Create(turnTo, oldPiece.owner, newPosition);
            }
            newPosition.SetPiece(endX, endY, oldPiece);
        }
        foreach (int[] capture in captures) {
            newPosition.SetPiece(capture[0], capture[1], new EmptyPiece(newPosition));
        }
        newPosition.enPassant = enPassant;
        return newPosition;
    }

    public override string ToString() {
        string output = "";
        
        for (int i = 0; i < startXs.Length; i++) {
            string at = $"({startXs[i]}, {startYs[i]})";
            string to = $"({endXs[i]}, {endYs[i]})";
            output = $"Move piece at {at} to {to}\n";
        }

        
        foreach (int[] capture in captures) {
            output += $"Capture piece at ({capture[0]}, {capture[1]})\n";
        }
        
        return output;
    }
}
