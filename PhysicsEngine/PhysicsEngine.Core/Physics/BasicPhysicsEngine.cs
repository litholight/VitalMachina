using System.Collections.Generic;
using System.Linq;

namespace PhysicsEngine.Core.Physics
{
    public class BasicPhysicsEngine : IPhysicsEngine
    {
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();
        public List<CollisionResult> CollisionResults { get; private set; } =
            new List<CollisionResult>();

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
            // Handle ground collisions as before
            // Assuming there's only one ground object for simplicity
            var ground = _bodies.FirstOrDefault(b => b.IsStatic);
            if (ground != null)
            {
                foreach (var body in _bodies.Where(b => !b.IsStatic))
                {
                    if (body.Y + body.Height >= ground.Y)
                    {
                        body.Y = ground.Y - body.Height; // Adjust Y position to sit on the ground
                        body.VelocityY = 0; // Stop downward movement

                        var groundCollision = new CollisionResult
                        {
                            IsColliding = true,
                            Direction = CollisionDirection.Bottom,
                            OtherBody = ground
                        };
                        CollisionResults.Add(groundCollision);
                    }
                }
            }

            // Detect collisions between dynamic bodies
            for (int i = 0; i < _bodies.Count; i++)
            {
                for (int j = i + 1; j < _bodies.Count; j++)
                {
                    var bodyA = _bodies[i];
                    var bodyB = _bodies[j];

                    // Skip if either body is static or if both bodies are the same
                    if (bodyA.IsStatic || bodyB.IsStatic || bodyA == bodyB)
                        continue;

                    var collisionResult = CheckCollision(bodyA, bodyB);
                    if (collisionResult.IsColliding)
                    {
                        CollisionResults.Add(collisionResult);
                    }
                }
            }
        }

        private CollisionResult CheckCollision(PhysicsBody bodyA, PhysicsBody bodyB)
        {
            // Simple AABB (Axis-Aligned Bounding Box) collision detection
            bool isColliding =
                bodyA.X < bodyB.X + bodyB.Width
                && bodyA.X + bodyA.Width > bodyB.X
                && bodyA.Y < bodyB.Y + bodyB.Height
                && bodyA.Y + bodyA.Height > bodyB.Y;

            if (!isColliding)
            {
                return new CollisionResult { IsColliding = false };
            }

            // For simplicity, this example does not calculate the precise collision direction
            // Implementing precise collision direction involves comparing the relative positions
            // and velocities of the bodies to determine the most likely side of impact
            return new CollisionResult
            {
                IsColliding = true,
                Direction = CollisionDirection.None, // Placeholder, consider implementing detailed direction logic
                OtherBody = bodyB
            };
        }

        public List<PhysicsBody> GetAllBodies() => _bodies.ToList();
    }
}
