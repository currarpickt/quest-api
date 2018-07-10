using QuestApi.Interfaces;
using QuestApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuestApiTest
{
    /// <summary>
    /// Mock implementation for database
    /// </summary>
    public class FakePlayerRepository : IPlayerRepository
    {
        public void Update(Player player)
        {
        }

        public Player GetPlayer(string playerId)
        {
            if (playerId == "P01")
            {
                return new Player
                {
                    Id = "P01",
                    QuestId = "Q01",
                    MilestoneIndex = 1,
                    QuestPoint = 5
                };
            }
            else
            {
                return null;
            }
        }

        public void SaveChanges()
        {
        }
    }
}
