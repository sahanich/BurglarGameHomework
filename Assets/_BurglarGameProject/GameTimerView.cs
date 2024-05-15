using System.Collections;
using UnityEngine;

namespace BurglarGame
{
    public class GameTimerView : MonoBehaviour
    {
        [SerializeField]
        private float TimerUpdateFrequency = 1;

        public float CurrentGameTimeInSec { get; private set; }

        private BurglarGameEventsHandler _eventsHandler;

        public void Init(BurglarGameEventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }

        public void StartGameTimer()
        {
            StopAllCoroutines();

            CurrentGameTimeInSec = 0;
            _eventsHandler.RaiseTimerUpdated();

            StartCoroutine(TimerRoutine());
        }

        public void StopGameTimer()
        {
            StopAllCoroutines();
        }

        private IEnumerator TimerRoutine()
        {
            WaitForSeconds timerUpdateYieldInstruction = new(TimerUpdateFrequency); 

            while (true)
            {
                yield return timerUpdateYieldInstruction;
                CurrentGameTimeInSec += TimerUpdateFrequency;
                _eventsHandler.RaiseTimerUpdated();
            }
        }
    }
}