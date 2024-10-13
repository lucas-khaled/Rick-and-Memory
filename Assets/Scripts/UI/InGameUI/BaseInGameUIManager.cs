using UnityEngine;

namespace RickAndMemory.UI
{
    public abstract class BaseInGameUIManager : MonoBehaviour
    {
        public abstract void SetScore(float score);
        public abstract void SetErrors(float errors);
    }
}
