using UnityEngine;

namespace BurglarGame
{

    public class PinSetView : MonoBehaviour
    {
        [SerializeField]
        private CanvasAnimatedPinView[] PinViews;

        //public void Init(IBurglarGameSettings gameSettings)
        //{
        //    foreach (var pinView in PinViews)
        //    {
        //        pinView.Init(gameSettings);
        //    }
        //}

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

        public void SetInitialState(GameState gameState)
        {
            if (gameState == null || gameState.PinValues == null
                || gameState.PinValues.Length != PinViews.Length)
            {
                Debug.LogError($"{nameof(PinSetView)}.{nameof(SetState)} gameState and pinViews don't match");
                return;
            }

            for (int i = 0; i < PinViews.Length; i++)
            {
                PinViews[i].SetInitialPinValue(gameState.PinValues[i]);
            }
        }
    }
}