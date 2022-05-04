using System;
// using System.Collections;
using System.Collections.Generic;
// You can rerun with 'dotnet run --no-build'


enum PawkayVersion {
    A
}


class Pawkay : Player {
    int depth = 5;
    PawkayVersion version = PawkayVersion.A;
    
    public Pawkay(Turn turn, int depth=5, PawkayVersion version=PawkayVersion.A) : base(turn) {
        this.depth = depth;
        this.version = version;
    }
                      
    public override Move ChooseMove(Position position) {
        double bestScore = Double.NegativeInfinity;
        Move bestMove = null;
        List<Move> possibleMoves = position.AllMoves();
        int i = 1;
        foreach (Move move in possibleMoves) {
            Console.WriteLine($"Pawkay is on move {i} out of {possibleMoves.Count} moves");
            double thisScore = TreeScore(move.Perform(position), depth);
            if (thisScore > bestScore) {
                bestMove = move;
                bestScore = thisScore;
            }
            i++;
        }
        return bestMove;
    }

    public double TreeScore(Position position, int depth) {
        if (depth == 0) {
            return ScorePosition(position);
        } else {
            double? bestScore = null;
            foreach (Move move in position.AllMoves()) {
                double thisScore = TreeScore(move.Perform(position), depth - 1);
                if (position.turn == turn) { // Might have to change to !=
                    if (bestScore == null || thisScore > bestScore) {
                        bestScore = thisScore;
                    }
                } else {
                    if (bestScore == null || thisScore < bestScore) {
                        bestScore = thisScore;
                    }
                }
            }
            if (bestScore == null) {
                if (position.IsKingCheck(true)) {
                    if (position.turn == turn) {
                        return double.NegativeInfinity;
                    } else {
                        return double.PositiveInfinity;
                    }
                }
                return 0;
            }
            return (double)bestScore;
            /*
            Move chosenMove;
            double chosenScore = Double.NegativeInfinity;
            foreach (Move move in position.AllMoves()) {
                Position newPosition = move.Perform(position);
                double score = ScorePosition(newPosition);
                if (score)
            }
            */
        }
    }

    public double ScorePosition(Position position) {
        // Console.WriteLine(position);
        switch(version) {
            case PawkayVersion.A:
                double score = 0;
                for (int x = 0; x < 8; x++) {
                    for (int y = 0; y < 8; y++) {
                        Piece piece = position.GetPiece(x, y);
                        double val = 0;
                        
                        switch (piece.type) {
                            case PieceType.Pawn:
                                val = 1;
                                break;
                            case PieceType.Bishop:
                                val = 3;
                                break;
                            case PieceType.Knight:
                                val = 3;
                                break;
                            case PieceType.Queen:
                                val = 9;
                                break;
                            case PieceType.Rook:
                                val = 5;
                                break;
                        }

                        // val += val * (Math.Abs(3.5 - x) / 4);
                        // val += val * (Math.Abs(3.5 - y) / 4);
                        
                        if (piece.owner != turn) {
                            val *= -1;
                        }

                        score += val;
                    }
                }
                // score += (double)position.AllMoves(false, true).Count / 20.0;
                return score;
            default:
                return 0.0;
        }
    }
}
