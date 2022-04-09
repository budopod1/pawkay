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

    public bool IsKingCheck(Move moveToCheck) {
        Position moved = moveToCheck.Perform(this);
        int[] kingPosition = moved.GetKing(turn);
        int kingX = kingPosition[0];
        int kingY = kingPosition[1];
        foreach (Move move in moved.AllMoves(false)) {
            for (int i = 0; i < move.endXs.Length; i++) {
                if (move.endXs[i] == kingX && move.endYs[i] == kingY) {
                    return true;
                }
            }
        }
        return false;
    }

    public Position CopyBoard() {
        Position boardCopy = new Position();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                piece = piece.Clone();
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

    public List<Move> AllMoves(bool checkCheck=true) {
        List<Move> allMoves = new List<Move>();
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

    public static Position fromFEN(string FEN) {
        string[] parts = FEN.Split(" ");
        Position position = new Position();

        string[] rows = parts[0].Split("/");
        int y = 7;
        foreach (string row in rows) {
            int x = 0;
            foreach (char pieceChar in row) {
                int emptyNum;
                if (int.TryParse(pieceChar.ToString(), out emptyNum)) {
                    for (int i = 0; i < emptyNum; i++) {
                        position.board[x, y] = new EmptyPiece(position);
                        x += 1;
                    }
                } else {
                    char piece = char.ToLower(pieceChar);
                    Turn pieceOwner = pieceChar == piece ? Turn.Black : Turn.White;
                    PieceType pieceType = new Dictionary<char, PieceType>() {
                        {'k', PieceType.King},
                        {'p', PieceType.Pawn},
                        {'n', PieceType.Knight},
                        {'b', PieceType.Bishop},
                        {'q', PieceType.Queen},
                        {'r', PieceType.Rook}
                    }[piece];
                    position.board[x, y] = Piece.Create(pieceType, pieceOwner, position);
                    x += 1;
                }
            }
            y -= 1;
        }
        
        if (parts[1] == "w") {
            position.turn = Turn.White;
        } else {
            position.turn = Turn.Black;
        }

        return position;
    }
}
