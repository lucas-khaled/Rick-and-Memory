using RickAndMemory.Core;
using RickAndMemory.Data;
using System;
using System.Collections.Generic;

namespace RickAndMemory.Modes
{
    public interface IModeManager
    {
        public Action OnUpdate { get; set; }
        public void StartGame(Layout layout, List<CardInfo> cards, int errors = 0, int score = 0);
        public void SetGameEndedCallback(Action<string> callback);
        public string GetModeName();
        public SaveInfo GetSaveInfo();
    }
}
