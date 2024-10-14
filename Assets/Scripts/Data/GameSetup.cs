using RickAndMemory.Attributes;
using RickAndMemory.Data;
using RickAndMemory.Modes;
using RickAndMemory.Providers;
using UnityEngine;

namespace RickAndMemory.Data
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

        public Style style;
    }
}
