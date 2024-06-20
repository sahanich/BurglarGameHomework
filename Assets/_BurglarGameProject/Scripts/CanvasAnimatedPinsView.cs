using UnityEngine;

namespace BurglarGame
{
    public interface IPinsView
    {
        public void Init(int minPinValue, int maxPinValue);
        public void SetPinValues(int[] pinValues);
    }

    public class CanvasAnimatedPinsView : MonoBehaviour, IPinsView
    {
        [SerializeField]
        private CanvasAnimatedPinView[] PinViews;
        [SerializeField]
        private float PinValueChangeAnimationDuration;

        public void Init(int minPinValue, int maxPinValue)
        {
            foreach (var pinView in PinViews)
            {
                pinView.Init(minPinValue, maxPinValue,
                    PinValueChangeAnimationDuration);
            }
        }

        //[Inject]
        //public void Construct(IBurglarGameSettings gameSettings)
        //{
        //    foreach (var pinView in PinViews)
        //    {
        //        pinView.Init(gameSettings.MinPinValue, gameSettings.MaxPinValue, 
        //            PinValueChangeAnimationDuration);
        //    }
        //}

        public void SetPinValues(int[] pinValues)
        {
            if (pinValues == null || pinValues.Length != PinViews.Length)
            {
                Debug.LogError($"Wrong pin values");
                return;
            }

            for (int i = 0; i < pinValues.Length; i++)
            {
                PinViews[i].SetPinValue(pinValues[i]);
            }
        }
    }
}