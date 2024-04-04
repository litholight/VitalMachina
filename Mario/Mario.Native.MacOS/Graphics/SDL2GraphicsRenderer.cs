using System;
using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Models;
using Mario.Native.MacOS.NativeBindings;
using SDL2;

namespace Mario.Native.MacOS
{
    public class SDL2GraphicsRenderer : IGraphicsRenderer
    {
        private IntPtr window = IntPtr.Zero;
        private IntPtr renderer = IntPtr.Zero;
        private Dictionary<string, IntPtr> loadedTextures = new();

        public SDL2GraphicsRenderer()
        {
            Initialize();
        }

        public async Task Initialize()
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            SDL_image.IMG_Init(
                SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_JPG
            );

            window = SDL.SDL_CreateWindow(
                "Mario Game",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                800,
                600,
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
            );
            renderer = SDL.SDL_CreateRenderer(
                window,
                -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED
                    | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC
            );
        }

        public async Task ClearScreen()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255); // Blue background
            SDL.SDL_RenderClear(renderer);
        }

        public async Task DrawTexture(string texturePath, int x, int y, int width, int height)
        {
            // You'll need to implement texture loading and drawing here
        }

        public async Task DrawText(
            string text,
            string fontPath,
            int fontSize,
            Color color,
            float x,
            float y
        )
        {
            // You'll need to implement text drawing here
        }

        public Task DrawRectangle(Color color, int x, int y, int width, int height)
        {
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);

            SDL.SDL_Rect rect = new SDL.SDL_Rect
            {
                x = x,
                y = y,
                w = width,
                h = height
            };

            SDL.SDL_RenderFillRect(renderer, ref rect);

            return Task.CompletedTask;
        }

        public Task Present()
        {
            SDL.SDL_RenderPresent(renderer);
            return Task.CompletedTask;
        }

        public void Cleanup()
        {
            // Cleanup logic for SDL2 resources
            if (renderer != IntPtr.Zero)
            {
                SDL.SDL_DestroyRenderer(renderer);
                renderer = IntPtr.Zero;
            }
            if (window != IntPtr.Zero)
            {
                SDL.SDL_DestroyWindow(window);
                window = IntPtr.Zero;
            }
            SDL.SDL_Quit();
        }

        public async Task DrawSpritePart(
            string imagePath,
            int srcX,
            int srcY,
            int srcWidth,
            int srcHeight,
            int destX,
            int destY,
            int destWidth,
            int destHeight
        )
        {
            // Check if the texture is already loaded
            if (!loadedTextures.TryGetValue(imagePath, out IntPtr texture))
            {
                // Load the texture and add it to the dictionary
                texture = SDL_image.IMG_LoadTexture(renderer, imagePath);
                if (texture == IntPtr.Zero)
                {
                    throw new Exception($"Failed to load texture: {imagePath}");
                }
                loadedTextures[imagePath] = texture;
            }

            // Define the source rectangle (part of the sprite sheet)
            SDL.SDL_Rect srcRect = new SDL.SDL_Rect
            {
                x = srcX,
                y = srcY,
                w = srcWidth,
                h = srcHeight
            };

            // Define the destination rectangle (where to draw on the screen)
            SDL.SDL_Rect destRect = new SDL.SDL_Rect
            {
                x = destX,
                y = destY,
                w = destWidth,
                h = destHeight
            };

            // Copy the specified part of the sprite sheet to the screen
            SDL.SDL_RenderCopy(renderer, texture, ref srcRect, ref destRect);
        }
    }
}
