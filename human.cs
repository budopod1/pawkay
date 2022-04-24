using System;
// using System.Collections;
using System.Collections.Generic;
using System.Globalization;


class Human : Player {
    public Human(Turn turn) : base(turn) {}
                      
    public override Move ChooseMove(Position position) {
        while (true) {
            Console.WriteLine("Input move: start square then end square (eg. d2d4)");
            string notatedMove = Console.ReadLine();
    
            if (notatedMove.Length != 4) {
                Console.WriteLine("Invalid move length");
                continue;
            }
            
            int[] start = Position.FromSquare(notatedMove.Substring(0, 2));
            if (start == null) {
                Console.WriteLine("Invalid start square");
                continue;
            }
            int startX = start[0];
            int startY = start[1];
            
            int[] end = Position.FromSquare(notatedMove.Substring(2, 2));
            if (end == null) {
                Console.WriteLine("Invalid end square");
                continue;
            }
            int endX = end[0];
            int endY = end[1];
    
            // Console.WriteLine($"{startX},{startY} {endX},{endY}");
            
            Piece piece = position.GetPiece(startX, startY);
            List<Move> possibleMoves = piece.MovesFromTo(startX, startY, endX, endY);
            if (possibleMoves.Count > 0) {
                if (possibleMoves.Count == 1) {
                    return possibleMoves[0];
                } else {
                    if ((endY == 0 || endY == 7)
                        && piece.type == PieceType.Pawn) {
                        Console.WriteLine("What do you want to promote to?");
                        PieceType newType;
                        while(true) {
                            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                            string newTypeString = Console.ReadLine().ToLower();
                            newTypeString = textInfo.ToTitleCase(newTypeString);
                            bool isValid = Enum.TryParse(newTypeString, out newType);
                            if (isValid
                                && newType != PieceType.Empty
                                && newType != PieceType.Copy) {
                                break;
                            } else {
                                Console.WriteLine("Not a piece type");
                            }
                        }
                        foreach (Move move in possibleMoves) {
                            if (move.turnTo == newType) {
                                return move;
                            }
                        }
                    } else {
                        throw new NotImplementedException();
                    }
                }
            } else {
                Console.WriteLine("Invalid move");
            }
        }
    }
}
