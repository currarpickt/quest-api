using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QuestApi.Controllers;
using QuestApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace QuestApiTest
{
    public class ProgressControllerTest
    {
        private IOptions<QuestConfiguration> _config;
        private FakePlayerRepository _fakePlayerRepository = new FakePlayerRepository();

        [Fact]
        public void ProgressReturnPlayerProgressWhenBetIsValid()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());

            var controller = new ProgressController(_fakePlayerRepository, _config);

            var bet = new PlayerBet
            {
                PlayerId = "P01",
                PlayerLevel = 2,
                ChipAmountBet = 10
            };

            var player = _fakePlayerRepository.GetPlayer(bet.PlayerId);
            var expectedResult = CalculateProgress(_config.Value, player, bet);

            //Act
            var result = controller.Progress(bet);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PlayerProgress>(okObjectResult.Value);
            Assert.Equal(expectedResult.TotalQuestPercentCompleted, model.TotalQuestPercentCompleted);
            Assert.True(expectedResult.MilestonesCompleted.All(shouldItem => model.MilestonesCompleted.Any(isItem => isItem.MilestoneIndex == shouldItem.MilestoneIndex && isItem.ChipsAwarded == shouldItem.ChipsAwarded)));
            Assert.Equal(expectedResult.QuestPointsEarned, model.QuestPointsEarned);
        }

        [Fact]
        public void ProgressReturnNotFoundWhenPlayerNotFound()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());

            var controller = new ProgressController(_fakePlayerRepository, _config);

            var bet = new PlayerBet
            {
                PlayerId = "P03",
                PlayerLevel = 2,
                ChipAmountBet = 10
            };

            //Act
            var result = controller.Progress(bet);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundObjectResult.Value);
        }

        [Fact]
        public void ProgressReturnNotFoundWhenQuestConfigNotFound()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());
            _config.Value.Quest = null;

            var controller = new ProgressController(_fakePlayerRepository, _config);

            var bet = new PlayerBet
            {
                PlayerId = "P01",
                PlayerLevel = 2,
                ChipAmountBet = 10
            };

            //Act
            var result = controller.Progress(bet);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundObjectResult.Value);
        }

        [Fact]
        public void ProgressReturnBadRequestWhenBetIsNull()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());

            var controller = new ProgressController(_fakePlayerRepository, _config);

            //Act
            var result = controller.Progress(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        PlayerProgress CalculateProgress(QuestConfiguration configuration, Player player, PlayerBet bet)
        {
            var totalPointNeeded = configuration.Quest.QuestPointNeeded;
            var questPointEarned = bet.ChipAmountBet * configuration.RateFromBet + bet.PlayerLevel * configuration.LevelBonusRate;

            var questMilestones = configuration.Milestones.OrderBy(x => x.Index);
            var milestonesCompleted = new List<MilestoneCompleted>();

            var lastMilestoneIndex = 0;
            var totalQuestPoint = questPointEarned;
            var percentCompleted = questPointEarned / totalPointNeeded * 100;

            if (player.QuestId == configuration.Quest.Id)
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

            return result;
        }
    }
}
