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
            GameOverView.Init(_eventsHandler);
            GameTimerView.Init(_eventsHandler);
            SecondsToLoseView.Init(GameSettings);
        }

        private void OnEnable()
        {
            _eventsHandler.ToolUseConfirmed += OnToolUsing;
            _eventsHandler.RestartGameRequested += OnRestartGameRequested;
            _eventsHandler.GameTimerUpdated += OnGameTimerUpdated;
        }

        private void OnGameTimerUpdated()
        {
            int newSecondsToLoseLeft = Mathf.Max(0, GameSettings.SecondsToLose - (int)GameTimerView.CurrentGameTimeInSec);
            
            _gameState.SetSecondsToLoseLeft(newSecondsToLoseLeft);
            SecondsToLoseView.SetState(_gameState);

            if (_gameState.SecondsToLoseLeft <= 0)
            {
                GameTimerView.StopGameTimer();
                GameOverView.ShowLosePanel();
            }
        }

        private void OnDisable()
        {
            _eventsHandler.ToolUseConfirmed -= OnToolUsing;
            _eventsHandler.RestartGameRequested -= OnRestartGameRequested;
        }

        private void Start()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            GameOverView.Hide();

            _gameState = CreateGameState();

            PinSetView.SetState(_gameState);
            ToolSetView.SetState(_gameState);
            SecondsToLoseView.SetState(_gameState);

            GameTimerView.StartGameTimer();
        }

        private GameState CreateGameState()
        {

            //ToolInfo[] tools = CreateTools(GameSettings.PinCount, GameSettings.Tools);
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

        //private int[] CreatePinStates(int pinCount, ToolInfo[] toolInfos)
        //{
        //    int[] pinStates = new int[pinCount];

        //    for (int i = 0; i < pinStates.Length; i++)
        //    {
        //        pinStates[i] = GameSettings.TargetPinValue;
        //    }

        //    int toolToApplyIndex = UnityEngine.Random.Range(0, toolInfos.Length);

        //    for (int i = 0; i < pinStates.Length; i++)
        //    {
        //        pinStates[i] -= toolInfos[toolToApplyIndex].PinChangeValues[i];
        //    }

        //    return pinStates;
        //}

        //private ToolInfo[] CreateTools(int pinCount, params ToolType[] toolTypes)
        //{
        //    ToolInfo[] tools = new ToolInfo[toolTypes.Length];

        //    for (int i = 0; i < toolTypes.Length; i++)
        //    {
        //        int[] pinChangeValues = new int[pinCount];
        //        for (int j = 0; j < pinChangeValues.Length; j++)
        //        {
        //            pinChangeValues[j] = UnityEngine.Random.Range(GameSettings.MinToolPinChangeValue,
        //                GameSettings.MaxToolPinChangeValue + 1);
        //        }

        //        int zeroCount = 0;
        //        for (int j = 0; j < pinChangeValues.Length; j++)
        //        {
        //            if (pinChangeValues[j] == 0)
        //            {
        //                ++zeroCount;
        //            }
        //        }
        //        if (zeroCount == pinChangeValues.Length && pinChangeValues.Length > 0)
        //        {
        //            pinChangeValues[0] = 1;
        //        }

        //        tools[i] = new(toolTypes[i], pinChangeValues);
        //    }

        //    return tools;
        //}

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
                //_pinValues[i] += UnityEngine.Mathf.Clamp(toolInfo.PinChangeValues[i];
            }
            //_gameState.ApplyToolValues(toolInfo);
            _gameState.SetPinValues(newPinValues);

            PinSetView.SetState(_gameState);
            ToolSetView.SetState(_gameState);

            if (IsWinState(_gameState))
            {
                GameTimerView.StopGameTimer();
                GameOverView.ShowWinPanel();
            }
        }

        private void OnRestartGameRequested()
        {
            ResetGame();
        }
    }
}