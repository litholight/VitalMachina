// PhysicsEngine.Core/Entities/PhysicsBody.cs
namespace PhysicsEngine.Core.Entities
{
    public class PhysicsBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }

        // Add properties for bounding boxes if needed for collision detection

        public void ApplyForce(Vector2 force)
        {
            // Implementation to apply force to this body
        }

        public void Update(float deltaTime)
        {
            // Basic physics update using chosen integration method
        }
    }
}
