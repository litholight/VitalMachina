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

        public async Task DrawRectangle(Color color, float x, float y, float width, float height)
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

        public async Task DrawSpritePart(
            string imagePath,
            int srcX,
            int srcY,
            int srcWidth,
            int srcHeight,
            float destX,
            float destY,
            float destWidth,
            float destHeight
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
            // This method might be a no-op in Blazor as the drawing commands might be immediate.
            return Task.CompletedTask;
        }
    }
}
