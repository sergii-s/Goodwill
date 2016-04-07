using System.Collections.Generic;
using System.Linq;

namespace Goodwill.Core.Rounds
{
    public class BiddingRound : IGameRound
    {
        private readonly Goodwill _goodwill;
        private readonly string _company;
        private readonly IDictionary<string, int> _playerBids = new Dictionary<string, int>();

        public BiddingRound(Goodwill goodwill, string company)
        {
            _goodwill = goodwill;
            _company = company;
        }

        public void SetPrice(string player, int price)
        {
            _playerBids[player] = price;
        }

        public void FinishRound()
        {
            var lowestPrice = _playerBids.Values.Min();
            var highestPrice = _playerBids.Values.Max();
            if (lowestPrice == highestPrice)
            {
                return;
            }

            //TODO check they have actions
            var sellers = _playerBids.Where(x => x.Value == lowestPrice).ToList();
            var buyers = _playerBids.Where(x => x.Value == highestPrice).ToList().Shuffle();
            
            foreach (var seller in sellers)
            {
                if (buyers.Count == 0) break;
                var buyer = buyers.Pick();

                var transactionPrice = (lowestPrice + highestPrice) / 2;
                var sellerPlayer = _goodwill.Players.WithName(seller.Key);
                var buyerPlayer = _goodwill.Players.WithName(buyer.Key);

                var action = sellerPlayer.Actions.Pick(x => x.Company.Name == _company).First();
                sellerPlayer.Money += transactionPrice;
                buyerPlayer.Actions.Add(action);
                buyerPlayer.Money -= transactionPrice;
            }
        }
    }
}