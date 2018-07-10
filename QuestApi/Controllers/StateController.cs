using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuestApi.Interfaces;
using QuestApi.Models;

namespace QuestApi.Controllers
{
    [Route("api/state")]
    public class StateController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly QuestConfiguration _configuration;

        public StateController(IPlayerRepository playerRepository, IOptions<QuestConfiguration> configuration)
        {
            _playerRepository = playerRepository;
            _configuration = configuration.Value;
        }

        // GET api/state/P01
        [HttpGet("{playerId}", Name = "GetState")]
        public IActionResult GetState(string playerId)
        {
            var item = _playerRepository.GetPlayer(playerId);
            if (item == null)
            {
                return NotFound(null);
            }
            
            if (_configuration.Quest != null)
            {
                var percentage = item.QuestPoint / _configuration.Quest.QuestPointNeeded * 100;
                var result = new PlayerState
                {
                    TotalQuestPercentCompleted = _configuration.Quest.Id == item.QuestId ? percentage : 0,
                    LastMilestoneIndexCompleted = _configuration.Quest.Id == item.QuestId ? item.MilestoneIndex : 0
                };
                return Ok(result);
            }

            return NotFound(null);
        }
    }
}
