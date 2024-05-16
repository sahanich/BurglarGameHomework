using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BurglarGame
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField]
        private GameObject GameOverPanel;
        [SerializeField]
        private CanvasGroup GameOverPanelCanvasGroup;
        [SerializeField]
        private TMP_Text GameOverText;
        [SerializeField]
        private AudioSource WinAudioSource;
        [SerializeField]
        private AudioSource LoseAudioSource;
        [SerializeField]
        private Button RestartButton;

        private BurglarGameSettings _gameSettings;
        private BurglarGameEventsHandler _eventsHandler;

        private void OnEnable()
        {
            RestartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDisable()
        {
            RestartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        public void Init(BurglarGameSettings gameSettings, BurglarGameEventsHandler eventsHandler)
        {
            _gameSettings = gameSettings;
            _eventsHandler = eventsHandler;
        }

        public void ShowWinPanel()
        {
            StopAllCoroutines();
            StartCoroutine(ShowGameOverRoutine(0.5f, "Вы выиграли", WinAudioSource, 
                waitForSoundEnd: true));
        }

        public void ShowLosePanel()
        {
            StopAllCoroutines();
            StartCoroutine(ShowGameOverRoutine(0.1f, "Вы проиграли", LoseAudioSource, 
                waitForSoundEnd: false));
        }

        public void Hide()
        {
            StopAllCoroutines();
            StopSound();

            GameOverPanel.SetActive(false);
            GameOverPanelCanvasGroup.alpha = 0;
        }

        private void StopSound()
        {
            if (WinAudioSource.isPlaying)
            {
                WinAudioSource.Stop();
            }
            if (LoseAudioSource.isPlaying)
            {
                LoseAudioSource.Stop();
            }
        }

        private IEnumerator ShowGameOverRoutine(float delay, string gameOverText, 
            AudioSource gameOverAudioSource, bool waitForSoundEnd)
        {
            StopSound();

            GameOverText.text = gameOverText;

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            if (gameOverAudioSource.isPlaying)
            {
                gameOverAudioSource.Stop();
            }
            gameOverAudioSource.Play();

            if (waitForSoundEnd)
            {
                yield return new WaitForSeconds(gameOverAudioSource.clip.length);
            }

            StartCoroutine(ShowPanelSmoothly());
        }

        private IEnumerator ShowPanelSmoothly()
        {
            float duration = _gameSettings.GameOverPanelAnimationDuration;

            GameOverPanelCanvasGroup.alpha = 0;
            GameOverPanel.SetActive(true);

            float endTime = Time.time + duration;

            while (Time.time < endTime)
            {
                GameOverPanelCanvasGroup.alpha += Time.deltaTime / duration;
                yield return null;
            }

            GameOverPanelCanvasGroup.alpha = 1;
        }

        private void OnRestartButtonClick()
        {
            _eventsHandler?.RaiseRestartGameRequested();
        }
    }
}