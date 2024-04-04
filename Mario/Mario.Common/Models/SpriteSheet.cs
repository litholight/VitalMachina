namespace Mario.Common.Models
{
    public class SpriteSheet
    {
        public string ImagePath { get; set; }

        // Add these properties to hold the dimensions of a single frame
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        private Dictionary<string, List<(int FrameX, int FrameY)>> animations =
            new Dictionary<string, List<(int FrameX, int FrameY)>>();

        // Modify the constructor to accept frame width and height
        public SpriteSheet(string imagePath, int frameWidth, int frameHeight)
        {
            ImagePath = imagePath;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
        }

        public void AddAnimation(string name, List<(int FrameX, int FrameY)> frames)
        {
            animations[name] = frames;
        }

        public (int FrameX, int FrameY) GetAnimationFrame(string animationName, int frameIndex)
        {
            if (animations.TryGetValue(animationName, out var frames))
            {
                return frames[frameIndex % frames.Count];
            }
            return (0, 0);
        }

        public int GetFrameCount(string animationName)
        {
            if (animations.ContainsKey(animationName))
            {
                return animations[animationName].Count;
            }
            return 0;
        }
    }
}
