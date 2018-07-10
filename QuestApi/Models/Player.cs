namespace QuestApi.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string QuestId { get; set; }
        public int MilestoneIndex { get; set; }
        public double QuestPoint { get; set; }
    }
}
