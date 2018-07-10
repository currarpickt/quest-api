using QuestApi.Models;
using System.Linq;

namespace QuestApi.Data
{
    public static class DBInitializer
    {
        public static void Initialize(PlayerContext context)
        {
            context.Database.EnsureCreated();

            if(context.Players.Any())
            {
                return;
            }

            var players = new Player[]
            {
            new Player{Id="P01", QuestId="Q01", MilestoneIndex=0, QuestPoint=0},
            new Player{Id="P02", QuestId="Q01", MilestoneIndex=0, QuestPoint=0},
            new Player{Id="P03", QuestId="Q01", MilestoneIndex=0, QuestPoint=0},
            new Player{Id="P04", QuestId="Q01", MilestoneIndex=0, QuestPoint=0},
            new Player{Id="P05", QuestId="Q01", MilestoneIndex=0, QuestPoint=0}
            };
            foreach (Player p in players)
            {
                context.Players.Add(p);
            }
            context.SaveChanges();
        }
    }
}
