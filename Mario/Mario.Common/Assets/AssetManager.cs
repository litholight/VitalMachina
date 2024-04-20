using System;
using System.Collections.Generic;
using Mario.Common.Models;

namespace Mario.Common.Assets
{
    public class AssetManager
    {
        private readonly Dictionary<string, SpriteSheet> spriteSheets = new();

        public AssetManager()
        {
            LoadAssets();
        }

        private string GetAssetPath(string assetName)
        {
            // For Blazor WebAssembly, assets are served from the wwwroot directory, accessible via relative URLs.
            // For other project types, assets are included in the file system, and their paths can be constructed.
            // This example assumes you have a mechanism to differentiate the running environment.
            // You might use a project-specific flag, configuration setting, or environment variable here.
            bool isBlazorWebAssembly = false; // You need to set this based on your project's runtime environment.

            if (isBlazorWebAssembly)
            {
                // Use a relative URL for assets in Blazor WebAssembly
                return $"./assets/{assetName}";
            }
            else
            {
                // Construct a file system path for other environments
                var basePath = AppContext.BaseDirectory;
                return System.IO.Path.Combine(basePath, "Assets", assetName);
            }
        }

        public void LoadAssets()
        {
            // Assuming frame dimensions and animation frames are known
            var spriteSheetPath = GetAssetPath("mario-spritesheet.png");
            var playerSpriteSheet = new SpriteSheet(spriteSheetPath, 33, 15);

            // Define animations with appropriate frame coordinates
            playerSpriteSheet.AddAnimation(
                "SmallFacingRight",
                new List<(int FrameX, int FrameY)> { (206, 0) }
            );

            playerSpriteSheet.AddAnimation(
                "SmallMovingRight",
                new List<(int FrameX, int FrameY)> { (234, 0), (263, 0), (296, 0) }
            );

            playerSpriteSheet.AddAnimation(
                "SmallFacingLeft",
                new List<(int FrameX, int FrameY)> { (177, 0) }
            );

            playerSpriteSheet.AddAnimation(
                "SmallMovingLeft",
                new List<(int FrameX, int FrameY)> { (143, 0), (114, 0), (83, 0) }
            );

            // Assuming frame dimensions and animation frames are known
            var goombaSpriteSheetPath = GetAssetPath("enemies-spritesheet.png");
            var goombaSpriteSheet = new SpriteSheet(goombaSpriteSheetPath, 26, 26);

            // Define animations with appropriate frame coordinates
            goombaSpriteSheet.AddAnimation(
                "GoombaA",
                new List<(int FrameX, int FrameY)> { (0, 0) }
            );
            spriteSheets.Add("Player", playerSpriteSheet);
            spriteSheets.Add("Goomba", goombaSpriteSheet);
        }

        public SpriteSheet GetSpriteSheet(string key)
        {
            if (spriteSheets.TryGetValue(key, out var spriteSheet))
            {
                return spriteSheet;
            }
            throw new KeyNotFoundException($"SpriteSheet with key '{key}' not found.");
        }
    }
}
