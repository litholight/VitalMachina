using System.Threading.Tasks;

namespace Mario.Common.Abstractions
{
    public interface IGameControls
    {
        Task MoveUp();
        Task MoveDown();
        Task MoveLeft();
        Task MoveRight();
    }
}
