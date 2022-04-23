using System;
// using System.Collections;
using System.Collections.Generic;
using System.Globalization;
// You can rerun with 'dotnet run --no-build'


// r3k2r/1pppp1pp/8/p6B/2b5/3P4/PPP2PPP/R4RK1 b kq - 1 3
// g7g6


class Program {
    public static void Main(string[] args) {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i <= 100; i++) {
            Console.WriteLine();
        }
        Console.WriteLine(@"Welcome to Lincons chess
Play with bot pawkay or a friend
        ");

        /*
        int choice;
        Console.WriteLine("\nIs white bot pawkay or a human?");
        choice = Input.Option(new string[] {"bot", "human"});
        Console.WriteLine("\nIs black bot pawkay or a human?");
        choice = Input.Option(new string[] {"bot", "human"});
        */
        Console.WriteLine("Enter the FEN (optional)");
        string FEN = Input.PromptDefault("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Position position = Position.fromFEN(FEN);
        Console.WriteLine();
        while (true) {
            Console.WriteLine(position);
            Console.WriteLine($"It's {position.turn}'s turn");
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
                    position = possibleMoves[0].Perform(position);
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
                                position = move.Perform(position);
                                break;
                            }
                        }
                    } else {
                        throw new NotImplementedException();
                    }
                }
            } else {
                Console.WriteLine("Invalid move\n");
            }
        }
    }
}
