namespace Goodwill.Core
{
    public sealed class GameState
    {
        public GameStateEnum State { get; }
        public string CurrentCompany { get; }
        public static GameState ChoosingExchangePartner = new GameState(GameStateEnum.ChosingExchangePartner);
        public static GameState Pricing = new GameState(GameStateEnum.Pricing);
        public static GameState VotingForManager = new GameState(GameStateEnum.ManagerChange);

        private GameState(GameStateEnum state)
        {
            State = state;
        }

        private GameState(GameStateEnum state, string company)
        {
            State = state;
            CurrentCompany = company;
        }

        public GameState Company(string company)
        {
            return new GameState(State, company);
        }

        private bool Equals(GameState other)
        {
            return State == other.State &&
                (CurrentCompany == null || other.CurrentCompany == null || string.Equals(CurrentCompany, other.CurrentCompany));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is GameState && Equals((GameState)obj);
        }
    }

    public enum GameStateEnum
    {
        Pricing, ManagerChange, ChosingExchangePartner
    }
}