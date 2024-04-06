namespace PhysicsEngine.Core.Physics
{
    public class PhysicsBody
    {
        public string Id { get; set; } // Unique identifier
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Mass { get; set; } = 1.0f;

        // Additional properties like acceleration, forces, etc., can be added here
    }
}
