using RickAndMemory.Data;
using RickAndMemory.Modes;
using System;

namespace RickAndMemory.UI
{
    public interface IMainUIManager
    {
        public void SetLoading(bool active);
        public void Initialize(Action<Layout, IModeManager> callback);
        public void SetEndGame(string text);
    }
}
