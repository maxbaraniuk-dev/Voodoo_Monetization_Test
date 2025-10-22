using Events;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyMainDialog : BaseView
    {
        [SerializeField] Button playButton;
        [SerializeField] Button leaderboardButton;

        public override void Show()
        {
            playButton.onClick.AddListener(OnPlayButton);
            leaderboardButton.onClick.AddListener(() => EventsMap.Dispatch(UIEvents.OnShowResults));
        }

        private void OnPlayButton()
        {
            EventsMap.Dispatch(UIEvents.OnPrepareNewGame);
            Close();
        }
    }
}