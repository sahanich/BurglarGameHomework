using System.Collections;
using TMPro;
using UnityEngine;

namespace BurglarGame
{
    public class PinView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text PinValueText;
        [SerializeField]
        private RectTransform PinMovingPanel;
        [SerializeField]
        private RectTransform MinMovingPanelPoint;
        [SerializeField]
        private RectTransform MaxMovingPanelPoint;

        private int _minValue;
        private int _maxValue;
        private int _currentValue;

        private float _pinValueChangeAnimationDuration;
        private Coroutine _setPinValueSmoothlyRoutine;

        public void Init(BurglarGameSettings gameSettings)
        {
            _minValue = gameSettings.MinPinValue;
            _maxValue = gameSettings.MaxPinValue;
            _pinValueChangeAnimationDuration = gameSettings.PinValueChangeAnimationDuration;
        }

        public void SetInitialPinValue(int newPinValue)
        {
            _currentValue = newPinValue;
            PinValueText.text = newPinValue.ToString();

            VisualizePinState();
        }

        public void SetPinValue(int newPinValue)
        {
            SetPinStateSmoothly(newPinValue);
        }

        private void VisualizePinState()
        {
            Vector3 maxShiftVect = MaxMovingPanelPoint.position - MinMovingPanelPoint.position;

            Vector3 shiftVect = maxShiftVect * _currentValue / (_maxValue - _minValue);

            Vector3 targetPoint = MinMovingPanelPoint.position + shiftVect;

            PinMovingPanel.position = targetPoint;
        }

        private void SetPinStateSmoothly(int newPinValue)
        {
            if (_setPinValueSmoothlyRoutine != null)
            {
                StopCoroutine(_setPinValueSmoothlyRoutine);
            }
            _setPinValueSmoothlyRoutine = StartCoroutine(SetPinValueSmoothlyRoutine(newPinValue));
        }

        private IEnumerator SetPinValueSmoothlyRoutine(int newPinValue)
        {
            Vector3 maxShiftVect = MaxMovingPanelPoint.position - MinMovingPanelPoint.position;

            Vector3 shiftVectFromMinPoint = maxShiftVect * newPinValue / (_maxValue - _minValue);
            Vector3 targetPoint = MinMovingPanelPoint.position + shiftVectFromMinPoint;

            Vector3 shiftVect = targetPoint - PinMovingPanel.position;
            Vector3 startPoint = PinMovingPanel.position;

            float shiftDistance = shiftVect.magnitude;

            float shiftTime = _pinValueChangeAnimationDuration;
            int startValue = _currentValue;

            float accumulatedDistance = 0;
            while (accumulatedDistance < shiftDistance)
            {
                yield return null;
                
                float distanceDelta = shiftDistance * Time.deltaTime / shiftTime;
                accumulatedDistance += distanceDelta;
                float factor = accumulatedDistance / shiftDistance;

                _currentValue = (int)(Mathf.Lerp(startValue, newPinValue, factor) + 0.5f);
                PinMovingPanel.position = Vector3.Lerp(startPoint, targetPoint, factor);
                PinValueText.text = _currentValue.ToString();
            }

            _currentValue = newPinValue;
            PinMovingPanel.position = targetPoint;
            PinValueText.text = _currentValue.ToString();

            _setPinValueSmoothlyRoutine = null;
        }
    }
}