using Mario.Common.Abstractions;

namespace Mario.Common.Models
{
    public class GameObject
    {
        public float X { get; set; } = 20; // Initial X position
        public float Y { get; set; } = 20; // Initial Y position
        public float Width { get; set; } = 50; // Width
        public float Height { get; set; } = 50; // Height
        public Color Color { get; set; } = Color.Green; // Default to red for visibility
        public string TexturePath { get; set; }
        public SpriteSheet SpriteSheet { get; set; } // Add this
        public string CurrentAnimation { get; set; } // Add this
        private int currentFrame = 0; // Animation frame index
        private float animationTimer = 0f; // Tracks time since last frame change
        public float FrameDuration = 0.1f;

        public virtual void Update(float deltaTime)
        {
            // Update logic for animation timing
            if (SpriteSheet != null && !string.IsNullOrEmpty(CurrentAnimation))
            {
                animationTimer += deltaTime; // Accumulate time

                // Check if it's time to update to the next frame
                if (animationTimer >= FrameDuration)
                {
                    // Move to the next frame
                    currentFrame++;

                    // Reset the timer to count towards the next frame change
                    animationTimer -= FrameDuration;

                    // If the current frame exceeds the number of frames in the animation,
                    // loop back to the first frame
                    int frameCount = SpriteSheet.GetFrameCount(CurrentAnimation);
                    if (currentFrame >= frameCount)
                    {
                        currentFrame = 0;
                    }
                }
            }

            // Include other update logic here (e.g., movement based on input).
        }

        public virtual async Task Render(IGraphicsRenderer renderer)
        {
            if (SpriteSheet != null && !string.IsNullOrEmpty(CurrentAnimation))
            {
                var (FrameX, FrameY) = SpriteSheet.GetAnimationFrame(
                    CurrentAnimation,
                    currentFrame
                );
                await renderer.DrawSpritePart(
                    SpriteSheet.ImagePath,
                    FrameX,
                    FrameY,
                    (int)SpriteSheet.FrameWidth,
                    (int)SpriteSheet.FrameHeight,
                    (int)X,
                    (int)Y,
                    (int)Width,
                    (int)Height
                );
            }
            else if (!string.IsNullOrEmpty(TexturePath))
            {
                await renderer.DrawTexture(TexturePath, (int)X, (int)Y, (int)Width, (int)Height);
            }
            else
            {
                await renderer.DrawRectangle(Color, (int)X, (int)Y, (int)Width, (int)Height);
            }
        }
    }
}
