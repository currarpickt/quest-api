using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QuestApi.Controllers;
using QuestApi.Models;
using System.IO;
using Xunit;

namespace QuestApiTest
{
    public class StateControllerTest
    {
        private IOptions<QuestConfiguration> _config;
        private FakePlayerRepository _fakePlayerRepository = new FakePlayerRepository();

        [Fact]
        public void GetStateReturnPlayerStateWhenPlayerIdIsValid()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());

            var controller = new StateController(_fakePlayerRepository, _config);
            var playerId = "P01";
            var totalQuestPercentCompleted = (double)5 / 31 * 100;

            //Act
            var result = controller.GetState(playerId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PlayerState>(okObjectResult.Value);
            Assert.Equal(1, model.LastMilestoneIndexCompleted);
            Assert.Equal(totalQuestPercentCompleted, model.TotalQuestPercentCompleted);
        }

        [Fact]
        public void GetStateReturnNotFoundWhenPlayerNotFound()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());

            var controller = new StateController(_fakePlayerRepository, _config);
            var playerId = "P03";

            //Act
            var result = controller.GetState(playerId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundObjectResult.Value);
        }

        [Fact]
        public void GetStateReturnNotFoundWhenQuestConfigNotFound()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

            _config = Options.Create(configuration.Get<QuestConfiguration>());
            _config.Value.Quest = null;

            var controller = new StateController(_fakePlayerRepository, _config);
            var playerId = "P03";

            //Act
            var result = controller.GetState(playerId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundObjectResult.Value);
        }
    }
}
