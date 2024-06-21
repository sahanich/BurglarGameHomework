using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public class SecondsToLoseView : MonoBehaviour, IGameTimeView
    {
        [SerializeField]
        private Image SecondsToLoseSliderImage;
        [SerializeField]
        private TMP_Text SecondsToLoseText;

        private int _initialSecondsToLose;
        private int _currentSecondsToLose;

        private Coroutine _refreshFillAmountRoutine;

        public void Init(int initialSecondsToLose)
        {
            _initialSecondsToLose = initialSecondsToLose;
        }

        public void SetTime(float timeFromGameStart)
        {
            if (timeFromGameStart > _initialSecondsToLose)
            {
                return;
            }
            _currentSecondsToLose = _initialSecondsToLose - (int)timeFromGameStart;
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

                float differenceDelta = difference * UnityEngine.Time.deltaTime;
                SecondsToLoseSliderImage.fillAmount += differenceDelta;
                accumulatedDifference += differenceDelta;
            }
            SecondsToLoseSliderImage.fillAmount = targetFillAmount;

            _refreshFillAmountRoutine = null;
        }
    }
}