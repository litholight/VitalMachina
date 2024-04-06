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
                Y = player.Y
            };
            player.PhysicsBody = playerPhysicsBody;
            physicsEngine.AddBody(playerPhysicsBody);

            var enemyPhysicsBody = new PhysicsBody
            {
                Id = enemy.Id,
                X = enemy.X,
                Y = enemy.Y
            };
            physicsEngine.AddBody(enemyPhysicsBody);

            var groundBody = new PhysicsBody
            {
                Id = "Ground",
                X = 0, // Assuming ground spans the entire bottom of your scene
                Y = 500, // Position Y at the bottom of your scene
                Width = 800, // Match your scene width
                Height = 50, // Arbitrary height for the ground
                IsStatic = true
            };
            physicsEngine.AddBody(groundBody);

            // Now pass the player to the GameState constructor
            GameState gameState = new GameState(player);

            Scene startScene = new Scene("StartScene");
            startScene.AddGameObject(enemy);
            startScene.AddGameObject(player);

            gameState.SceneManager.AddScene("StartScene", startScene);
            gameState.SceneManager.SwitchToScene("StartScene");

            return gameState;
        }
    }
}
