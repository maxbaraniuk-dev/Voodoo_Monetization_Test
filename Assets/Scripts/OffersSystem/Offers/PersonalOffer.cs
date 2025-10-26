using System.Collections.Generic;
using System.Linq;
using Store;
using UI;
using VoodooSDK.DTO.Offers;
using Zenject;

namespace OffersSystem.Offers
{
    public class PersonalOffer : BaseOffer
    {
        [Inject] private IStore _store;
        [Inject] private IUISystem _uiSystem;
        public PurchaseItem purchaseItem;

        public override void Show()
        {
            purchaseItem.package = _store.GetPackage(purchaseItem.packageId);
            _uiSystem.ShowView<PersonalOfferDialog, PersonalOffer>(this);
        }

        public static PersonalOffer FromDto(PersonalOfferData data)
        {
            return new PersonalOffer
            {
                segment = data.segment,
                trigger = data.trigger,
                purchaseItem = PurchaseItem.FromDto(data.item)
            };
        }
    }
}