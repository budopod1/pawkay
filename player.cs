class Player {
    public Turn turn;
    
    public Player(Turn turn) {
        this.turn = turn;
    }
    
    public virtual Move ChooseMove(Position position) {
        return null;
    }
}
