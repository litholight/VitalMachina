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
                Id = "Enemy", // Set the ID of the enemy
                X = startX, // Set the starting X position
                Y = startY, // Set the starting Y position
                Width = 50, // Set the width of the enemy
                Height = 50, // Set the height of the enemy
                Color = Color.Orange // Give the enemy a brown color for now
            };

            // If you have specific logic or properties for enemies, you can extend the GameObject class and create an Enemy class
            // In the future, you can add a spritesheet to the enemy here.

            return enemy;
        }

        public static GameObject CreateGround(float x, float y, float width, float height)
        {
            return new GameObject
            {
                Id = "Ground", // Unique identifier for the ground object
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Color = Color.Brown, // Assuming you have a way to set color or texture
                // If using sprites/textures, set the appropriate properties here
            };
        }

        // You can continue adding factory methods for different types of game objects here.
    }
}
