using System;
// using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
// You can rerun with 'dotnet run --no-build'


class Program {
    public static void Main(string[] args) {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        for (int i = 0; i <= 100; i++) {
            Console.WriteLine();
        }
        
        // Program.Test();
        
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
        Player white = new Human(Turn.White);
        Player black = new Pawkay(Turn.Black, 2, PawkayVersion.A);
        
        Console.WriteLine("Enter the FEN (optional)");
        string FEN = Input.PromptDefault("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Position position = Position.fromFEN(FEN);
        Console.WriteLine();
        while (true) {
            Console.WriteLine(position);
            
            if (position.AllMoves().Count != 0) {
                Console.WriteLine($"It's {position.turn}'s turn");
                
                if (position.turn == Turn.White) {
                    position = white.ChooseMove(position).Perform(position);
                } else if (position.turn == Turn.Black) {
                    position = black.ChooseMove(position).Perform(position);
                }
            } else {
                if (position.IsKingCheck(true)) {
                    if (position.turn == Turn.White) {
                        Console.WriteLine($"Win for Black!");
                    } else if (position.turn == Turn.Black) {
                        Console.WriteLine($"Win for White!");
                    }
                } else {
                    Console.WriteLine("Draw!");
                }
                break;
            }
        }
    }

    public static void Test() {
        Position position = Position.fromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        Stopwatch timer = new Stopwatch();
        timer.Start();
        Move move = null;
        for (int i = 0; i < 1000; i++) {
            move = position.AllMoves()[0];
        }
        timer.Stop();

        Console.WriteLine($"AllMoves: {timer.ElapsedMilliseconds/1000.0}");

        timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++) {
            move.Perform(position);
        }
        timer.Stop();

        Console.WriteLine($"Perform: {timer.ElapsedMilliseconds/1000.0}");

        Pawkay bot = new Pawkay(Turn.Black, 5);

        timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++) {
            bot.ScorePosition(position);
        }
        timer.Stop();

        Console.WriteLine($"ScorePosition: {timer.ElapsedMilliseconds/1000.0}");
    }
}
