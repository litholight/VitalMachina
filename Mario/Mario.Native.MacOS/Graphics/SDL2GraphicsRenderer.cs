using System;
using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Models;
using Mario.Native.MacOS.NativeBindings;
using GameDevelopmentTools;
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

            if (SDL_ttf.TTF_Init() == -1)
            {
                Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: " + SDL.SDL_GetError());
                throw new Exception("Failed to initialize SDL_ttf.");
            }

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

        private SDL_Color ToSDLColor(Color color)
        {
            return new SDL_Color { r = color.R, g = color.G, b = color.B, a = color.A };
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
            // Load the font
            IntPtr font = SDL_ttf.TTF_OpenFont(fontPath, fontSize);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load font! SDL_ttf Error: " + SDL.SDL_GetError());
                return;
            }

            // Render the text to a surface
            SDL_Color sdlColor = ToSDLColor(color); // Convert your custom Color to SDL_Color
            IntPtr textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, sdlColor);
            if (textSurface == IntPtr.Zero)
            {
                SDL_ttf.TTF_CloseFont(font);
                Console.WriteLine("Unable to render text surface! SDL Error: " + SDL.SDL_GetError());
                return;
            }

            // Create a texture from the surface
            IntPtr textTexture = SDL.SDL_CreateTextureFromSurface(renderer, textSurface);
            if (textTexture == IntPtr.Zero)
            {
                Console.WriteLine("Unable to create texture from rendered text! SDL Error: " + SDL.SDL_GetError());
            }
            else
            {
                // Get the texture dimensions
                SDL.SDL_QueryTexture(textTexture, out _, out _, out int textWidth, out int textHeight);

                // Set the rendering space and render to screen
                SDL.SDL_Rect renderQuad = new SDL.SDL_Rect { x = (int)x, y = (int)y, w = textWidth, h = textHeight };
                SDL.SDL_RenderCopy(renderer, textTexture, IntPtr.Zero, ref renderQuad);
            }

            // Clean up
            SDL.SDL_FreeSurface(textSurface);
            SDL.SDL_DestroyTexture(textTexture);
            SDL_ttf.TTF_CloseFont(font);
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
        public Task DrawBoundingBox(Color color, int x, int y, int width, int height)
        {
            // Set the color for drawing. This sets the color for the outline.
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);

            // Define the rectangle (this remains the same).
            SDL.SDL_Rect rect = new SDL.SDL_Rect
            {
                x = x,
                y = y,
                w = width,
                h = height
            };

            // Draw the rectangle outline instead of filling it.
            SDL.SDL_RenderDrawRect(renderer, ref rect);

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
            SDL_ttf.TTF_Quit();
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
