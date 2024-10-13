using RickAndMemory.Core;
using RickAndMemory.Data;
using System;
using System.Collections.Generic;

namespace RickAndMemory.Modes
{
    public interface IModeManager
    {
        public Action OnUpdate { get; set; }
        public void StartGame(Layout layout, List<CardInfo> cards, ModeInfo uiInfo);
        public void SetGameEndedCallback(Action<string> callback);
        public string GetModeName();
        public SaveInfo GetSaveInfo();
    }
}
