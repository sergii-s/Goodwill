using System.Collections.Generic;
using System.Linq;

namespace Goodwill.Core.Rounds
{
    public class VoteManagerRound : IGameRound
    {
        private readonly Goodwill _goodwill;
        private readonly string _company;
        private readonly IDictionary<PlayerManagerVote, int> _playerVotes = new Dictionary<PlayerManagerVote, int>();

        public VoteManagerRound(Goodwill goodwill, string company)
        {
            _goodwill = goodwill;
            _company = company;
        }

        public void VoteManager(string player, string manager)
        {
            _playerVotes[new PlayerManagerVote(player, manager)] = _goodwill.Players.WithName(player).Actions.OfCompany(_company).Count();
        }

        private struct PlayerManagerVote
        {
            private readonly string _player;
            private readonly string _manager;

            public PlayerManagerVote(string player, string manager)
            {
                _player = player;
                _manager = manager;
            }

            private bool Equals(PlayerManagerVote other)
            {
                return string.Equals(_player, other._player) && string.Equals(_manager, other._manager);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is PlayerManagerVote && Equals((PlayerManagerVote) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_player?.GetHashCode() ?? 0)*397) ^ (_manager?.GetHashCode() ?? 0);
                }
            }
        }

        public void FinishRound()
        {
            throw new System.NotImplementedException();
        }

        public GameState State => new GameState { Round = EGameRound.VoteManager, Company = _company };
    }
}