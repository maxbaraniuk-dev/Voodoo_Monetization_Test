using System.Collections.Generic;
using System.Linq;
using VoodooSDK.Store;

namespace VoodooSDK.Game.Purchasables
{
    public struct PurchaseItem
    {
        public string id;
        public List<Reward> rewards;
        public string packageId;
        public bool isFree;
        public bool purchased;
        
        public StorePackage package;

        public int CoinsReward => rewards.FirstOrDefault(reward => reward.type == RewardType.Coins).value;
        public int StarsReward => rewards.FirstOrDefault(reward => reward.type == RewardType.Stars).value;
    }
}