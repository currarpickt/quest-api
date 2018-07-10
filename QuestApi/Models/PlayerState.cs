using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestApi.Models
{
    public class PlayerState
    {
        public double TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
    }
}
