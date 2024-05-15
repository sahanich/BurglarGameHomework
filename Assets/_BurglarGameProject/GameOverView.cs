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
        private TMP_Text GameOverText;
        [SerializeField]
        private Button RestartButton;

        private BurglarGameEventsHandler _eventsHandler;

        private void OnEnable()
        {
            RestartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDisable()
        {
            RestartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        public void Init(BurglarGameEventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }

        public void ShowWinPanel()
        {
            GameOverText.text = "Вы выиграли";
            GameOverPanel.SetActive(true);
        }

        public void ShowLosePanel()
        {
            GameOverText.text = "Вы проиграли";
            GameOverPanel.SetActive(true);
        }

        public void Hide()
        {
            GameOverPanel.SetActive(false);
        }

        private void OnRestartButtonClick()
        {
            _eventsHandler?.RaiseRestartGameRequested();
        }
    }
}