namespace Mario.Common.Models
{
    public class CameraObject : GameObject
    {
        public GameObject Target { get; set; } // The target the camera follows

        public override void Update(float deltaTime)
        {
            if (Target != null)
            {
                // Example: Center the camera on the target
                X = Target.X + Target.Width / 2 - this.Width / 2;
                Y = Target.Y + Target.Height / 2 - this.Height / 2;
            }

            // Additional camera logic here
        }

        // Optionally, override other methods if the camera needs special handling
    }
}
