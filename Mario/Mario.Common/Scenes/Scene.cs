using System;
using System.Collections.Generic;
using Mario.Common.Abstractions;
using Mario.Common.Input;
using Mario.Common.Models;

namespace Mario.Common.Scenes
{
    public class Scene
    {
        public string Name { get; set; }
        public List<GameObject> GameObjects { get; private set; } = new List<GameObject>();

        public event Action<Scene> OnSceneLoaded;
        public event Action<Scene> OnSceneUnloaded;

        public Scene(string name)
        {
            Name = name;
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        public async Task Render(IGraphicsRenderer renderer)
        {
            foreach (var gameObject in GameObjects)
            {
                await gameObject.Render(renderer);
            }
        }

        public void HandleInput(GameAction action, bool isKeyDown)
        {
            // The HandleInput method in Scene is used for handling actions specific to the current scene.
            // This can include navigating a menu, interacting with scene-specific objects, or controlling
            // characters within a particular level. This method iterates over all GameObjects that
            // implement IInputHandler, allowing each object to respond to input based on its own logic.

            foreach (var handler in GameObjects.OfType<IInputHandler>())
            {
                handler.HandleAction(action, isKeyDown);
            }
            // Additional scene-specific input handling logic can be implemented here.
        }

        // Called when the scene is about to be shown
        public void Load()
        {
            // Initialize scene-specific resources or game objects here

            OnSceneLoaded?.Invoke(this);
        }

        // Called when the scene is transitioning out
        public void Unload()
        {
            // Clean up scene-specific resources here

            OnSceneUnloaded?.Invoke(this);
        }

        // Update logic for the scene, if any
        public void Update(float deltaTime)
        {
            // Update game objects or handle scene-specific logic
            foreach (var gameObject in GameObjects)
            {
                // For example, update game object positions or states
                gameObject.Update(deltaTime);
            }
        }

        // You might add more methods here for scene-specific functionality
    }
}
