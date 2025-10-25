using Events;
using TMPro;
using UI.Core;
using UnityEngine;
using Utils;

namespace UI
{
    public class GameUI : BaseView
    {
        [SerializeField] TMP_Text timeText;
        [SerializeField] TMP_Text distanceText;

        public override void Show()
        {
            EventsMap.Subscribe<float>(GameEvents.PlayerDistanceUpdated, PassedDistanceUpdateListener);
            EventsMap.Subscribe<float>(GameEvents.TimeUpdated, TimeUpdateListener);
        }

        protected override void CloseInternal()
        {
            EventsMap.Unsubscribe<float>(GameEvents.PlayerDistanceUpdated, PassedDistanceUpdateListener);
            EventsMap.Unsubscribe<float>(GameEvents.TimeUpdated, TimeUpdateListener);
        }

        private void PassedDistanceUpdateListener(float distance)
        {
            distanceText.text = StringUtils.FormatDistance(distance);
        }
        
        private void TimeUpdateListener(float time)
        {
            timeText.text = StringUtils.FormatTime(time);
        }
    }
}