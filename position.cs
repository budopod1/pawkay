using System;
// using System.Collections;
using System.Collections.Generic;


class Position {
    Piece[,] board = new Piece[8, 8];
    public Turn turn = Turn.White;
    
    public Position() {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                board[x, y] = new EmptyPiece(this);
            }
        }
    }

    public bool IsValid(int x, int y) {
        return !(x > 7 || y > 7 || x < 0 || y < 0);
    }

    public bool IsPieceAt(int x, int y) {
        return GetPiece(x, y).exists;
    }

    public bool CanMoveTo(int x, int y) {
        return IsValid(x, y) && GetPiece(x, y).owner != turn;
    }

    public void SetPiece(int x, int y, Piece piece) {
        board[x, y] = piece;
    }

    public Piece GetPiece(int x, int y) {
        return board[x, y];
    }

    public int[] GetKing(Turn turn) {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                if (piece.owner == turn && piece is KingPiece) {
                    return new int[] {x, y};
                }
            }
        }
        return null;
    }

    public bool IsKingCheck(int startX, int startY, int endX, int endY) {
        Position moved = Move(startX, startY, endX, endY);
        int[] kingPosition = moved.GetKing(turn);
        int kingX = kingPosition[0];
        int kingY = kingPosition[1];
        foreach (int[] move in moved.AllMoves(false)) {
            if (move[0] == kingX && move[1] == kingY) {
                return true;
            }
        }
        return false;
    }

    public Position CopyBoard() {
        Position boardCopy = new Position();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                piece = (Piece)piece.Clone();
                piece.position = boardCopy;
                boardCopy.board[x, y] = piece;
            }
        }
        boardCopy.turn = turn;
        return boardCopy;
    }

    public void SwapTurn() {
        if (turn == Turn.White) {
            turn = Turn.Black;
        } else {
            turn = Turn.White;
        }
    }

    public List<int[]> AllMoves(bool checkCheck=true) {
        List<int[]> allMoves = new List<int[]>();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                if (piece.exists && piece.owner == turn) {
                    allMoves.AddRange(piece.AllMoves(x, y, checkCheck));
                }
            }
        }
        return allMoves;
    }

    public Position Move(int startX, int startY, int endX, int endY) {
        Position newPosition = CopyBoard();
        Piece startPiece = newPosition.GetPiece(startX, startY);
        startPiece.moved = true;
        newPosition.board[startX, startY] = new EmptyPiece(newPosition);
        newPosition.board[endX, endY] = startPiece;
        newPosition.SwapTurn();
        return newPosition;
    }

    public override string ToString() {
        string text = "   ";
        for (int x = 0; x < 8; x++) {
            text += (char)(x + 97);
        }
        text += "\n\n";
        for (int y = 8; y > 0; y--) {
            text += $"{y}  ";
            for (int x = 0; x < 8; x++) {
                text += GetPiece(x, y - 1).GetIcon();
            }
            text += $"  {y}\n";
        }
        text += "\n   ";
        for (int x = 0; x < 8; x++) {
            text += (char)(x + 97);
        }
        return text;
    }

    public void DefaultPosition() {
        for (int y = 0; y < 8; y++) {
            Turn turn = Turn.None;
            if (y == 1 || y == 0) {
                turn = Turn.White;
            } else if (y == 6 || y == 7) {
                turn = Turn.Black;
            }
            for (int x = 0; x < 8; x++) {
                Piece piece = new EmptyPiece(this);
                if (y == 1 || y == 6) {
                    piece = new PawnPiece(turn, this);
                } else if (y == 0 || y == 7) {
                    switch(x) {
                        case 0:
                            piece = new RookPiece(turn, this);
                            break;
                        case 1:
                            piece = new KnightPiece(turn, this);
                            break;
                        case 2:
                            piece = new BishopPiece(turn, this);
                            break;
                        case 3:
                            piece = new QueenPiece(turn, this);
                            break;
                        case 4:
                            piece = new KingPiece(turn, this);
                            break;
                        case 5:
                            piece = new BishopPiece(turn, this);
                            break;
                        case 6:
                            piece = new KnightPiece(turn, this);
                            break;
                        case 7:
                            piece = new RookPiece(turn, this);
                            break;
                    }
                }
                board[x, y] = (Piece)piece;
            }
        }
    }
}
