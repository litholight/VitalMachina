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

            GameObject ground = GameObjectFactory.CreateGround(0, 500, 800, 50); // Example dimensions

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

            var groundPhysicsBody = new PhysicsBody
            {
                Id = ground.Id,
                X = ground.X,
                Y = ground.Y,
                Width = ground.Width,
                Height = ground.Height,
                IsStatic = true, // Important for the ground to not fall
            };
            physicsEngine.AddBody(groundPhysicsBody);

            // Now pass the player to the GameState constructor
            GameState gameState = new GameState(player);

            Scene startScene = new Scene("StartScene");
            startScene.AddGameObject(ground);
            startScene.AddGameObject(enemy);
            startScene.AddGameObject(player);

            gameState.SceneManager.AddScene("StartScene", startScene);
            gameState.SceneManager.SwitchToScene("StartScene");

            return gameState;
        }
    }
}
