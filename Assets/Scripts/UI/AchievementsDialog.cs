using System.Linq;
using Game;
using Game.Level;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AchievementsDialog : BaseDataView<AchievementsData>
    {
        [SerializeField] private Transform easyLevelRoot;
        [SerializeField] private Transform mediumLevelRoot;
        [SerializeField] private Transform hardLevelRoot;
        
        [SerializeField] private Button okButton;
        
        [SerializeField] private LevelInfoItem levelInfoItemPrefab;
        public override void Show(AchievementsData viewModel)
        {
            okButton.onClick.AddListener(OnClick);
            foreach (var levelResultData in viewModel.levels.OrderBy(data => data.time))
            {
                var root = levelResultData.difficultyLevel switch
                {
                    DifficultyLevel.Easy   => easyLevelRoot,
                    DifficultyLevel.Medium => mediumLevelRoot,
                    DifficultyLevel.Hard   => hardLevelRoot,
                    _                      => null
                };

                var item = Instantiate(levelInfoItemPrefab, root);
                item.SetLevelInfo(levelResultData.time, levelResultData.distance);
            }
        }

        private void OnClick()
        {
            Close();
        }
    }
}