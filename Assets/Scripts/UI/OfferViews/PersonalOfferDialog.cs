using Events;
using OffersSystem;
using OffersSystem.Offers;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PersonalOfferDialog : BaseDataView<PersonalOffer>
    {
        [SerializeField] TMP_Text offerTitle;
        [SerializeField] TMP_Text offerCoinsReward;
        [SerializeField] TMP_Text offerStarsReward;
        [SerializeField] TMP_Text offerPrice;
        [SerializeField] Button buyButton;
        
        PersonalOffer _offerData;
        public override void Show(PersonalOffer viewModel)
        {
            _offerData = viewModel;
            offerTitle.text = "Personal offer";
            
            if (viewModel.purchaseItem.CoinsReward > 0)
                offerCoinsReward.text = viewModel.purchaseItem.CoinsReward.ToString();
            else
                offerCoinsReward.gameObject.SetActive(false);
            
            if (viewModel.purchaseItem.StarsReward > 0)
                offerStarsReward.text = viewModel.purchaseItem.StarsReward.ToString();
            else
                offerStarsReward.gameObject.SetActive(false);

            offerPrice.text = $"BUY for {viewModel.purchaseItem.package.currency}{viewModel.purchaseItem.package.price}";
            buyButton.onClick.AddListener(OnBuy);
        }
        
        private void OnBuy()
        {
            EventsMap.Dispatch(GameEvents.PurchaseOfferItem, _offerData.purchaseItem);
            Close();
        }
    }
}