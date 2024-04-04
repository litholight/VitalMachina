using System;
using Mario.Common.Models;

namespace Mario.Common.Services
{
    public class GameService
    {
        // Assuming you're managing a player object for now
        public GameObject Player { get; private set; } = new GameObject();

        public event Action OnStateChange = delegate { };

        public void MoveUp()
        {
            Player.Y -= 10;
            NotifyStateChanged();
        }

        public void MoveDown()
        {
            Player.Y += 10;
            NotifyStateChanged();
        }

        public void MoveLeft()
        {
            Player.X -= 10;
            NotifyStateChanged();
        }

        public void MoveRight()
        {
            Player.X += 10;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
