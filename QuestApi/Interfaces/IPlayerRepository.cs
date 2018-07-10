using QuestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestApi.Interfaces
{
    public interface IPlayerRepository
    {
        void Update(Player player);
        Player GetPlayer(string playerId);
        void SaveChanges();
    }
}
