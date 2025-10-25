using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Events;
using Infrastructure;
using Logs;
using Store;
using UI;
using User;
using VoodooSDK;
using VoodooSDK.Game.Offers;
using VoodooSDK.Game.Purchasables;
using Zenject;

namespace Offers
{
    public class OffersSystem : IOffersSystem, ISystem
    {
        [Inject] private IStore _store;
        [Inject] private IUISystem _uiSystem;
        [Inject] private IUserSystem _userSystem;
        [Inject] private ILog _log;
        private List<BaseOffer> _offers;
        public void Initialize()
        {
            EventsMap.Subscribe<OfferTrigger>(GameEvents.OfferTrigger, OnOfferTriggered);
            EventsMap.Subscribe<PurchaseItem>(GameEvents.PurchaseOfferItem, OnPurchaseOfferItem);
        }

        public void Dispose()
        {
            EventsMap.Unsubscribe<OfferTrigger>(GameEvents.OfferTrigger, OnOfferTriggered);
            EventsMap.Unsubscribe<PurchaseItem>(GameEvents.PurchaseOfferItem, OnPurchaseOfferItem);
        }

        public async UniTask<Result> LoadOffers()
        {
            var offersResult = await MonetizationSDK.GetAllOffers();
            if (!offersResult.Success)
                return Result.FailedResult(offersResult.Message);
            
            if (offersResult.Payload == null)
                return Result.FailedResult("Offers are empty");
            
            _offers = offersResult.Payload;
            return Result.SuccessResult();
        }

        private void OnOfferTriggered(OfferTrigger obj)
        {
            var offerToShow = _offers.FirstOrDefault(offer => offer.trigger == obj);
            if (offerToShow == null)
                return;
            
            ShowOffer(offerToShow);
        }
        
        private void OnPurchaseOfferItem(PurchaseItem item)
        {
            Purchase(item).Forget();
        }
        
        private void ShowOffer(BaseOffer offer)
        {
            switch (offer.type)
            {
                case OfferType.Personal:
                    ShowPersonalOffer(offer as PersonalOffer);
                    break;
                case OfferType.MultiplePersonal:
                    ShowMultiplePersonalOffers(offer as MultiplePersonalOffer);
                    break;
                case OfferType.Chained:
                    break;
                case OfferType.Endless:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask Purchase(PurchaseItem item)
        {
            var purchaseResult = await PurchaseOfferItem(item);
            if (purchaseResult.Success)
                _log.Debug(() => "Purchase complete");
            else
                _log.Error(() => $"Purchase error. {purchaseResult.Message}");
            
            _uiSystem.ShowView<PurchaseResultPopup, Result>(purchaseResult);
        }

        private async UniTask<Result> PurchaseOfferItem(PurchaseItem item)
        {
            var purchaseResult = await _store.Purchase(item.packageId);
            
            if (!purchaseResult.Success)
                return Result.FailedResult(purchaseResult.Message);

            var serverValidateResult = await MonetizationSDK.PurchaseOfferItem(item.id);
            
            if (!serverValidateResult.Success)
                return Result.FailedResult(serverValidateResult.Message);
            
            serverValidateResult.Payload.rewards
                                .ForEach(reward => _userSystem.AddReward(reward));
            
            _uiSystem.CloseView<PersonalOfferDialog>();
            _uiSystem.GetView<LobbyView>().UpdateView(_userSystem.GetUserData());
            return Result.SuccessResult();
        }

        private void ShowPersonalOffer(PersonalOffer offer)
        {
            offer.item.package = _store.GetPackage(offer.item.packageId);
            _uiSystem.ShowView<PersonalOfferDialog, PersonalOffer>(offer);
        }
        
        void ShowMultiplePersonalOffers(MultiplePersonalOffer offer)
        {
        }
    }
}