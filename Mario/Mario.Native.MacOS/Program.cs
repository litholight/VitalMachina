using System;
using System.Text;
using System.Threading.Tasks;
using GameDevelopmentTools;
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
        private static DateTime lastUpdate = DateTime.Now;
        private static DateTime lastVelocityUpdate = DateTime.Now;
        private static string lastCollisionText = "";

        static async Task Main(string[] args)
        {
            bool showBoundingBoxes =
                Environment.GetEnvironmentVariable("DEBUG_SHOW_BOUNDING_BOXES") == "true";
            bool showPointerCoordinates =
                Environment.GetEnvironmentVariable("DEBUG_SHOW_POINTER_COORDINATES") == "true";

            // Initialize your debug config with the fetched flags
            var debugConfig = new DebugConfig
            {
                ShowBoundingBoxes = showBoundingBoxes,
                ShowPointerCoordinates = showPointerCoordinates
            };

            var graphicsRenderer = new SDL2GraphicsRenderer();
            await graphicsRenderer.Initialize();

            // Create the physics engine instance
            BasicPhysicsEngine marioWorld = new BasicPhysicsEngine();

            // Initialize the game with the physics engine
            var gameState = GameInitializer.InitializeGame(marioWorld);

            var lastTick = DateTime.Now;
            bool running = true;
            SDL.SDL_Event e;
            while (running)
            {
                var currentTick = DateTime.Now;
                var deltaTime = (float)(currentTick - lastTick).TotalSeconds;
                lastTick = currentTick;

                // Update the physics engine first
                marioWorld.Update(deltaTime);

                // Clear the screen at the start of each frame
                await graphicsRenderer.ClearScreen();

                // Inside your main game loop, after updating physics and before rendering:
                UpdateCollisionInfoDisplay(gameState, marioWorld.CollisionResults);

                // Inside your game loop, after updating physics:
                var currentSceneGameObjects = gameState.SceneManager.CurrentScene.GameObjects;
                SyncPhysicsWithGameObjects(marioWorld, currentSceneGameObjects);

                if (debugConfig.ShowBoundingBoxes)
                {
                    // This would be a place to invoke debug rendering for bounding boxes
                    // You would need to implement a method that can render these based on each gameObject
                    RenderBoundingBoxes(graphicsRenderer, currentSceneGameObjects);
                }

                // Input handling
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    HandleInput(ref running, gameState, e);
                }

                // Update game logic
                gameState.Update(deltaTime);

                UpdateVelocityDisplay(gameState);

                // Render the current scene
                await gameState.SceneManager.CurrentScene.Render(graphicsRenderer);

                if (debugConfig.ShowPointerCoordinates)
                {
                    int pointerX,
                        pointerY;
                    SDL.SDL_GetMouseState(out pointerX, out pointerY);

                    string pointerCoordsText = $"Pointer: ({pointerX}, {pointerY})";

                    string fontPath = Path.Combine(
                        AppContext.BaseDirectory,
                        "Assets",
                        "Roboto-Regular.ttf"
                    );
                    int fontSize = 24;

                    // Call the updated DrawText method with fontPath and fontSize
                    await graphicsRenderer.DrawText(
                        pointerCoordsText,
                        fontPath,
                        fontSize,
                        Color.White,
                        .15F, // X position in meters
                        .15F // Y position in meters
                    );
                }

                // Update the screen with the current rendering
                await graphicsRenderer.Present();
            }

            graphicsRenderer.Cleanup(); // Clean up SDL resources
        }

        private static void UpdateVelocityDisplay(GameState gameState)
        {
            Scene currentScene = gameState.SceneManager.CurrentScene;
            Player player = currentScene.GameObjects.OfType<Player>().FirstOrDefault();
            TextObject velocityDisplay = currentScene
                .GameObjects.OfType<TextObject>()
                .FirstOrDefault(t => t.Id == "VelocityDisplay");

            // Update the display only if at least 200 milliseconds have passed
            if (
                velocityDisplay != null
                && player != null
                && (DateTime.Now - lastVelocityUpdate).TotalMilliseconds > 200
            )
            {
                // Format the velocity to show only two decimal places
                string formattedVelocityX = player.PhysicsBody.VelocityX.ToString("F2");
                string formattedVelocityY = player.PhysicsBody.VelocityY.ToString("F2");

                velocityDisplay.Text =
                    $"Velocity: (X: {formattedVelocityX}, Y: {formattedVelocityY})";
                lastVelocityUpdate = DateTime.Now; // Update the time of the last update
            }
        }

        private static void UpdateCollisionInfoDisplay(
            GameState gameState,
            List<CollisionResult> collisionResults
        )
        {
            Scene currentScene = gameState.SceneManager.CurrentScene;
            TextObject collisionInfoDisplay = currentScene
                .GameObjects.OfType<TextObject>()
                .FirstOrDefault(t => t.Id == "CollisionInfoDisplay");

            if (collisionInfoDisplay == null)
                return;

            StringBuilder sb = new StringBuilder();
            if (collisionResults.Any())
            {
                foreach (var collision in collisionResults)
                {
                    sb.AppendLine($"Collision: {collision.OtherBody.Id} at {collision.Direction}");
                }
            }
            else
            {
                sb.Append("No collisions");
            }

            // Only update the display if the text has changed and it's been at least 200 milliseconds
            if (
                lastCollisionText != sb.ToString()
                && (DateTime.Now - lastUpdate).TotalMilliseconds > 200
            )
            {
                collisionInfoDisplay.Text = sb.ToString();
                lastCollisionText = sb.ToString();
                lastUpdate = DateTime.Now;
            }
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

        private static void RenderBoundingBoxes(
            SDL2GraphicsRenderer graphicsRenderer,
            IEnumerable<GameObject> gameObjects
        )
        {
            foreach (var gameObject in gameObjects)
            {
                // Cast the float values to int before passing them to DrawRectangle
                float x = gameObject.X;
                float y = gameObject.Y;
                float width = gameObject.Width;
                float height = gameObject.Height;

                graphicsRenderer.DrawBoundingBox(Color.Red, x, y, width, height);
            }
        }

        private static void HandleInput(ref bool running, GameState gameState, SDL.SDL_Event e)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    running = false;
                    break;

                case SDL.SDL_EventType.SDL_KEYDOWN:
                    var actionDown = InputTranslator.TranslateKeyToGameAction(e.key.keysym.sym);
                    if (actionDown.HasValue)
                    {
                        gameState.HandleInput(actionDown.Value, true); // Handle the key down event
                    }
                    break;

                case SDL.SDL_EventType.SDL_KEYUP:
                    var actionUp = InputTranslator.TranslateKeyToGameAction(e.key.keysym.sym);
                    if (actionUp.HasValue)
                    {
                        gameState.HandleInput(actionUp.Value, false); // Handle the key up event
                    }
                    break;
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
                case SDL.SDL_Keycode.SDLK_SPACE: // Assuming spacebar is the jump key
                    return GameAction.Jump;
                default:
                    return null; // No action for this key
            }
        }
    }
}
