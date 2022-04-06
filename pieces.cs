using System;
// using System.Collections;
using System.Collections.Generic;


class PawnPiece : Piece {
    public PawnPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♙';
        exists = true;
        type = PieceType.Pawn;
    }
    
    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return FilterCheck(x, y, checkCheck, AllMovesForward(x, y));
    }
}


class BishopPiece : Piece {
    public BishopPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♗';
        exists = true;
        type = PieceType.Bishop;
    }
    
    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return FilterCheck(x, y, checkCheck, AllMovesDiagonal(x, y));
    }
}


class RookPiece : Piece {
    public RookPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♖';
        exists = true;
        type = PieceType.Rook;
    }

    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return FilterCheck(x, y, checkCheck, AllMovesStraight(x, y));
    }
}


class KingPiece : Piece {
    public KingPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♔';
        exists = true;
        type = PieceType.King;
    }
    
    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return FilterCheck(x, y, checkCheck, AllMovesAdjacent(x, y));
    }
}


class QueenPiece : Piece {
    public QueenPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♕';
        exists = true;
        type = PieceType.Queen;
    }

    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        List<int[]> moves1 = AllMovesStraight(x, y);
        List<int[]> moves2 = AllMovesDiagonal(x, y);
        List<int[]> allMoves = new List<int[]>(moves1.Count + moves2.Count);
        allMoves.AddRange(moves1);
        allMoves.AddRange(moves2);
        return FilterCheck(x, y, checkCheck, allMoves);
    }
}


class KnightPiece : Piece {
    public KnightPiece(Turn owner, Position position) : base(owner, position) {
        icon = '♘';
        exists = true;
        type = PieceType.Knight;
    }

    public override List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return FilterCheck(x, y, checkCheck, AllMovesL(x, y));
    }
}


class EmptyPiece : Piece {
    public EmptyPiece(Position position) : base(Turn.None, position) {
        icon = ' ';
    }
}