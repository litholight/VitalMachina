using Mario.Common.Assets;
using Mario.Common.Models;

namespace Mario.Common.Factories
{
    public static class GameObjectFactory
    {
        private static Player _playerInstance;

        public static Player CreatePlayer(AssetManager assets)
        {
            if (_playerInstance != null)
            {
                throw new InvalidOperationException("Player instance already created.");
            }

            // Only create a new Player if one doesn't already exist
            _playerInstance = new Player
            {
                // Assuming "Player" is the key used when adding the player's sprite sheet in the AssetManager.
                SpriteSheet = assets.GetSpriteSheet("Player"),
                CurrentAnimation = "SmallFacingRight" // Set the initial animation to "FacingRight"
            };

            // Here you could set up additional properties or configurations for the player.
            // e.g., player.Speed = 5.0f;

            return _playerInstance;
        }

        // If you have additional game objects, like enemies, you can create methods for them too.
        public static Goomba CreateGoomba(AssetManager assets, float startX, float startY)
        {
            Goomba goomba = new Goomba
            {
                X = startX, // Set the starting X position
                Y = startY, // Set the starting Y position
                SpriteSheet = assets.GetSpriteSheet("Goomba"),
                CurrentAnimation = "GoombaA"
            };

            // If you have specific logic or properties for enemies, you can extend the GameObject class and create an Enemy class
            // In the future, you can add a spritesheet to the enemy here.

            return goomba;
        }

        // You can continue adding factory methods for different types of game objects here.
    }
}
