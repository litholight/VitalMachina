using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mario.Blazor.WebImplementations
{
    public class BlazorGraphicsRenderer : IGraphicsRenderer
    {
        private readonly IJSRuntime _jsRuntime;
        private ElementReference _canvasElement;
        private const float PixelsPerMeter = 66.0f;


        public BlazorGraphicsRenderer(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeCanvas(ElementReference canvasElement)
        {
            _canvasElement = canvasElement;
            await _jsRuntime.InvokeVoidAsync("initializeCanvas", _canvasElement);
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public async Task ClearScreen()
        {
            await _jsRuntime.InvokeVoidAsync("renderingFunctions.clearCanvas", _canvasElement);
            await _jsRuntime.InvokeVoidAsync(
                "renderingFunctions.setCanvasBackground",
                _canvasElement,
                "blue"
            );
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

            await _jsRuntime.InvokeVoidAsync(
                "drawTexture",
                _canvasElement,
                texturePath,
                pixelX,
                pixelY,
                pixelWidth,
                pixelHeight
            );
        }


        public async Task DrawRectangle(Color color, float x, float y, float width, float height)
        {
            string colorHex = color.ToHexString();
            int pixelX = (int)(x * PixelsPerMeter);
            int pixelY = (int)(y * PixelsPerMeter);
            int pixelWidth = (int)(width * PixelsPerMeter);
            int pixelHeight = (int)(height * PixelsPerMeter);

            await _jsRuntime.InvokeVoidAsync(
                "drawRectangle",
                _canvasElement,
                pixelX,
                pixelY,
                pixelWidth,
                pixelHeight,
                colorHex
            );
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
            // Convert destination meters to pixels
            int pixelDestX = (int)(destX * PixelsPerMeter);
            int pixelDestY = (int)(destY * PixelsPerMeter);
            int pixelDestWidth = (int)(destWidth * PixelsPerMeter);
            int pixelDestHeight = (int)(destHeight * PixelsPerMeter);

            await _jsRuntime.InvokeVoidAsync(
                "drawSpritePart",
                _canvasElement,
                imagePath,
                srcX,
                srcY,
                srcWidth,
                srcHeight,
                pixelDestX,
                pixelDestY,
                pixelDestWidth,
                pixelDestHeight
            );
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

            // Convert the color to a CSS-compatible format
            string colorHex = color.ToHexString();

            // You might want to scale the font size based on your PixelsPerMeter if needed
            // This is an optional step and can be adjusted based on the visual needs
            int pixelFontSize = (int)(fontSize * (PixelsPerMeter / 66.0)); // Example scale based on original pixel density

            await _jsRuntime.InvokeVoidAsync(
                "drawText",
                _canvasElement,
                text,
                fontPath,
                pixelFontSize,
                colorHex,
                pixelX,
                pixelY
            );
        }

        public Task Present()
        {
            // This method might be a no-op in Blazor as the drawing commands might be immediate.
            return Task.CompletedTask;
        }
    }
}
