﻿using QuestApi.Interfaces;
using QuestApi.Models;

namespace QuestApi.Services
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly PlayerContext _context;

        public PlayerRepository(PlayerContext context)
        {
            _context = context;
        }

        public Player GetPlayer(string playerId)
        {
            return _context.Players.Find(playerId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Player player)
        {
            _context.Update(player);
        }
    }
}
