using UnityEngine;

namespace RickAndMemory.UI
{
    public interface IInGameUIManager
    {
        public void SetScore(float score);
        public void SetErrors(float errors);
    }
}
