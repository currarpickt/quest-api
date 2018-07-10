using QuestApi.Models;

namespace QuestApi.Interfaces
{
    public interface IPlayerRepository
    {
        void Update(Player player);
        Player GetPlayer(string playerId);
        void SaveChanges();
    }
}
