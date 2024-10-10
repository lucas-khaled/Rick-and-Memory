using RickAndMemory.Data;
using System;

namespace RickAndMemory.Modes
{
    public interface IModeManager
    {
        public void StartGame(Layout layout, CardInfo[] cards);
        public void SetGameEndedCallback(Action<string> callback);
        public string GetModeName();
    }
}
