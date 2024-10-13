using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RickAndMemory
{
    public class TimeInGameUIManager : NormalInGameUIManager
    {
        [SerializeField] private TMP_Text timeText;

        private Action onTimeFinished;
        private int time;
        private Coroutine timerRoutine;

        public void SetTime(int timeInSeconds, Action onTimeFinished) 
        {
            time = timeInSeconds;
            SetTimerText();

            if(timerRoutine == null)
               timerRoutine = StartCoroutine(TimerCoroutine());

            this.onTimeFinished = onTimeFinished;
        }

        public void StopTime() 
        {
            if (timerRoutine == null) return;

            StopCoroutine(timerRoutine);
        }

        private IEnumerator TimerCoroutine() 
        {
            while(time > 0) 
            {
                yield return new WaitForSeconds(1);
                time--;
                SetTimerText();
            }

            timerRoutine = null;
            onTimeFinished?.Invoke();
        }

        private void SetTimerText() 
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            timeText.text = t.ToString(@"mm\:ss");
        }
    }
}
