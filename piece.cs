using System;
// using System.Collections;
using System.Collections.Generic;


class Piece {
    public Position position;
    public Turn owner;
    public char icon = ' ';
    public bool exists = false;
    public bool moved = false;
    public PieceType type = PieceType.Empty;
    
    public Piece(Turn owner, Position position) {
        this.position = position;
        this.owner = owner;
    }

    public Piece Clone() {
        return Piece.Create(type, owner, position);
    }

    public char GetIcon() {
        if (owner == Turn.Black) {
            return (char)((int)icon + 6);
        }
        return icon;
    }

    public virtual List<Move> AllMoves(int x, int y, bool checkCheck=true) {
        return new List<Move>();
    }

    public List<Move> MovesFromTo(int startX, int startY, int endX, int endY) {
        List<Move> allMoves = new List<Move>();
        
        if (owner != position.turn) {
            return allMoves;
        }

        foreach (Move move in AllMoves(startX, startY)) {
            for (int i = 0; i < move.startXs.Length; i++) {
                if (move.endXs[i] == endX && move.endYs[i] == endY
                   && move.startXs[i] == startX && move.startYs[i] == startY) {
                    allMoves.Add(move);
                    break;
                }
            }
            // if (move.startXs.Length == 1
            //     && move.endXs[0] == endX
            //     && move.endYs[0] == endY) {
            // }
        }
        
        return allMoves;
    }

    public static PieceType FromLetter(string letter) {
        try {
            return new Dictionary<string, PieceType>() {
                {"q", PieceType.Queen},
                {"p", PieceType.Pawn},
                {"n", PieceType.Knight},
                {"k", PieceType.King},
                {"b", PieceType.Bishop},
                {"r", PieceType.Rook}
            }[letter.ToLower()];
        } catch (KeyNotFoundException) {
            return PieceType.Empty;
        }
    }

    public Move CreateMove(int x, int y, int endX, int endY) {
        return new Move(new int[] {x}, new int[] {y}, new int[] {endX}, new int[] {endY});
    }

    public List<Move> AllMovesStraight(int x, int y) {
        List<Move> allMoves = new List<Move>();

        bool right = true;
        bool left = true;
        bool up = true;
        bool down = true;
        for (int i = 1; i <= 8; i++) {
            if (right) {
                int xp = x + i;
                int yp = y;
                
                if (position.CanMoveTo(xp, yp)) {
                    if (position.IsPieceAt(xp, yp)) {
                        right = false;
                    }
                    allMoves.Add(CreateMove(x, y, xp, yp));
                } else {
                    right = false;
                }
            }
            if (left) {
                int xp = x - i;
                int yp = y;
                
                if (position.CanMoveTo(xp, yp)) {
                    if (position.IsPieceAt(xp, yp)) {
                        left = false;
                    }
                    allMoves.Add(CreateMove(x, y, xp, yp));
                } else {
                    left = false;
                }
            }
            if (up) {
                int xp = x;
                int yp = y - i;
                
                if (position.CanMoveTo(xp, yp)) {
                    if (position.IsPieceAt(xp, yp)) {
                        up = false;
                    }
                    allMoves.Add(CreateMove(x, y, xp, yp));
                } else {
                    up = false;
                }
            }
            if (down) {
                int xp = x;
                int yp = y + i;
                
                if (position.CanMoveTo(xp, yp)) {
                    if (position.IsPieceAt(xp, yp)) {
                        down = false;
                    }
                    allMoves.Add(CreateMove(x, y, xp, yp));
                } else {
                    down = false;
                }
            }
        }
        
        return allMoves;
    }

    public List<Move> AllMovesKnight(int x, int y) {
        List<int[]> lPaterns = new List<int[]> {
            new int[] {2, 1},
            new int[] {2, -1},
            new int[] {-2, 1},
            new int[] {-2, -1},
            new int[] {1, 2},
            new int[] {1, -2},
            new int[] {-1, 2},
            new int[] {-1, -2}
        };

        List<Move> allMoves = new List<Move>();

        foreach (int[] lPatern in lPaterns) {
            int xCoord = x + lPatern[0];
            int yCoord = y + lPatern[1];
            if (position.CanMoveTo(xCoord, yCoord)) {
                allMoves.Add(CreateMove(x, y, xCoord, yCoord));
            }
        }
        return allMoves;
    }

