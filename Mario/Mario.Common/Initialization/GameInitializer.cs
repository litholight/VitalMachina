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
            GameObject enemy = GameObjectFactory.CreateEnemy(assets, 4.55F, 1.52F);

            // Use the factory to create a player with its sprite sheet and animations ready
            Player player = GameObjectFactory.CreatePlayer(assets);

            // Create a PhysicsBody for the player and add it to the physics engine
            var playerPhysicsBody = new PhysicsBody
            {
                Id = player.Id,
                X = player.X,
                Y = player.Y,
                Height = player.Height,
                Width = player.Width,
                Mass = 50.0f // Arbitrary mass value
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
                FontPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Roboto-Regular.ttf"),
                FontSize = 24,
                Color = Color.White,
                X = .15F,
                Y = .61F
            };

            TextObject velocityDisplay = new TextObject
            {
                Id = "VelocityDisplay",
                Text = "Velocity: (0, 0)",
                FontPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Roboto-Regular.ttf"),
                FontSize = 24,
                Color = Color.White,
                X = .15F, // Adjust position as needed
                Y = .91F // Adjust position as needed
            };

            var groundBody = new PhysicsBody
            {
                Id = "Ground",
                X = 0F, // Assuming ground spans the entire bottom of your scene
                Y = 9.10F, // Position Y at the bottom of your scene
                Width = 12.12F, // Match your scene width
                Height = 0F, // Arbitrary height for the ground
                IsStatic = true
            };
            physicsEngine.AddBody(groundBody);

            // Now pass the player to the GameState constructor
            GameState gameState = new GameState(player);

            Scene startScene = new Scene("StartScene");
            startScene.AddGameObject(enemy);
            startScene.AddGameObject(player);
            startScene.AddGameObject(collisionInfoDisplay);
            startScene.AddGameObject(velocityDisplay);

            gameState.SceneManager.AddScene("StartScene", startScene);
            gameState.SceneManager.SwitchToScene("StartScene");

            return gameState;
        }
    }
}
