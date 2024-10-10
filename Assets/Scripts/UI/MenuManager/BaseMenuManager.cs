using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory
{
    public abstract class BaseMenuManager : MonoBehaviour
    {
        public abstract void Initialize(Action<Layout, IModeManager> callback);

        public abstract void HideAll();

        public abstract void SetEndGame(string text);

        public abstract void SetLoading(bool active);
    }
}
