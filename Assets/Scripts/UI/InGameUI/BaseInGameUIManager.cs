using UnityEngine;

namespace RickAndMemory.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class BaseInGameUIManager : MonoBehaviour
    {
        public abstract void SetScore(float score);
        public abstract void SetErrors(float errors);

    }
}
