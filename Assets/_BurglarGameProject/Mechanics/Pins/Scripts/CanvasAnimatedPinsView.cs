using UnityEngine;

namespace _BurglarGameProject.Mechanics.Pins.Scripts
{
    public class CanvasAnimatedPinsView : MonoBehaviour, IPinsView
    {
        [SerializeField]
        private CanvasAnimatedPinView[] PinViews;

        public void Init(int minPinValue, int maxPinValue, float pinValueChangeAnimationDuration)
        {
            foreach (var pinView in PinViews)
            {
                pinView.Init(minPinValue, maxPinValue,
                    pinValueChangeAnimationDuration);
            }
        }

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