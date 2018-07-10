using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestApi.Models
{
    public class PlayerBet
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public double ChipAmountBet { get; set; }
    }
}
