using RickAndMemory.Audio;
using RickAndMemory.Core;
using RickAndMemory.Data;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public class TimeModeManager : BaseModeManager
    {
        public int timeInSecondsPerCard = 10;
        public CardsManager cardsManagerPrefab;
        public TimeInGameUIManager UIManagerPrefab;
        public AudioClip timeOverClip;

        private TimeInGameUIManager timeUiManager;

        private const string FINISHED_TIME_STRING = "The time is over! You were not able to complete it :(";

        public override string GetModeName()
        {
            return "Time Mode";
        }

        protected override CardsManager GetCardsManagerPrefab()
        {
            return cardsManagerPrefab;
        }

        protected override BaseInGameUIManager GetUIManagerPrefab()
        {
            return UIManagerPrefab;
        }

        protected override void OnCardsFinished()
        {
            timeUiManager.StopTime();
            base.OnCardsFinished();
        }

        public override void StartGame(Layout layout, List<CardInfo> cards, ModeInfo info)
        {
            base.StartGame(layout, cards, info);

            timeUiManager = (TimeInGameUIManager)UIManager;

            var time = (info is TimeModeInfo timeInfo && timeInfo.time > 0) ? timeInfo.time : CalculateTime();
            timeUiManager.SetTime(time, OnFinishTime, OnUpdate);
        }

        private void OnFinishTime()
        {
            AudioManager.Instance.PlayClip(timeOverClip);
            EndGame(FINISHED_TIME_STRING);
        }

        private int CalculateTime() 
        {
            return layout.TotalAmount * timeInSecondsPerCard;
        }

        protected override ModeInfo GetModeInfo()
        {
            return new TimeModeInfo()
            {
                score = score,
                time = (timeUiManager != null) ? timeUiManager.Time : 0,
                errors = errors,
                streak = streak
            };
        }

        [Serializable]
        public class TimeModeInfo : ModeInfo 
        {
            public int time;
        }
    }
}
