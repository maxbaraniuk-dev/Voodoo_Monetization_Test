using System.Collections;
using Events;
using Game.Level;
using Game.Maze;
using Game.Player;
using Infrastructure;
using Logs;
using SaveLoad;
using UI;
using UnityEngine;

namespace Game
{
    public class GameSystem : MonoBehaviour, IGameSystem
    {
        [SerializeField] private GameConfig gameConfig;
        
        private GameObject _maze;
        private GameObject _gameBackground;
        private PlayerController _playerController;
        private bool _isGameStarted;
        private bool _isTargetReached;
        
        private float _passedTime;
        private DifficultyLevel _difficultyLevel;

        public void Initialize()
        {
            Context.GetSystem<ILog>().Debug(() => "GameSystem initialized");
        }
        
        public void Dispose() { }

        public void StartNewGame(DifficultyLevel difficultyLevel)
        {
            EventsMap.Subscribe<float>(GameEvents.OnTargetReached, OnTargetReached);
            _isTargetReached = false;
            _passedTime = 0;
            _difficultyLevel = difficultyLevel;
            var mazeData = MazeGenerator.Generate(gameConfig.GetMazeData(difficultyLevel));
            _maze = BuildMaze(mazeData, gameConfig.wallPrefab, gameConfig.pathPrefab, gameConfig.exitPrefab);
            var spawnPosition = MazeGenerator.FindNearestToCenter(mazeData);
            _playerController = Instantiate(gameConfig.playerControllerPrefab, new Vector3(spawnPosition.X, spawnPosition.Y, 0), Quaternion.identity);
            _gameBackground = Instantiate(gameConfig.gameBackground);
            _isGameStarted = true;
        }

        public void ExitGame()
        {
            EventsMap.Unsubscribe<float>(GameEvents.OnTargetReached, OnTargetReached);
            _isGameStarted = false;
            Destroy(_maze);
            Destroy(_playerController.gameObject);
            Destroy(_gameBackground);
        }

        private void Update()
        {
            if (!_isGameStarted || _isTargetReached) 
                return;
            
            _passedTime += Time.deltaTime;
            EventsMap.Dispatch(GameEvents.OnTimeUpdated, _passedTime);
        }

        private void OnTargetReached(float passedDistance)
        {
            StartCoroutine(LevelCompleteFlow(passedDistance));
        }

        private IEnumerator LevelCompleteFlow(float passedDistance)
        {
            _isTargetReached = true;
            var complete = false;
            _playerController.PlayWinAnimation(()=> complete = true);
            yield return new WaitWhile(() => !complete);
            
            Context.GetSystem<IUISystem>().CloseView<GameUI>();
            var levelResultData = new LevelResultData
            {
                difficultyLevel = _difficultyLevel,
                time = _passedTime,
                distance = passedDistance
            };
            Context.GetSystem<ISaveSystem>().SaveCompletedLevel(levelResultData);
            Context.GetSystem<IUISystem>().ShowView<LevelCompleteDialog, LevelResultData>(levelResultData);
        }

        private GameObject BuildMaze(int[,] mazeData, GameObject wallPrefab, GameObject pathPrefab, GameObject exitPrefab)
        {
            var root = new GameObject("Maze");

            var width = mazeData.GetLength(0);
            var height = mazeData.GetLength(1);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cell = mazeData[x, y];
                    GameObject prefab = cell switch
                    {
                        0 => wallPrefab,
                        1 => pathPrefab,
                        2 => exitPrefab,
                        _ => null
                    };

                    if (prefab == null) continue;
                    var go = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity, root.transform);
                    go.name = $"Cell_{x}_{y}";
                }
            }

            return root;
        }
    }
}