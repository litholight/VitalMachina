using System.Threading.Tasks;
using Mario.Common.Abstractions;

namespace Mario.Common.Models
{
    public class TextObject : GameObject
    {
        public string Text { get; set; } // The text to be displayed
        public string FontPath { get; set; } // Path to the font file
        public int FontSize { get; set; } = 24; // Default font size

        // Override the Render method to handle text rendering specifically
        public override async Task Render(IGraphicsRenderer renderer)
        {
            // If there's text and a FontPath, use a renderer method designed for text.
            // This assumes the renderer supports drawing text directly.
            if (!string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(FontPath))
            {
                await renderer.DrawText(Text, FontPath, FontSize, Color, X, Y);
            }
            else
            {
                // If no text is set, or no FontPath is provided, fall back to the base class's render method.
                await base.Render(renderer);
            }
        }
    }
}
