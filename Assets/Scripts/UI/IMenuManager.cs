using RickAndMemory.Data;
using RickAndMemory.Modes;
using System;

namespace RickAndMemory.UI
{
    public interface IMenuManager
    {
        public void SetLoading(bool active);
        public void Initialize(Action<Layout, IModeManager> callback);
        public void SetEndGame(string text);
        public void HideAll();
    }
}
