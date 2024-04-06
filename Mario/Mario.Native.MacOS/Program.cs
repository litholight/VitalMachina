using System;
using System.Threading.Tasks;
using Mario.Common.Initialization;
using Mario.Common.Input;
using Mario.Common.Models;
using Mario.Common.Scenes; // Make sure to include the namespace for SceneManager
using Mario.Common.Services;
using PhysicsEngine.Core.Physics;
using SDL2;

namespace Mario.Native.MacOS
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var graphicsRenderer = new SDL2GraphicsRenderer();
            await graphicsRenderer.Initialize();

            // Create the physics engine instance
            BasicPhysicsEngine physicsEngine = new BasicPhysicsEngine();

            // Initialize the game with the physics engine
            var gameState = GameInitializer.InitializeGame(physicsEngine);

            var lastTick = DateTime.Now;
            bool running = true;
            SDL.SDL_Event e;
            while (running)
            {
                var currentTick = DateTime.Now;
                var deltaTime = (float)(currentTick - lastTick).TotalSeconds;
                lastTick = currentTick;

                physicsEngine.ApplyGravity();

                // Update the physics engine first
                physicsEngine.Update(deltaTime);

                // Inside your game loop, after updating physics:
                var currentSceneGameObjects = gameState.SceneManager.CurrentScene.GameObjects;
                SyncPhysicsWithGameObjects(physicsEngine, currentSceneGameObjects);

                // Clear the screen at the start of each frame
                await graphicsRenderer.ClearScreen();

                // Input handling
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            running = false;
                            break;

                        case SDL.SDL_EventType.SDL_KEYDOWN:
                            var actionDown = InputTranslator.TranslateKeyToGameAction(
                                e.key.keysym.sym
                            );
                            if (actionDown.HasValue)
                            {
                                gameState.HandleInput(actionDown.Value, true); // true for key down
                            }
                            break;

                        case SDL.SDL_EventType.SDL_KEYUP:
                            var actionUp = InputTranslator.TranslateKeyToGameAction(
                                e.key.keysym.sym
                            );
                            if (actionUp.HasValue)
                            {
                                gameState.HandleInput(actionUp.Value, false); // false for key up
                            }
                            break;
                    }
                }

                // Update game logic
                gameState.Update(deltaTime);

                // Render the current scene
                await gameState.SceneManager.CurrentScene.Render(graphicsRenderer);

                // Update the screen with the current rendering
                await graphicsRenderer.Present();
            }

            graphicsRenderer.Cleanup(); // Clean up SDL resources
        }

        private static void SyncPhysicsWithGameObjects(
            IPhysicsEngine physicsEngine,
            IEnumerable<GameObject> gameObjects
        )
        {
            foreach (var body in physicsEngine.GetAllBodies())
            {
                var gameObject = gameObjects.FirstOrDefault(go => go.Id == body.Id);
                if (gameObject != null)
                {
                    gameObject.X = body.X;
                    gameObject.Y = body.Y;
                    // Update other properties as needed
                }
            }
        }
    }

    public static class InputTranslator
    {
        public static GameAction? TranslateKeyToGameAction(SDL.SDL_Keycode keycode)
        {
            switch (keycode)
            {
                case SDL.SDL_Keycode.SDLK_UP:
                    return GameAction.MoveUp;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    return GameAction.MoveDown;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    return GameAction.MoveLeft;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    return GameAction.MoveRight;
                // Add cases for other keycodes as needed
                default:
                    return null; // No action for this key
            }
        }
    }
}
