using System.Collections.Generic;
using Mario.Common.Input;
using Mario.Common.Scenes;

namespace Mario.Common.Models
{
    public class GameState
    {
        public SceneManager SceneManager { get; private set; } = new SceneManager();
        public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        public Player Player { get; set; }
        public int CurrentLevel { get; set; }
        public int Score { get; set; }

        public GameState(Player player)
        {
            this.Player = player;
            GameObjects.Add(player);
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        public void Update(float deltaTime)
        {
            // Update each game object
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(deltaTime);
                HandleInteractions(gameObject);
            }
        }

        private void HandleInteractions(GameObject gameObject)
        {
            // Example: Check for and handle interactions between the provided gameObject and others in the GameObjects list
            foreach (var other in GameObjects)
            {
                if (gameObject != other && IsColliding(gameObject, other))
                {
                    // Handle collision or interaction
                    // This could involve calling a method on the gameObject or other object, updating state, etc.
                }
            }
        }

        private bool IsColliding(GameObject a, GameObject b)
        {
            // Implement collision detection logic
            return false;
        }

        public void HandleInput(GameAction action, bool isKeyDown)
        {
            // The HandleInput method in GameState is typically used for handling global game actions.
            // Examples include pausing the game, opening a global settings menu, or other actions
            // that affect the game regardless of the current scene or game object states.
            // This method delegates the handling of the action directly to the player,
            // allowing for a centralized point of input processing for player-related actions.

            Player?.HandleAction(action, isKeyDown);
            // Additional global input handling logic can be implemented here.
        }
        // Methods for scene transitions, score updates, etc.
    }
}
