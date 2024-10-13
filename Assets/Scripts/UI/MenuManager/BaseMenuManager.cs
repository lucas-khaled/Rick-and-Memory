using RickAndMemory.Data;
using RickAndMemory.Modes;
using System;
using UnityEngine;

namespace RickAndMemory
{
    public abstract class BaseMenuManager : MonoBehaviour
    {
        public abstract void Initialize(Action<Layout, IModeManager> callback, IModeManager[] avaliableModes);

        public abstract void HideAll();

        public abstract void SetEndGame(string text);

        public abstract void SetLoading(bool active);
        public abstract void ShowInitialScreen();
    }
}
