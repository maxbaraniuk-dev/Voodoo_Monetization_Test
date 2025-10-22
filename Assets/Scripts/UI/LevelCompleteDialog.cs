using Events;
using Game.Level;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LevelCompleteDialog : BaseDataView<LevelResultData>
    {
        [SerializeField] TMP_Text timeText;
        [SerializeField] TMP_Text distanceText;
        [SerializeField] Button confirmButton;

        public override void Show(LevelResultData viewModel)
        {
            timeText.text = StringUtils.FormatTime(viewModel.time);
            distanceText.text = StringUtils.FormatDistance(viewModel.distance);
            confirmButton.onClick.AddListener(OnConfirm);
        }

        private void OnConfirm()
        {
            EventsMap.Dispatch(UIEvents.OnBackToMenu);
            Close();
        }
    }
}