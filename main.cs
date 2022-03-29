using System;
// using System.Collections;
using System.Collections.Generic;
// You can rerun with 'dotnet run --no-build'


enum Turn {
    Black,
    White,
    None
}


class Piece : ICloneable {
    public Position position;
    public Turn owner;
    public string icon = "";
    public bool exists = false;
    public bool moved = false;
    
    public Piece(Turn owner, Position position) {
        this.position = position;
        this.owner = owner;
    }

    public string GetIcon() {
        if (owner == Turn.Black) {
            return icon.ToLower();
        }
        return icon;
    }

    public object Clone() { // Stackoverflow is always right?
        return this.MemberwiseClone();
    }

    public virtual List<int[]> AllMoves(int x, int y) {
        return new List<int[]>();
    }

    public bool CanMove(int startX, int startY, int endX, int endY) {
        // Console.WriteLine(startX);
        // Console.WriteLine(startY);
        // Console.WriteLine(endX);
        // Console.WriteLine(endY);
        foreach (int[] move in AllMoves(startX, startY)) {
            // Console.WriteLine("move:");
            // Console.WriteLine(move[0]);
            // Console.WriteLine(move[1]);
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
        for (int i = 0; i <= 8; i++) {
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
        
        for (int i = 0; i <= 8; i++) {
            if (upRight) {
                int xCoord = x + i;
                int yCoord = y - i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    upRight = false;
                }
            }
            if (upLeft) {
                int xCoord = x - i;
                int yCoord = y - i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    upLeft = false;
                }
            }
            if (downRight) {
                int xCoord = x + i;
                int yCoord = y + i;

                if (position.CanMoveTo(xCoord, yCoord)) {
                    allMoves.Add(new int[] {xCoord, yCoord});
                } else {
                    downRight = false;
                }
            }
            if (downLeft) {
                int xCoord = x - i;
                int yCoord = y + i;

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
                if (position.CanMoveTo(xc, yc)) {
                    allMoves.Add(new int[] {xc, yc});
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

        return allMoves;
    }
}


class PawnPiece : Piece {
    public PawnPiece(Turn owner, Position position) : base(owner, position) {
        icon = "P";
        exists = true;
    }
    
    public override List<int[]> AllMoves(int x, int y) {
        return AllMovesForward(x, y);
    }
}


class BishopPiece : Piece {
    public BishopPiece(Turn owner, Position position) : base(owner, position) {
        icon = "B";
        exists = true;
    }
    
    public override List<int[]> AllMoves(int x, int y) {
        return AllMovesDiagonal(x, y);
    }
}


class RookPiece : Piece {
    public RookPiece(Turn owner, Position position) : base(owner, position) {
        icon = "R";
        exists = true;
    }

    public override List<int[]> AllMoves(int x, int y) {
        return AllMovesStraight(x, y);
    }
}


class KingPiece : Piece {
    public KingPiece(Turn owner, Position position) : base(owner, position) {
        icon = "K";
        exists = true;
    }

    public override List<int[]> AllMoves(int x, int y) {
        return AllMovesAdjacent(x, y);
    }
}


class QueenPiece : Piece {
    public QueenPiece(Turn owner, Position position) : base(owner, position) {
        icon = "Q";
        exists = true;
    }

    public override List<int[]> AllMoves(int x, int y) {
        List<int[]> moves1 = AllMovesStraight(x, y);
        List<int[]> moves2 = AllMovesStraight(x, y);
        List<int[]> allMoves = new List<int[]>(moves1.Count + moves2.Count);
        allMoves.AddRange(moves1);
        allMoves.AddRange(moves2);
        return allMoves;
    }
}


class KnightPiece : Piece {
    public KnightPiece(Turn owner, Position position) : base(owner, position) {
        icon = "N";
        exists = true;
    }

    public override List<int[]> AllMoves(int x, int y) {
        return AllMovesL(x, y);
    }
}


class EmptyPiece : Piece {
    public EmptyPiece(Position position) : base(Turn.None, position) {
        icon = " ";
    }
}


class Position {
    Piece[,] board = new Piece[8, 8];
    Turn turn = Turn.White;
    
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

    public Piece GetPiece(int x, int y) {
        return board[x, y];
    }

    public void SetPiece(int x, int y, Piece piece) {
        board[x, y] = piece;
    }

    public Position CopyBoard() {
        Position boardCopy = new Position();
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                object piece = GetPiece(x, y).Clone();
                boardCopy.SetPiece(x, y, (Piece)piece);
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

    public void AllMoves() {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Piece piece = GetPiece(x, y);
                if (piece.exists && piece.owner == turn) {
                    
                }
            }
        }
    }

    public bool CanMove(int startX, int startY, int endX, int endY) {
        Piece startPiece = GetPiece(startX, startY);
        Piece endPiece = GetPiece(endX, endY);
        // Console.WriteLine(startPiece);
        // Console.WriteLine(endPiece);
        // Console.WriteLine(!(startPiece.owner == endPiece.owner));
        // Console.WriteLine(!(startPiece.owner == Turn.None));
        // Console.WriteLine((startPiece.owner == turn));
        // Console.WriteLine(startPiece.owner);
        // Console.WriteLine(endPiece.owner);
        // Console.WriteLine(turn);
        if (!(startPiece.owner == endPiece.owner) && !(startPiece.owner == Turn.None) && (startPiece.owner == turn)) {
            return startPiece.CanMove(startX, startY, endX, endY);
        }
        return false;
    }

    public Position Move(int startX, int startY, int endX, int endY) {
        Position newPosition = CopyBoard();
        Piece startPiece = GetPiece(startX, startY);
        startPiece.moved = true;
        newPosition.SetPiece(startX, startY, new EmptyPiece(this));
        newPosition.SetPiece(endX, endY, startPiece);
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
                SetPiece(x, y, piece);
            }
        }
    }
}


class Program {
    public static void Main(string[] args) {
        Position pos = new Position();
        pos.DefaultPosition();
        Console.WriteLine();
        while (true) {
            Console.WriteLine(pos);
            Console.WriteLine("\nInput move: start square then"
                              + " end square (eg. d2d4)");
            string move = Console.ReadLine();
            Console.WriteLine();
            int startX = (int)move[0] - 97;
            int startY = Int32.Parse(move[1].ToString()) - 1;
            int endX = (int)move[2] - 97;
            int endY = Int32.Parse(move[3].ToString()) - 1;
            if (pos.CanMove(startX, startY, endX, endY)) {
                pos = pos.Move(startX, startY, endX, endY);
            } else {
                Console.WriteLine("Invalid move\n");
            }
        }
    }
}
