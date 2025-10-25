using Infrastructure;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PurchaseResultPopup : BaseDataView<Result>
    {
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button okButton;
        [SerializeField] private TMP_Text errorText;

        public override void Show(Result viewModel)
        {
            resultText.text = viewModel.Success ? "Purchase complete" : "Purchase error";
            errorText.gameObject.SetActive(!viewModel.Success);
            
            if (!viewModel.Success)
                errorText.text = viewModel.Message;
            
            okButton.onClick.AddListener(Close);
        }
    }
}