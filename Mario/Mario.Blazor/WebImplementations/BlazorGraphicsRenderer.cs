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

        public BlazorGraphicsRenderer(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeCanvas(ElementReference canvasElement)
        {
            _canvasElement = canvasElement;
            // This call was moved from Initialize to here
            await _jsRuntime.InvokeVoidAsync("initializeCanvas", _canvasElement);
        }

        public Task Initialize()
        {
            // If there's any general initialization logic that doesn't specifically require the canvas element,
            // it can go here. Otherwise, this method might not be necessary.
            return Task.CompletedTask;
        }

        public async Task ClearScreen()
        {
            await _jsRuntime.InvokeVoidAsync("renderingFunctions.clearCanvas", _canvasElement);
            await _jsRuntime.InvokeVoidAsync(
                "renderingFunctions.setCanvasBackground",
                _canvasElement,
                "blue"
            ); // Example color
        }

        public async Task DrawTexture(string texturePath, int x, int y, int width, int height)
        {
            await _jsRuntime.InvokeVoidAsync(
                "drawTexture",
                _canvasElement,
                texturePath,
                x,
                y,
                width,
                height
            );
        }

        public async Task DrawRectangle(Color color, int x, int y, int width, int height)
        {
            string colorHex = color.ToHexString();
            await _jsRuntime.InvokeVoidAsync(
                "drawRectangle",
                _canvasElement,
                x,
                y,
                width,
                height,
                colorHex
            );
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
            string colorHex = color.ToHexString();
            await _jsRuntime.InvokeVoidAsync(
                "drawText",
                _canvasElement,
                text,
                fontPath,
                fontSize,
                colorHex,
                x,
                y
            );
        }

        public Task Present()
        {
            // This might be a no-op in Blazor as the drawing commands might be immediate.
            return Task.CompletedTask;
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
            await _jsRuntime.InvokeVoidAsync(
                "drawSpritePart",
                _canvasElement,
                imagePath,
                srcX,
                srcY,
                srcWidth,
                srcHeight,
                destX,
                destY,
                destWidth,
                destHeight
            );
        }
    }
}
