using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestApi.Models
{
    public class PlayerProgress
    {
        public double QuestPointsEarned { get; set; }
        public double TotalQuestPercentCompleted { get; set; }
        public List<MilestoneCompleted> MilestonesCompleted { get; set; }
    }

    public class MilestoneCompleted
    {
        public int MilestoneIndex { get; set; }
        public double ChipsAwarded { get; set; }
    }
}
