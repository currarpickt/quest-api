using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestApi.Interfaces;
using QuestApi.Models;

namespace QuestApi.Controllers
{
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly QuestConfiguration _configuration;

        public ProgressController(IPlayerRepository playerRepository, IOptions<QuestConfiguration> configuration)
        {
            _playerRepository = playerRepository;
            _configuration = configuration.Value;
        }

        // POST api/progress
        [HttpPost]
        public IActionResult Progress([FromBody]PlayerBet bet)
        {
            if(bet == null)
            {
                return BadRequest();
            }

            var player = _playerRepository.GetPlayer(bet.PlayerId);
            if (player != null && _configuration.Quest != null)
            {
                var totalPointNeeded = _configuration.Quest.QuestPointNeeded;
                var questPointEarned = bet.ChipAmountBet * _configuration.RateFromBet + bet.PlayerLevel * _configuration.LevelBonusRate;

                var questMilestones = _configuration.Milestones.OrderBy(x => x.Index);
                var milestonesCompleted = new List<MilestoneCompleted>();
                
                var lastMilestoneIndex = 0;
                var totalQuestPoint = questPointEarned;
                var percentCompleted = questPointEarned / totalPointNeeded * 100;

                //Only take player data into account if player's current quest is active
                if (player.QuestId == _configuration.Quest.Id)
                {
                    totalQuestPoint = questPointEarned + player.QuestPoint;
                    lastMilestoneIndex = player.MilestoneIndex;
                    percentCompleted = (questPointEarned + player.QuestPoint) / totalPointNeeded * 100;
                }

                var projectedMilestoneIndex = questMilestones.Last(x => x.TotalQuestPoint <= totalQuestPoint).Index;

                foreach (Milestone milestone in questMilestones)
                {
                    if (milestone.Index > lastMilestoneIndex && milestone.Index <= projectedMilestoneIndex)
                    {
                        milestonesCompleted.Add(new MilestoneCompleted
                        {
                            MilestoneIndex = milestone.Index,
                            ChipsAwarded = milestone.ChipsAwarded
                        });
                    }
                }

                var result = new PlayerProgress
                {
                    QuestPointsEarned = questPointEarned,
                    TotalQuestPercentCompleted = percentCompleted > 100 ? 100 : percentCompleted,
                    MilestonesCompleted = milestonesCompleted
                };

                player.QuestPoint = totalQuestPoint;
                player.QuestId = _configuration.Quest.Id;
                player.MilestoneIndex = projectedMilestoneIndex;
                _playerRepository.Update(player);
                _playerRepository.SaveChanges();

                return Ok(result);
            }
            else
            {
                return NotFound(null);
            }
        }
    }
}
