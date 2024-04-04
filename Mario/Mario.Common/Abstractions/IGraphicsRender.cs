using System.Threading.Tasks;
using Mario.Common.Models;

namespace Mario.Common.Abstractions
{
    public interface IGraphicsRenderer
    {
        Task Initialize();
        Task ClearScreen();
        Task DrawTexture(string texturePath, int x, int y, int width, int height);
        Task DrawRectangle(Color color, int x, int y, int width, int height);
        Task DrawSpritePart(
            string imagePath,
            int srcX,
            int srcY,
            int srcWidth,
            int srcHeight,
            int destX,
            int destY,
            int destWidth,
            int destHeight
        );
        Task DrawText(string text, string fontPath, int fontSize, Color color, float x, float y);
        Task Present(); // Update the screen with the current rendering.
    }
}
