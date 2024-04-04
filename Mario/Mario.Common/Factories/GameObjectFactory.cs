using Mario.Common.Assets;
using Mario.Common.Models;

namespace Mario.Common.Factories
{
    public static class GameObjectFactory
    {
        public static Player CreatePlayer(AssetManager assets)
        {
            Player player = new Player
            {
                // Assuming "Player" is the key used when adding the player's sprite sheet in the AssetManager.
                SpriteSheet = assets.GetSpriteSheet("Player"),
                CurrentAnimation = "SmallFacingRight" // Set the initial animation to "FacingRight"
            };

            // Here you could set up additional properties or configurations for the player.
            // e.g., player.Speed = 5.0f;

            return player;
        }

        // If you have additional game objects, like enemies, you can create methods for them too.
        public static GameObject CreateEnemy(int startX, int startY)
        {
            // Create a new GameObject instance for the enemy
            GameObject enemy = new GameObject
            {
                X = startX, // Set the starting X position
                Y = startY, // Set the starting Y position
                Width = 50, // Set the width of the enemy
                Height = 50, // Set the height of the enemy
                Color = Color.Brown // Give the enemy a brown color for now
            };

            // If you have specific logic or properties for enemies, you can extend the GameObject class and create an Enemy class
            // In the future, you can add a spritesheet to the enemy here.

            return enemy;
        }
        // You can continue adding factory methods for different types of game objects here.
    }
}
