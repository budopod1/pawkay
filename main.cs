using System;
// using System.Collections;
using System.Collections.Generic;
// You can rerun with 'dotnet run --no-build'


class Program {
    public static void Main(string[] args) {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i <= 100; i++) {
            Console.WriteLine();
        }
        Console.WriteLine(@"Welcome to Lincons chess
Play with bot pawkay or a friend
White pieces are uppercase
Black pieces are lower case

King - K
Pawn - P
Knight - N
Queen - Q
Rook - R
Bishop - B
        ");

        /*
        int choice;
        Console.WriteLine("\nIs white bot pawkay or a human?");
        choice = Input.Option(new string[] {"bot", "human"});
        Console.WriteLine("\nIs black bot pawkay or a human?");
        choice = Input.Option(new string[] {"bot", "human"});
        */
        
        Position position = Position.fromFEN("4k3/4p3/8/3P4/8/8/8/4K3 b - - 0 1");
        Console.WriteLine();
        while (true) {
            Console.WriteLine(position);
            Console.WriteLine($"\nIt's {position.turn}'s turn");
            Console.WriteLine("Input move: start square then end square (eg. d2d4)");
            string move = Console.ReadLine();
            Console.WriteLine();

            if (move.Length != 4) {
                Console.WriteLine("Invalid move length");
                continue;
            }
            
            int[] start = Position.FromSquare(move.Substring(0, 2));
            if (start == null) {
                Console.WriteLine("Invalid start square");
                continue;
            }
            int startX = start[0];
            int startY = start[1];
            
            int[] end = Position.FromSquare(move.Substring(2, 2));
            if (end == null) {
                Console.WriteLine("Invalid end square");
                continue;
            }
            int endX = end[0];
            int endY = end[1];

            /*
            try {
                startX = (int)move[0] - 97;
                startY = Int32.Parse(move[1].ToString()) - 1;
                endX = (int)move[2] - 97;
                endY = Int32.Parse(move[3].ToString()) - 1;
            } catch (System.FormatException) {
                Console.WriteLine("Move does not use correct format\n");
                continue;
            }

            if (!position.IsValid(startX, startY)
                || !position.IsValid(endX, endY)) {
                Console.WriteLine("Cannot move a piece not on the board\n");
                continue;
            }
            */
            
            Piece piece = position.GetPiece(startX, startY);
            if (piece.CanMove(startX, startY, endX, endY)) {
                Move toMove = piece.CreateMove(startX, startY, endX, endY);
                position = toMove.Perform(position);
            } else {
                Console.WriteLine("Invalid move\n");
            }
        }
    }
}
