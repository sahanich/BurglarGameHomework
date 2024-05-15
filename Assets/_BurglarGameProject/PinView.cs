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

        public void Init(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public void SetPinValue(int value)
        {
            _currentValue = value;
            PinValueText.text = value.ToString();

            VisualizePinState();
        }

        public void VisualizePinState()
        {
            Vector3 maxShiftVect = MaxMovingPanelPoint.position - MinMovingPanelPoint.position;

            Vector3 shiftVect = maxShiftVect * _currentValue / (_maxValue - _minValue);

            Vector3 targetPoint = MinMovingPanelPoint.position + shiftVect;

            PinMovingPanel.position = targetPoint;
        }
    }
}