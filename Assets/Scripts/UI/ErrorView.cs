using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ErrorView : BaseDataView<string>
    {
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private Button okButton;
        public override void Show(string viewModel)
        {
            errorText.text = viewModel;
            okButton.onClick.AddListener(Confirm);
        }

        private void Confirm()
        {
            throw new System.NotImplementedException();
        }
    }
}