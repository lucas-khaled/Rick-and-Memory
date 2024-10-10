using RickAndMemory.Attributes;
using RickAndMemory.Modes;
using RickAndMemory.Providers;
using RickAndMemory.UI;
using UnityEngine;

namespace RickAndMemory
{
    [CreateAssetMenu(menuName = "RickAndMemory/Game Setup", fileName = "GameSetup", order = 0)]
    public class GameSetup : ScriptableObject
    {
        [InterfaceSelection(typeof(ICardInfoProvider))]
        [SerializeReference]
        public ICardInfoProvider provider;

        [InterfaceSelection(typeof(IModeManager))]
        [SerializeReference]
        public IModeManager[] avaiableModes;
    }
}
