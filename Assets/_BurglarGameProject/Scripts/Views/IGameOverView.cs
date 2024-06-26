using System;

namespace _BurglarGameProject.Scripts.Views
{
    public interface IGameOverView
    {
        public event Action RestartButtonClicked;
        public void ShowWinScreen();
        public void ShowLoseScreen();
        public void Hide();
    }
}