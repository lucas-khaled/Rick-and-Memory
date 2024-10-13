using RickAndMemory.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RickAndMemory.Providers
{
    public interface ICardInfoProvider
    {
        public Task<List<CardInfo>> GetCards(int amount);
    }
}
