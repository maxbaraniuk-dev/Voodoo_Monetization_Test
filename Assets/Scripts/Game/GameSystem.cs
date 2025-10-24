using Cysharp.Threading.Tasks;
using Events;
using Game.Level;
using Game.Maze;
using Game.Player;
using Infrastructure;
using Logs;
using SaveLoad;
using UI;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameSystem : IGameSystem, ISystem
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private ILog _log;
        [Inject] private IUISystem _uiSystem;
        [Inject] private ISaveSystem _saveSystem;
        
        private GameObject _maze;
        private GameObject _gameBackground;
        private PlayerController _playerController;
        private bool _isGameStarted;
        private bool _isTargetReached;
        
        private float _passedTime;
        private DifficultyLevel _difficultyLevel;

        public void Initialize()
        {
            _log.Debug(() => "GameSystem initialized");
        }
        
        public void Dispose() { }

        public void StartNewGame(DifficultyLevel difficultyLevel)
        {
            EventsMap.Subscribe<float>(GameEvents.OnTargetReached, OnTargetReached);
            _isTargetReached = false;
            _passedTime = 0;
            _difficultyLevel = difficultyLevel;
            var mazeData = MazeGenerator.Generate(_gameConfig.GetMazeData(difficultyLevel));
            _maze = BuildMaze(mazeData, _gameConfig.wallPrefab, _gameConfig.pathPrefab, _gameConfig.exitPrefab);
            var spawnPosition = MazeGenerator.FindNearestToCenter(mazeData);
            _playerController = Object.Instantiate(_gameConfig.playerControllerPrefab, new Vector3(spawnPosition.X, spawnPosition.Y, 0), Quaternion.identity);
            _gameBackground = Object.Instantiate(_gameConfig.gameBackground);
            StartUpdateTimer().Forget();
            _isGameStarted = true;
        }

        public void ExitGame()
        {
            EventsMap.Unsubscribe<float>(GameEvents.OnTargetReached, OnTargetReached);
            _isGameStarted = false;
            Object.Destroy(_maze);
            Object.Destroy(_playerController.gameObject);
            Object.Destroy(_gameBackground);
        }

        private async UniTask StartUpdateTimer()
        {
            while (_isGameStarted && !_isTargetReached)
            {
                await UniTask.NextFrame();
                _passedTime += Time.deltaTime;
                EventsMap.Dispatch(GameEvents.OnTimeUpdated, _passedTime);
            }
        }

        private void OnTargetReached(float passedDistance)
        {
            LevelCompleteFlow(passedDistance).Forget();
        }

        private async UniTask LevelCompleteFlow(float passedDistance)
        {
            _isTargetReached = true;
            var complete = false;
            _playerController.PlayWinAnimation(()=> complete = true);
            await UniTask.WaitUntil(() => complete);
            
            _uiSystem.CloseView<GameUI>();
            var levelResultData = new LevelResultData
            {
                difficultyLevel = _difficultyLevel,
                time = _passedTime,
                distance = passedDistance
            };
            _saveSystem.SaveCompletedLevel(levelResultData);
            _uiSystem.ShowView<LevelCompleteDialog, LevelResultData>(levelResultData);
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
                    var go = Object.Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity, root.transform);
                    go.name = $"Cell_{x}_{y}";
                }
            }

            return root;
        }
    }
}