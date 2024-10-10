using UnityEngine;

namespace RickAndMemory
{
    public class LoadingPanel : MonoBehaviour, IPanel
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
