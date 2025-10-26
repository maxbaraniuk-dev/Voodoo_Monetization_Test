using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Events;
using Infrastructure;
using Logs;
using OffersSystem.Offers;
using Store;
using UI;
using User;
using VoodooSDK;
using VoodooSDK.DTO.Offers;
using Zenject;

namespace OffersSystem
{
    public class OffersSystem : IOffersSystem, ISystem
    {
        [Inject] private IStore _store;
        [Inject] private IUISystem _uiSystem;
        [Inject] private IUserSystem _userSystem;
        [Inject] private ILog _log;
        [Inject] private DiContainer _container;
        private List<BaseOffer> _offers;
        public void Initialize()
        {
            EventsMap.Subscribe<OfferTrigger>(GameEvents.OfferTrigger, OnOfferTriggered);
            EventsMap.Subscribe<PurchaseItem>(GameEvents.PurchaseOfferItem, OnPurchaseOfferItem);
            _log.Debug(() => "OffersSystem initialized");
        }

        public void Dispose()
        {
            EventsMap.Unsubscribe<OfferTrigger>(GameEvents.OfferTrigger, OnOfferTriggered);
            EventsMap.Unsubscribe<PurchaseItem>(GameEvents.PurchaseOfferItem, OnPurchaseOfferItem);
        }

        public async UniTask<Result> LoadOffers()
        {
            var offersResult = await MonetizationServer.GetAllOffers();
            if (!offersResult.Success)
            {
                _log.Error(() => $"Load offers error. {offersResult.Message}");
                return Result.FailedResult(offersResult.Message);
            }

            if (offersResult.Payload == null)
            {
                _log.Error(() => "Load offers error. Offers are empty");
                return Result.FailedResult("Offers are empty");
            }
            
            _offers = offersResult.Payload.Select(BaseOffer.CreateFromData).ToList();
            _offers.ForEach(offer => _container.Inject(offer));
            return Result.SuccessResult();
        }

        private void OnOfferTriggered(OfferTrigger obj)
        {
            var offerToShow = _offers.FirstOrDefault(offer => offer.trigger == obj && _userSystem.GetUserData().segments.Contains(offer.segment));
            if (offerToShow == null)
            {
                _log.Warning(() => $"There is no offer for trigger {obj}");
                return;
            }
            
            offerToShow.Show();
        }
        
        private void OnPurchaseOfferItem(PurchaseItem itemData)
        {
            Purchase(itemData).Forget();
        }
 
        private async UniTask Purchase(PurchaseItem itemData)
        {
            var purchaseResult = await PurchaseOfferItem(itemData);
            if (purchaseResult.Success)
                _log.Debug(() => "Purchase complete");
            else
                _log.Error(() => $"Purchase error. {purchaseResult.Message}");
            
            _uiSystem.ShowView<PurchaseResultPopup, Result>(purchaseResult);
        }

        private async UniTask<Result> PurchaseOfferItem(PurchaseItem itemData)
        {
            var purchaseResult = await _store.Purchase(itemData.packageId);
            
            if (!purchaseResult.Success)
                return Result.FailedResult(purchaseResult.Message);

            var serverValidateResult = await MonetizationServer.PurchaseOfferItem(itemData.id);
            
            if (!serverValidateResult.Success)
                return Result.FailedResult(serverValidateResult.Message);
            
            serverValidateResult.Payload.rewards
                                .ForEach(reward => _userSystem.AddReward(reward));
            
            _uiSystem.GetView<LobbyView>().UpdateView(_userSystem.GetUserData());
            return Result.SuccessResult();
        }
    }
}