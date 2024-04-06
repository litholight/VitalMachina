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
        public bool IsStatic { get; set; } = false; // Indicates if the body is movable

        // Bounding box dimensions for collision detection
        public float Width { get; set; }
        public float Height { get; set; }

        // Additional properties like acceleration, forces, etc., can be added here
    }
}
