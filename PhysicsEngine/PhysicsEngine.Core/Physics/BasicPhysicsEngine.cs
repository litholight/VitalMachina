using System.Collections.Generic;
using System.Linq;

namespace PhysicsEngine.Core.Physics
{
    public class BasicPhysicsEngine : IPhysicsEngine
    {
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();

        public void AddBody(PhysicsBody body) => _bodies.Add(body);

        public void RemoveBody(PhysicsBody body) => _bodies.Remove(body);

        public void ApplyGravity(float gravityScale = 9.8f)
        {
            foreach (var body in _bodies)
            {
                body.VelocityY += gravityScale;
            }
        }

        public void Update(float deltaTime)
        {
            ApplyGravity();
            DetectCollisions(); // A new method to detect and resolve collisions

            foreach (var body in _bodies.Where(b => !b.IsStatic))
            {
                body.X += body.VelocityX * deltaTime;
                body.Y += body.VelocityY * deltaTime;
            }
        }

        private void DetectCollisions()
        {
            // Example of a very basic ground collision detection
            foreach (var body in _bodies.Where(b => !b.IsStatic))
            {
                var ground = _bodies.FirstOrDefault(b => b.IsStatic); // Assuming there's only one ground object for simplicity
                if (ground != null && body.Y + body.Height >= ground.Y)
                {
                    body.Y = ground.Y - body.Height; // Adjust Y position to sit on the ground
                    body.VelocityY = 0; // Stop downward movement
                }
            }
        }

        public List<PhysicsBody> GetAllBodies() => _bodies.ToList();
    }
}
