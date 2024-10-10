using RickAndMemory.Data;
using RickAndMemory.Modes;
using System;

namespace RickAndMemory
{
    public class NormalModeManager : IModeManager
    {
        public string GetModeName()
        {
            return "Normal Mode";
        }

        public void SetGameEndedCallback(Action<string> callback)
        {
            
        }

        public void StartGame(Layout layout, CardInfo[] cards)
        {
            
        }
    }
}
