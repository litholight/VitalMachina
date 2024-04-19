using System;
using System.Threading.Tasks;
using GameDevelopmentTools;
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
        private const float PixelsPerMeter = 66.0f;

        public SDL2GraphicsRenderer()
        {
            Initialize();
        }

        public async Task Initialize()
        {
            try
            {
                SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
                SDL_image.IMG_Init(
                    SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_JPG
                );

                if (SDL_ttf.TTF_Init() == -1)
                {
                    throw new Exception("Failed to initialize SDL_ttf: " + SDL.SDL_GetError());
                }

                window = SDL.SDL_CreateWindow(
                    "Mario Game",
                    SDL.SDL_WINDOWPOS_CENTERED,
                    SDL.SDL_WINDOWPOS_CENTERED,
                    800,
                    600,
                    SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
                );
                if (window == IntPtr.Zero)
                {
                    throw new Exception("Failed to create SDL window: " + SDL.SDL_GetError());
                }

                renderer = SDL.SDL_CreateRenderer(
                    window,
                    -1,
                    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED
                        | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC
                );
                if (renderer == IntPtr.Zero)
                {
                    throw new Exception("Failed to create renderer: " + SDL.SDL_GetError());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialization failed: " + ex.Message);
                Cleanup();
                throw;
            }
        }

        public async Task ClearScreen()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255); // Blue background
            SDL.SDL_RenderClear(renderer);
        }

        public async Task DrawTexture(
            string texturePath,
            float x,
            float y,
            float width,
            float height
        )
        {
            int pixelX = (int)(x * PixelsPerMeter);
            int pixelY = (int)(y * PixelsPerMeter);
            int pixelWidth = (int)(width * PixelsPerMeter);
            int pixelHeight = (int)(height * PixelsPerMeter);
            // You'll need to implement texture loading and drawing here
        }

        private SDL_Color ToSDLColor(Color color)
        {
            return new SDL_Color
            {
                r = color.R,
                g = color.G,
                b = color.B,
                a = color.A
            };
        }

        public async Task DrawText(
            string text,
            string fontPath,
            int fontSize,
            Color color,
            float x, // x-coordinate in meters
            float y // y-coordinate in meters
        )
        {
            // Convert meter coordinates to pixel coordinates
            int pixelX = (int)(x * PixelsPerMeter);
            int pixelY = (int)(y * PixelsPerMeter);

            // Optionally adjust the font size based on meters to pixels conversion
            // int pixelFontSize = (int)(fontSize * PixelsPerMeter); // Uncomment if scaling font size is needed

            // Load the font with the original fontSize or pixelFontSize if scaled
            IntPtr font = SDL_ttf.TTF_OpenFont(fontPath, fontSize); // or pixelFontSize
            if (font == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load font! SDL_ttf Error: " + SDL.SDL_GetError());
                return;
            }

            // Convert your custom Color to SDL_Color
            SDL_Color sdlColor = ToSDLColor(color);

            // Render the text to a surface
            IntPtr textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, sdlColor);
            if (textSurface == IntPtr.Zero)
            {
                SDL_ttf.TTF_CloseFont(font);
                Console.WriteLine(
                    "Unable to render text surface! SDL Error: " + SDL.SDL_GetError()
                );
                return;
            }

            // Create a texture from the surface
            IntPtr textTexture = SDL.SDL_CreateTextureFromSurface(renderer, textSurface);
            if (textTexture == IntPtr.Zero)
            {
                SDL.SDL_FreeSurface(textSurface);
                SDL_ttf.TTF_CloseFont(font);
                Console.WriteLine(
                    "Unable to create texture from rendered text! SDL Error: " + SDL.SDL_GetError()
                );
                return;
            }

            // Get the texture dimensions
            SDL.SDL_QueryTexture(textTexture, out _, out _, out int textWidth, out int textHeight);

            // Set the rendering space and render to screen
            SDL.SDL_Rect renderQuad = new SDL.SDL_Rect
            {
                x = pixelX,
                y = pixelY,
                w = textWidth,
                h = textHeight
            };
            SDL.SDL_RenderCopy(renderer, textTexture, IntPtr.Zero, ref renderQuad);

            // Clean up
            SDL.SDL_FreeSurface(textSurface);
            SDL.SDL_DestroyTexture(textTexture);
            SDL_ttf.TTF_CloseFont(font);
        }

        public Task DrawRectangle(Color color, float x, float y, float width, float height)
        {
            // Convert from meters to pixels for rendering purposes
            int pixelX = (int)(x * PixelsPerMeter);
            int pixelY = (int)(y * PixelsPerMeter);
            int pixelWidth = (int)(width * PixelsPerMeter);
            int pixelHeight = (int)(height * PixelsPerMeter);

            // Set the color for drawing
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);

            // Define the rectangle with converted pixel coordinates
            SDL.SDL_Rect rect = new SDL.SDL_Rect
            {
                x = pixelX,
                y = pixelY,
                w = pixelWidth,
                h = pixelHeight
            };

            // Fill the rectangle with the specified color
            SDL.SDL_RenderFillRect(renderer, ref rect);

            return Task.CompletedTask;
        }

        public Task DrawBoundingBox(Color color, float x, float y, float width, float height)
        {
            // Convert from meters to pixels for rendering purposes
            int pixelX = (int)(x * PixelsPerMeter);
            int pixelY = (int)(y * PixelsPerMeter);
            int pixelWidth = (int)(width * PixelsPerMeter);
            int pixelHeight = (int)(height * PixelsPerMeter);

            // Set the color for drawing the outline
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);

            // Define the rectangle with converted pixel coordinates
            SDL.SDL_Rect rect = new SDL.SDL_Rect
            {
                x = pixelX,
                y = pixelY,
                w = pixelWidth,
                h = pixelHeight
            };

            // Draw the rectangle outline instead of filling it
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
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }

        public async Task DrawSpritePart(
            string imagePath,
            int srcX, // Source x-coordinate in pixels (sprite sheet coordinates)
            int srcY, // Source y-coordinate in pixels (sprite sheet coordinates)
            int srcWidth, // Source width in pixels (sprite sheet dimensions)
            int srcHeight, // Source height in pixels (sprite sheet dimensions)
            float destX, // Destination x-coordinate in meters (game world coordinates)
            float destY, // Destination y-coordinate in meters (game world coordinates)
            float destWidth, // Destination width in meters (game world size)
            float destHeight // Destination height in meters (game world size)
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

            // Convert destination meters to pixels
            int pixelDestX = (int)(destX * PixelsPerMeter);
            int pixelDestY = (int)(destY * PixelsPerMeter);
            int pixelDestWidth = (int)(destWidth * PixelsPerMeter);
            int pixelDestHeight = (int)(destHeight * PixelsPerMeter);

            // Define the destination rectangle (where to draw on the screen) with pixel values
            SDL.SDL_Rect destRect = new SDL.SDL_Rect
            {
                x = pixelDestX,
                y = pixelDestY,
                w = pixelDestWidth,
                h = pixelDestHeight
            };

            // Copy the specified part of the sprite sheet to the screen
            SDL.SDL_RenderCopy(renderer, texture, ref srcRect, ref destRect);
        }
    }
}
