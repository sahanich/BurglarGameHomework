using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BurglarGame
{
    public class SecondsToLoseView : MonoBehaviour
    {
        [SerializeField]
        private Image SecondsToLoseSliderImage;
        [SerializeField]
        private TMP_Text SecondsToLoseText;

        private int _initialSecondsToLose;
        private int _currentSecondsToLose;

        private Coroutine _refreshFillAmountRoutine;

        public void Init(BurglarGameSettings gameSettings)
        {
            _initialSecondsToLose = gameSettings.SecondsToLose;
        }

        public void SetState(GameState gameState)
        {
            _currentSecondsToLose = gameState.SecondsToLoseLeft;
            RefreshTimerViewSmoothly();
        }

        private void RefreshTimerViewSmoothly()
        {
            SecondsToLoseText.text = _currentSecondsToLose.ToString();

            if (_refreshFillAmountRoutine != null)
            {
                StopCoroutine(_refreshFillAmountRoutine);
            }
            _refreshFillAmountRoutine = StartCoroutine(RefreshFillAmountSmoothlyRoutine());
        }

        private IEnumerator RefreshFillAmountSmoothlyRoutine()
        {
            if (_currentSecondsToLose == _initialSecondsToLose)
            {
                SecondsToLoseSliderImage.fillAmount = 1;
            }

            float targetFillAmount = Mathf.Max(0, (float)(_currentSecondsToLose - 1) / _initialSecondsToLose);
            float difference = targetFillAmount - SecondsToLoseSliderImage.fillAmount;
            float accumulatedDifference = 0;
            while (Mathf.Abs(difference) - Mathf.Abs(accumulatedDifference) > 0)
            {
                yield return null;

                float differenceDelta = difference * Time.deltaTime;
                SecondsToLoseSliderImage.fillAmount += differenceDelta;
                accumulatedDifference += differenceDelta;
            }
            SecondsToLoseSliderImage.fillAmount = targetFillAmount;

            _refreshFillAmountRoutine = null;
        }
    }
}