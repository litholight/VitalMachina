using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Services;
using Microsoft.JSInterop;

namespace Mario.WebImplementations
{
    public class WebGameControls : IGameControls
    {
        private readonly GameService _gameService;
        private readonly IJSRuntime _jsRuntime;

        public WebGameControls(GameService gameService, IJSRuntime jsRuntime)
        {
            _gameService = gameService;
            _jsRuntime = jsRuntime;
        }

        public Task MoveUp()
        {
            _gameService.MoveUp();
            return Task.CompletedTask;
        }

        public Task MoveDown()
        {
            _gameService.MoveDown();
            return Task.CompletedTask;
        }

        public Task MoveLeft()
        {
            _gameService.MoveLeft();
            return Task.CompletedTask;
        }

        public Task MoveRight()
        {
            _gameService.MoveRight();
            return Task.CompletedTask;
        }
    }
}
