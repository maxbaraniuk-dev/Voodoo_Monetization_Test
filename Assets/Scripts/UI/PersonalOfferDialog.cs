using Events;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using VoodooSDK.Game.Offers;

namespace UI
{
    public class PersonalOfferDialog : BaseDataView<PersonalOffer>
    {
        [SerializeField] TMP_Text offerTitle;
        [SerializeField] TMP_Text offerCoinsReward;
        [SerializeField] TMP_Text offerStarsReward;
        [SerializeField] TMP_Text offerPrice;
        [SerializeField] Button buyButton;
        
        PersonalOffer _offer;
        public override void Show(PersonalOffer viewModel)
        {
            _offer = viewModel;
            offerTitle.text = "Personal offer";
            
            if (viewModel.item.CoinsReward > 0)
                offerCoinsReward.text = viewModel.item.CoinsReward.ToString();
            else
                offerCoinsReward.gameObject.SetActive(false);
            
            if (viewModel.item.StarsReward > 0)
                offerStarsReward.text = viewModel.item.StarsReward.ToString();
            else
                offerStarsReward.gameObject.SetActive(false);

            offerPrice.text = $"BUY for {viewModel.item.package.currency}{viewModel.item.package.price}";
            buyButton.onClick.AddListener(OnBuy);
        }
        
        private void OnBuy()
        {
            EventsMap.Dispatch(GameEvents.PurchaseOfferItem, _offer.item);
        }
    }
}