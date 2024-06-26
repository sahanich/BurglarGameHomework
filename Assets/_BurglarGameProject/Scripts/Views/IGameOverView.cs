using System;

namespace Assets._BurglarGameProject.Scripts.Gameplay
{
    public interface IGameOverView
    {
        public event Action RestartButtonClicked;
        public void ShowWinScreen();
        public void ShowLoseScreen();
        public void Hide();
    }
}