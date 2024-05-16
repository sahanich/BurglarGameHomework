using UnityEngine;

namespace BurglarGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private BurglarGameSettings GameSettings;
        [SerializeField]
        private PinSetView PinSetView;
        [SerializeField]
        private ToolSetView ToolSetView;
        [SerializeField]
        private GameOverView GameOverView;
        [SerializeField]
        private GameTimerView GameTimerView;
        [SerializeField]
        private SecondsToLoseView SecondsToLoseView;

        private GameState _gameState;
        private BurglarGameEventsHandler _eventsHandler;

        private void Awake()
        {
            _eventsHandler = new();
            PinSetView.Init(GameSettings);
            ToolSetView.Init(GameSettings, _eventsHandler);
            GameOverView.Init(GameSettings, _eventsHandler);
            GameTimerView.Init(_eventsHandler);
            SecondsToLoseView.Init(GameSettings);
        }

        private void OnEnable()
        {
            _eventsHandler.ToolUseConfirmed += OnToolUsing;
            _eventsHandler.RestartGameRequested += OnRestartGameRequested;
            _eventsHandler.GameTimerUpdated += OnGameTimerUpdated;
        }

        private void OnDisable()
        {
            _eventsHandler.ToolUseConfirmed -= OnToolUsing;
            _eventsHandler.RestartGameRequested -= OnRestartGameRequested;
        }

        private void Start()
        {
            RestartGame();
        }

        private void RestartGame()
        {
            StopAllCoroutines();

            GameOverView.Hide();

            _gameState = CreateGameState();

            PinSetView.SetInitialState(_gameState);
            ToolSetView.SetState(_gameState);
            SecondsToLoseView.SetState(_gameState);

            PlayGame();
        }

        private void PlayGame()
        {
            GameTimerView.StartGameTimer();
            ToolSetView.SetToolsInteractable(true);
        }

        private void StopGame()
        {
            GameTimerView.StopGameTimer();
            ToolSetView.SetToolsInteractable(false);
        }

        private GameState CreateGameState()
        {
            int[] pinValues = CreatePinStates(GameSettings.PinCount, GameSettings.Tools);

            GameState gameState = new();
            gameState.SetPinValues(pinValues);
            gameState.SetToolInfos(GameSettings.Tools);

            return gameState;
        }

        private void ShuffleArray(object[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, array.Length);
                object tempElement = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = tempElement;
            }
        }

        private bool IsRevertedToolApplyingResultInPinValuesRange(ToolInfo toolInfo, int[] pinValues)
        {
            bool resultPinValueIsInRange = true;

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                int resultPinValue = pinValue - toolChangeValue;
                if (resultPinValue < GameSettings.MinPinValue
                    || resultPinValue > GameSettings.MaxPinValue)
                {
                    resultPinValueIsInRange = false;
                    break;
                }
            }

            return resultPinValueIsInRange;
        }

        private bool TryApplyToolReverted(ToolInfo toolInfo, int[] pinValues)
        {
            if (!IsRevertedToolApplyingResultInPinValuesRange(toolInfo, pinValues))
            {
                return false;
            }

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                pinValues[i] = pinValue - toolChangeValue;
            }
            return true;

        }

        private int[] CreatePinStates(int pinCount, ToolInfo[] toolInfos)
        {
            int[] pinValues = new int[pinCount];

            for (int i = 0; i < pinValues.Length; i++)
            {
                pinValues[i] = GameSettings.TargetPinValue;
            }

            ToolInfo[] randomOrderedToolInfos = (ToolInfo[])toolInfos.Clone();
            ShuffleArray(randomOrderedToolInfos);

            int currentToolIndex = 0;
            for (int i = 0; i < GameSettings.CodingIterationCount; i++)
            {
                int tryCountToApplyTool = randomOrderedToolInfos.Length;

                while (tryCountToApplyTool > 0)
                {
                    ToolInfo toolInfo = randomOrderedToolInfos[currentToolIndex];

                    bool missTool = UnityEngine.Random.Range(0, 2) == 1;

                    if (!missTool && TryApplyToolReverted(toolInfo, pinValues))
                    {
                        tryCountToApplyTool = 0;
                    }
                    else
                    {
                        --tryCountToApplyTool;
                    }

                    ++currentToolIndex;
                    if (currentToolIndex >= randomOrderedToolInfos.Length)
                    {
                        currentToolIndex = 0;
                    }
                }
            }

            return pinValues;
        }

        private bool IsWinState(GameState gameState)
        {
            int pinInTargetStateCount = 0;
            foreach (int pinValue in gameState.PinValues)
            {
                if (pinValue == GameSettings.TargetPinValue)
                {
                    ++pinInTargetStateCount;
                }
            }

            return pinInTargetStateCount == gameState.PinValues.Length;
        }

        private void OnToolUsing(ToolInfo toolInfo)
        {
            int[] newPinValues = (int[])_gameState.PinValues.Clone();

            for (int i = 0; i < toolInfo.PinChangeValues.Length; i++)
            {
                newPinValues[i] = Mathf.Clamp(newPinValues[i] + toolInfo.PinChangeValues[i],
                    GameSettings.MinPinValue, GameSettings.MaxPinValue);
            }

            _gameState.SetPinValues(newPinValues);

            PinSetView.SetState(_gameState);
            ToolSetView.SetState(_gameState);

            if (IsWinState(_gameState))
            {
                StopGame();
                GameOverView.ShowWinPanel();
            }
        }

        private void OnRestartGameRequested()
        {
            RestartGame();
        }

        private void OnGameTimerUpdated()
        {
            int newSecondsToLoseLeft = Mathf.Max(0, GameSettings.SecondsToLose - (int)GameTimerView.CurrentGameTimeInSec);

            _gameState.SetSecondsToLoseLeft(newSecondsToLoseLeft);
            SecondsToLoseView.SetState(_gameState);

            if (_gameState.SecondsToLoseLeft <= 0)
            {
                StopGame();
                GameOverView.ShowLosePanel();
            }
        }
    }
}