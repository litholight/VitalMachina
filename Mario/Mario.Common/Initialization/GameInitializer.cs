using Mario.Common.Assets;
using Mario.Common.Factories;
using Mario.Common.Models;
using Mario.Common.Scenes;
using PhysicsEngine.Core;
using PhysicsEngine.Core.Physics;

namespace Mario.Common.Initialization
{
    public static class GameInitializer
    {
        public static GameState InitializeGame(BasicPhysicsEngine physicsEngine)
        {
            AssetManager assets = new AssetManager();

            // Assuming CreateEnemy is a static method of GameObjectFactory just like CreatePlayer
            GameObject enemy = GameObjectFactory.CreateEnemy(300, 100);

            // Use the factory to create a player with its sprite sheet and animations ready
            Player player = GameObjectFactory.CreatePlayer(assets);

            // Create a PhysicsBody for the player and add it to the physics engine
            var playerPhysicsBody = new PhysicsBody
            {
                Id = player.Id,
                X = player.X,
                Y = player.Y,
                Height = player.Height,
                Width = player.Width
            };
            player.PhysicsBody = playerPhysicsBody;
            physicsEngine.AddBody(playerPhysicsBody);

            var enemyPhysicsBody = new PhysicsBody
            {
                Id = enemy.Id,
                X = enemy.X,
                Y = enemy.Y,
                Height = enemy.Height,
                Width = enemy.Width
            };
            physicsEngine.AddBody(enemyPhysicsBody);

            // Add a TextObject to the scene to display collision information
            TextObject collisionInfoDisplay = new TextObject
            {
                Id = "CollisionInfoDisplay",
                Text = "No collisions yet",
                FontPath = "/Users/seanstoneburner/VitalMachina/Mario/Mario.Common/Assets/Roboto-Regular.ttf", // Ensure this points to a valid font file
                FontSize = 24,
                Color = Color.White, // Assuming white color for visibility
                X = 10, // Position the text object on the screen as desired
                Y = 40
            };

            var groundBody = new PhysicsBody
            {
                Id = "Ground",
                X = 0, // Assuming ground spans the entire bottom of your scene
                Y = 600, // Position Y at the bottom of your scene
                Width = 800, // Match your scene width
                Height = 0, // Arbitrary height for the ground
                IsStatic = true
            };
            physicsEngine.AddBody(groundBody);

            // Now pass the player to the GameState constructor
            GameState gameState = new GameState(player);

            Scene startScene = new Scene("StartScene");
            startScene.AddGameObject(enemy);
            startScene.AddGameObject(player);
            startScene.AddGameObject(collisionInfoDisplay);

            gameState.SceneManager.AddScene("StartScene", startScene);
            gameState.SceneManager.SwitchToScene("StartScene");

            return gameState;
        }
    }
}