    public List<Move> AllMovesDiagonal(int x, int y) {
        bool upRight = true;
        bool upLeft = true;
        bool downRight = true;
        bool downLeft = true;

        List<Move> allMoves = new List<Move>();
        
        int xCoord;
        int yCoord;
        
        for (int i = 1; i <= 8; i++) {
            if (upRight) {
                xCoord = x + i;
                yCoord = y - i;
                
                if (position.CanMoveTo(xCoord, yCoord)) {
                    if (position.IsPieceAt(xCoord, yCoord)) {
                        upRight = false;
                    }
                    allMoves.Add(CreateMove(x, y, xCoord, yCoord));
                } else {
                    upRight = false;
                }
            }
            if (upLeft) {
                xCoord = x - i;
                yCoord = y - i;
                
                if (position.CanMoveTo(xCoord, yCoord)) {
                    if (position.IsPieceAt(xCoord, yCoord)) {
                        upLeft = false;
                    }
                    allMoves.Add(CreateMove(x, y, xCoord, yCoord));
                } else {
                    upLeft = false;
                }
            }
            if (downRight) {
                xCoord = x + i;
                yCoord = y + i;
                
                if (position.CanMoveTo(xCoord, yCoord)) {
                    if (position.IsPieceAt(xCoord, yCoord)) {
                        downRight = false;
                    }
                    allMoves.Add(CreateMove(x, y, xCoord, yCoord));
                } else {
                    downRight = false;
                }
            }
            if (downLeft) {
                xCoord = x - i;
                yCoord = y + i;
                
                if (position.CanMoveTo(xCoord, yCoord)) {
                    if (position.IsPieceAt(xCoord, yCoord)) {
                        downLeft = false;
                    }
                    allMoves.Add(CreateMove(x, y, xCoord, yCoord));
                } else {
                    downLeft = false;
                }
            }
        }
        
        return allMoves;
    }

    public List<Move> AllMovesKing(int x, int y, bool checkCheck=true) {
        List<Move> allMoves = new List<Move>();
        
        for (int xc = -1; xc <= 1; xc++) {
            for (int yc = -1; yc <= 1; yc++) {
                if (position.CanMoveTo(xc + x, yc + y)) {
                    allMoves.Add(CreateMove(x, y, xc + x, yc + y));
                }
            }
        }

        if (!checkCheck) {
            return allMoves;
        }

        bool kingside = false;
        bool queenside = false;
        int kingEndX = 0;
        int rookStartX = 0;
        int rookEndX = 0;
        
        if (owner == Turn.White) {
            kingside = position.whiteKingside;
            queenside = position.whiteQueenside;
        } else if (owner == Turn.Black) {
            kingside = position.blackKingside;
            queenside = position.blackQueenside;
        }

        if (kingside) {
            kingEndX = 6;
            rookStartX = 7;
            rookEndX = 5;

            int kingStart = Math.Min(x, kingEndX);
            int kingEnd = Math.Max(x, kingEndX);
            
            // < because last pos is check auto
            for (int i = kingStart; i <= kingEnd; i++) {
                if (i != 0 && i != 7 && i != 4) {
                    if (position.GetPiece(i, y).exists) {
                        kingside = false;
                    }
                }
                kingside = !position.IsCheckAt(i, y, true) && kingside;
            }

            if (kingside) {
                allMoves.Add(new Move(
                    new int[] {x, rookStartX},
                    new int[] {y, y},
                    new int[] {kingEndX, rookEndX},
                    new int[] {y, y}
                ));
            }
        }
        
        if (queenside) {
            kingEndX = 2;
            rookStartX = 0;
            rookEndX = 3;

            int kingStart = Math.Min(x, kingEndX);
            int kingEnd = Math.Max(x, kingEndX);
            
            // < because last pos is check auto
            for (int i = kingStart; i <= kingEnd; i++) {
                if (i != 0 && i != 7 && i != 4) {
                    if (position.GetPiece(i, y).exists) {
                        queenside = false;
                    }
                }
                queenside = !position.IsCheckAt(i, y, true) && queenside;
            }

            if (queenside) {
                allMoves.Add(new Move(
                    new int[] {x, rookStartX},
                    new int[] {y, y},
                    new int[] {kingEndX, rookEndX},
                    new int[] {y, y}
                ));
            }
        }

        return allMoves;
    }

