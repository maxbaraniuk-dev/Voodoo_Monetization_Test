using Game.Level;
using Game.Maze;
using Game.Player;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public MazeData easyMazeData;
        public MazeData mediumMazeData;
        public MazeData hardMazeData;
        
        public int upLockLevelsCoinsCost;
        public int upLockLevelsStarsCost;
        
        public PlayerController playerControllerPrefab;
        public GameObject gameBackground;
        
        public GameObject wallPrefab;
        public GameObject pathPrefab;
        public GameObject exitPrefab;
        
        public MazeData GetMazeData(DifficultyLevel difficultyLevel)
        {
            return difficultyLevel switch
            {
                DifficultyLevel.Easy => easyMazeData,
                DifficultyLevel.Medium => mediumMazeData,
                DifficultyLevel.Hard => hardMazeData,
                _ => null
            };
        }
    }
}