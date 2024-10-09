using RickAndMemory.Data;
using System.Threading.Tasks;

namespace RickAndMemory.Providers
{
    public interface ICardInfoProvider
    {
        public Task<CardInfo[]> GetCards(int amount);
    }
}