    public List<Move> AllMovesPawn(int x, int y) {
        int direction = (owner == Turn.Black) ? -1 : 1;

        List<Move> allMoves = new List<Move>();

        int y1;
        int y2;

        y1 = y + direction;
        y2 = y + direction * 2;

        bool noPieceOneAhead = !position.IsPieceAt(x, y1);
        if (Position.IsValid(x, y1) && noPieceOneAhead) {
            allMoves.Add(CreateMove(x, y, x, y1));
        }
        
        if ((y == 1 || y == 6) && Position.IsValid(x, y2) && noPieceOneAhead && !position.IsPieceAt(x, y2)) {
            Move move = CreateMove(x, y, x, y2);
            move.enPassant = new int[] {x, y1};
            allMoves.Add(move);
        }

        int x1 = x - 1;
        int x2 = x + 1;

        if (position.CanMoveTo(x1, y1)) {
            if (position.IsPieceAt(x1, y1)) {
                allMoves.Add(CreateMove(x, y, x1, y1));
            } else {
                if (position.enPassant != null
                    && position.enPassant[0] == x1 && position.enPassant[1] == y1) {
                    Move move = CreateMove(x, y, x1, y1);
                    move.captures = new List<int[]>() {new int[] {x1, y}};
                    allMoves.Add(move);
                }
            }
        }

        if (position.CanMoveTo(x2, y1) && position.IsPieceAt(x2, y1)) {
            allMoves.Add(CreateMove(x, y, x2, y1));
        }

        if (position.CanMoveTo(x2, y1)) {
            if (position.IsPieceAt(x2, y1)) {
                allMoves.Add(CreateMove(x, y, x2, y1));
            } else {
                if (position.enPassant != null
                    && position.enPassant[0] == x2 && position.enPassant[1] == y1) {
                    Move move = CreateMove(x, y, x2, y1);
                    move.captures = new List<int[]>() {new int[] {x2, y}};
                    allMoves.Add(move);
                }
            }
        }

        List<Move> finalMoves = new List<Move>();
        List<PieceType> promoteTo = new List<PieceType>() {
            PieceType.Queen,
            PieceType.Bishop,
            PieceType.Rook,
            PieceType.Knight
        };
        foreach (Move move in allMoves) {
            bool isAtEnd = false;
            foreach (int endY in move.endYs) {
                if (endY == 0 || endY == 7) {
                    isAtEnd = true;
                    foreach (PieceType pieceType in promoteTo) {
                        Move copy = move.Clone();
                        copy.turnTo = pieceType;
                        finalMoves.Add(copy);
                    }
                }
            }
            if (!isAtEnd) {
                finalMoves.Add(move);
            }
        }
        
        return finalMoves;
    }

    public List<Move> FilterCheck(int startX, int startY, bool checkCheck, List<Move> unfiltered) {
        List<Move> allMoves = new List<Move>();
        if (checkCheck) {
            foreach (Move move in unfiltered) {
                if (!position.IsKingCheck(move)) {
                    allMoves.Add(move);
                }
            }
            return allMoves;
        } else {
            return unfiltered;
        }
    }

    public static Piece Create(PieceType type, Turn owner, Position position) {
        switch(type) {
            case PieceType.Pawn:
                return new PawnPiece(owner, position);
            case PieceType.Bishop:
                return new BishopPiece(owner, position);
            case PieceType.King:
                return new KingPiece(owner, position);
            case PieceType.Queen:
                return new QueenPiece(owner, position);
            case PieceType.Knight:
                return new KnightPiece(owner, position);
            case PieceType.Rook:
                return new RookPiece(owner, position);
            case PieceType.Empty:
                return new EmptyPiece(position);
            default:
                return null;
        }
    }
}