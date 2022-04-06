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

    public virtual List<int[]> AllMoves(int x, int y, bool checkCheck=true) {
        return new List<int[]>();
    }

    public bool CanMove(int startX, int startY, int endX, int endY) {
        if (owner != position.turn) {
            return false;
        }
        
        foreach (int[] move in AllMoves(startX, startY)) {
            if (move[0] == endX && move[1] == endY) {
                return true;
            }
        }
        return false;
    }

    public List<int[]> AllMovesStraight(int x, int y) {
        List<int[]> allMoves = new List<int[]>();

        bool right = true;
        bool left = true;
        bool up = true;
        bool down = true;
        for (int i = 1; i <= 8; i++) {
            if (right) {
                int xp = x + i;
                int yp = y;
                if (position.CanMoveTo(xp, yp)) {
                    allMoves.Add(new int[] {xp, yp});
                } else {
                    right = false;
                }
            }
            if (left) {
                int xp = x - i;
                int yp = y;
                if (position.CanMoveTo(xp, yp)) {
                    allMoves.Add(new int[] {xp, yp});
                } else {
                    left = false;
                }
            }
            if (up) {
                int xp = x;
                int yp = y - i;
                if (position.CanMoveTo(xp, yp)) {
                    allMoves.Add(new int[] {xp, yp});
                } else {
                    up = false;
                }
            }
            if (down) {
                int xp = x;
                int yp = y + i;
                if (position.CanMoveTo(xp, yp)) {
                    allMoves.Add(new int[] {xp, yp});
                } else {
                    down = false;
                }
            }
        }
        
        return allMoves;
    }

    public List<int[]> AllMovesL(int x, int y) {
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

        List<int[]> allMoves = new List<int[]>();

        foreach (int[] lPatern in lPaterns) {
            int xCoord = x + lPatern[0];
            int yCoord = y + lPatern[1];
            if (position.CanMoveTo(xCoord, yCoord)) {
                allMoves.Add(new int[] {xCoord, yCoord});
            }
        }
        return allMoves;
    }

    public List<int[]> AllMovesDiagonal(int x, int y) {
        bool upRight = true;
        bool upLeft = true;
        bool downRight = true;
        bool downLeft = true;

        List<int[]> allMoves = new List<int[]>();
        
        int xCoord;
        int yCoord;
        
        for (int i = 1; i <= 8; i++) {
            if (upRight) {
                xCoord = x + i;
                yCoord = y - i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    upRight = false;
                }
            }
            if (upLeft) {
                xCoord = x - i;
                yCoord = y - i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    upLeft = false;
                }
            }
            if (downRight) {
                xCoord = x + i;
                yCoord = y + i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    downRight = false;
                }
            }
            if (downLeft) {
                xCoord = x - i;
                yCoord = y + i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    downLeft = false;
                }
            }
        }
        
        return allMoves;
    }

    public List<int[]> AllMovesAdjacent(int x, int y) {
        List<int[]> allMoves = new List<int[]>();
        
        for (int xc = -1; xc <= 1; xc++) {
            for (int yc = -1; yc <= 1; yc++) {
                if (position.CanMoveTo(xc + x, yc + y)) {
                    allMoves.Add(new int[] {xc + x, yc + y});
                }
            }
        }

        return allMoves;
    }

    public List<int[]> AllMovesForward(int x, int y) {
        int direction = (owner == Turn.Black) ? -1 : 1;

        List<int[]> allMoves = new List<int[]>();

        int y1;
        int y2;

        y1 = y + direction;
        y2 = y + direction * 2;

        bool noPieceOneAhead = !position.IsPieceAt(x, y1);
        if (position.IsValid(x, y1) && noPieceOneAhead) {
            allMoves.Add(new int[] {x, y1});
        }

        if (!moved && position.IsValid(x, y2) && noPieceOneAhead && !position.IsPieceAt(x, y2)) {
            allMoves.Add(new int[] {x, y2});
        }

        int x1 = x - 1;
        int x2 = x + 1;

        if (position.CanMoveTo(x1, y1) && position.IsPieceAt(x1, y1)) {
            allMoves.Add(new int[] {x1, y1});
        }

        if (position.CanMoveTo(x2, y1) && position.IsPieceAt(x2, y1)) {
            allMoves.Add(new int[] {x2, y1});
        }

        // Check for en passant

        

        return allMoves;
    }

    public List<int[]> FilterCheck(int startX, int startY, bool checkCheck, List<int[]> unfiltered) {
        List<int[]> allMoves = new List<int[]>();
        
        if (checkCheck) {
            foreach (int[] move in unfiltered) {
                if (!position.IsKingCheck(startX, startY, move[0], move[1])) {
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
            default:
                return null;
        }
    }
}