using System.Threading.Tasks;
using Mario.Common.Models;

namespace Mario.Common.Abstractions
{
    public interface IGraphicsRenderer
    {
        Task Initialize();
        Task ClearScreen();
        Task DrawTexture(string texturePath, float x, float y, float width, float height);
        Task DrawRectangle(Color color, float x, float y, float width, float height);
        Task DrawSpritePart(
            string imagePath,
            int srcX, // These remain as pixels because they reference sprite sheet coordinates
            int srcY,
            int srcWidth,
            int srcHeight,
            float destX, // Destination coordinates need to be in meters
            float destY,
            float destWidth,
            float destHeight
        );
        Task DrawText(string text, string fontPath, int fontSize, Color color, float x, float y);
        Task Present(); // Update the screen with the current rendering.
    }
}
