﻿namespace QuestApi.Models
{
    public class Quest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double QuestPointNeeded { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
    }
}
