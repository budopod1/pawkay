using System;
// using System.Collections;
using System.Collections.Generic;


class Position {
    Piece[,] board = new Piece[8, 8];
    public Turn turn = Turn.White;
    
    public int turnNum = 1;
    public int[] enPassant = null;
    public bool whiteKingside = true;
    public bool whiteQueenside = true;
    public bool blackKingside = true;
    public bool blackQueenside = true;
    
    
    public Position() {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                SetPiece(x, y, new EmptyPiece(this));
            }
        }
    }

    public static int[] FromSquare(string square) {
        if (square.Length == 2) {
            try {
                int x = (int)square[0] - 97;
                int y = Int32.Parse(square[1].ToString()) - 1;
                if (Position.IsValid(x, y)) {
                    return new int[] {x, y};
                } else {
                    return null;
                }
            } catch (System.FormatException) {
                return null;
            }
        } else {
            return null;
        }
    }

    public static string ToSquare(int x, int y) {
        return ((char)(x + 97)).ToString() + (y + 1).ToString();
    }

    public static bool IsValid(int x, int y) {
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
        return moved.IsCheckAt(kingX, kingY);
    }

    public bool IsCheckAt(int x, int y, bool switchTurn=false) {
        foreach (Move move in AllMoves(false, !switchTurn)) {
            for (int i = 0; i < move.endXs.Length; i++) {
                if (move.endXs[i] == x && move.endYs[i] == y) {
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
                boardCopy.SetPiece(x, y, piece);
            }
        }
        
        boardCopy.turn = turn;
        boardCopy.turnNum = turnNum;
        boardCopy.enPassant = enPassant;
        
        boardCopy.whiteKingside = whiteKingside;
        boardCopy.whiteQueenside = whiteQueenside;
        boardCopy.blackKingside = blackKingside;
        boardCopy.blackQueenside = blackQueenside;
        
        return boardCopy;
    }

    public void SwapTurn() {
        if (turn == Turn.White) {
            turn = Turn.Black;
        } else {
            turn = Turn.White;
        }
    }

    public List<Move> AllMoves(bool checkCheck=true, bool currentTurn=true) {
        List<Move> allMoves = new List<Move>();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                bool sameTurn = piece.owner == turn;
                if (!currentTurn) {
                    sameTurn = !sameTurn;
                }
                if (piece.exists && sameTurn) {
                    allMoves.AddRange(piece.AllMoves(x, y, checkCheck));
                }
            }
        }
        return allMoves;
    }

    public override string ToString() {
        string text = "   ";
        for (int x = 0; x < 8; x++) {
            text += (char)(x + 97);
        }
        text += "\n\n";
        for (int y = 8; y > 0; y--) {
            int outY = y;
            text += $"{outY}  ";
            for (int x = 0; x < 8; x++) {
                text += GetPiece(x, y - 1).GetIcon();
            }
            text += $"  {outY}\n";
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
                        position.SetPiece(x, y, new EmptyPiece(position));
                        x += 1;
                    }
                } else {
                    char piece = char.ToLower(pieceChar);
                    Turn pieceOwner = pieceChar == piece ? Turn.Black : Turn.White;
                    PieceType pieceType = Piece.FromLetter(piece.ToString());
                    position.SetPiece(x, y, Piece.Create(pieceType, pieceOwner, position));
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

        // Castling loading here
        
        string enPassant = parts[3];
        if (enPassant != "-") {
            position.enPassant = Position.FromSquare(enPassant);
        }

        return position;
    }
}
