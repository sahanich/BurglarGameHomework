using UnityEngine;

namespace BurglarGame
{
    public class PinSetView : MonoBehaviour
    {
        [SerializeField]
        private PinView[] PinViews;

        private int _minPinValue;
        private int _maxPinValue;

        public void Init(BurglarGameSettings gameSettings)
        {
            _minPinValue = gameSettings.MinPinValue;
            _maxPinValue = gameSettings.MaxPinValue;

            foreach (var pinView in PinViews)
            {
                pinView.Init(_minPinValue, _maxPinValue);
            }
        }

        public void SetState(GameState gameState)
        {
            if (gameState == null || gameState.PinValues == null
                || gameState.PinValues.Length != PinViews.Length)
            {
                Debug.LogError($"{nameof(PinSetView)}.{nameof(SetState)} gameState and pinViews don't match");
                return;
            }

            for (int i = 0; i < PinViews.Length; i++)
            {
                PinViews[i].SetPinValue(gameState.PinValues[i]);
            }
        }
    }
}