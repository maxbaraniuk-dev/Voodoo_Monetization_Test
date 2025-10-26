using System.Collections.Generic;
using System.Linq;
using VoodooSDK.DTO.Purchasables;
using VoodooSDK.DTO.Store;

namespace OffersSystem
{
    public class PurchaseItem
    {
        public string id;
        public string packageId;
        public bool isFree;
        private List<Reward> _rewards;
        
        public StorePackage package;

        public int CoinsReward => _rewards.FirstOrDefault(reward => reward.type == RewardType.Coins).value;
        public int StarsReward => _rewards.FirstOrDefault(reward => reward.type == RewardType.Stars).value;

        public static PurchaseItem FromDto(PurchaseItemData data)
        {
            return new PurchaseItem
            {
                id = data.id,
                _rewards = data.rewards,
                packageId = data.packageId,
                isFree = data.isFree
            };
        }
    }
}