using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RickAndMemory
{
    public class TimeInGameUIManager : NormalInGameUIManager
    {
        [SerializeField] private TMP_Text timeText;

        public int Time { get; private set; }
        
        private Action onTimeFinished;
        private Action onTimePassed;
        private Coroutine timerRoutine;

        public void SetTime(int timeInSeconds, Action onTimeFinished, Action onTimePassed) 
        {
            Time = timeInSeconds;
            SetTimerText();

            this.onTimeFinished = onTimeFinished;
            this.onTimePassed = onTimePassed;

            if (timerRoutine == null)
               timerRoutine = StartCoroutine(TimerCoroutine());

        }

        public void StopTime() 
        {
            if (timerRoutine == null) return;

            StopCoroutine(timerRoutine);
        }

        private IEnumerator TimerCoroutine() 
        {
            while(Time > 0) 
            {
                yield return new WaitForSeconds(1);
                Time--;
                onTimePassed?.Invoke();
                SetTimerText();
            }

            timerRoutine = null;
            onTimeFinished?.Invoke();
        }

        private void SetTimerText() 
        {
            TimeSpan t = TimeSpan.FromSeconds(Time);
            timeText.text = t.ToString(@"mm\:ss");
        }
    }
}
