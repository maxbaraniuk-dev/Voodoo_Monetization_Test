using Events;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace UI
{
    public class LobbyView : BaseDataView<UserData>
    {
        [SerializeField] TMP_Text coinsText;
        [SerializeField] TMP_Text starsText;
        [SerializeField] Button playButton;
        [SerializeField] Button leaderboardButton;
        
        public override void Show(UserData viewModel)
        {
            coinsText.text = viewModel.coins.ToString();
            starsText.text = viewModel.stars.ToString();
            playButton.onClick.AddListener(OnPlayButton);
            leaderboardButton.onClick.AddListener(() => EventsMap.Dispatch(UIEvents.OnShowResults));
        }

        public override void UpdateView(UserData viewModel)
        {
            coinsText.text = viewModel.coins.ToString();
            starsText.text = viewModel.stars.ToString();
        }

        private void OnPlayButton()
        {
            EventsMap.Dispatch(UIEvents.OnPrepareNewGame);
        }
    }
}