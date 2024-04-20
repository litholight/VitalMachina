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
        public static GameObject CreateEnemy(AssetManager assets, float startX, float startY)
        {
            // Create a new GameObject instance for the enemy
            GameObject enemy = new GameObject
            {
                Type = GameObjectType.Enemy,
                X = startX, // Set the starting X position
                Y = startY, // Set the starting Y position
                Width = .76F, // Set the width of the enemy
                Height = .76F, // Set the height of the enemy
                Color = Color.Orange, // Give the enemy a brown color for now
                SpriteSheet = assets.GetSpriteSheet("Enemy"),
                CurrentAnimation = "EnemyA"
            };

            // If you have specific logic or properties for enemies, you can extend the GameObject class and create an Enemy class
            // In the future, you can add a spritesheet to the enemy here.

            return enemy;
        }

        // You can continue adding factory methods for different types of game objects here.
    }
}
