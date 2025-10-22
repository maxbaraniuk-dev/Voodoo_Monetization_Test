using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class LevelInfoItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelTime;
        [SerializeField] private TMP_Text levelDistance;

        public void SetLevelInfo(float time, float distance)
        {
            levelTime.text = StringUtils.FormatTime(time);
            levelDistance.text = StringUtils.FormatDistance(distance);
        }
    }
}