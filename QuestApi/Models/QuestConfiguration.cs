using System.Collections.Generic;

namespace QuestApi.Models
{
    public class QuestConfiguration
    {
        public double RateFromBet { get; set; }
        public double LevelBonusRate { get; set; }
        public Quest Quest { get; set; }
        public List<Milestone> Milestones { get; set; }
    }
}
