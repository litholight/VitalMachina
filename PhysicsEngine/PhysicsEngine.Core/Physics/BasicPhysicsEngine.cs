using System.Collections.Generic;
using System.Linq;

namespace PhysicsEngine.Core.Physics
{
    public class BasicPhysicsEngine : IPhysicsEngine
    {
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();
        public List<CollisionResult> CollisionResults { get; private set; } = new List<CollisionResult>();

        public void AddBody(PhysicsBody body) => _bodies.Add(body);
        public void RemoveBody(PhysicsBody body) => _bodies.Remove(body);

        public void ApplyGravity(float gravityScale = 9.8f)
        {
            foreach (var body in _bodies.Where(b => !b.IsResting))
            {
                body.VelocityY += gravityScale * body.Mass;
            }
        }

        public void Update(float deltaTime)
        {
            ApplyGravity();
            DetectCollisions();

            foreach (var body in _bodies.Where(b => !b.IsStatic))
            {
                body.X += body.VelocityX * deltaTime;
                body.Y += body.VelocityY * deltaTime;
            }

            ResolveCollisions();
        }

        private void DetectCollisions()
        {
            CollisionResults.Clear();
            var ground = _bodies.FirstOrDefault(b => b.IsStatic && b.Id == "Ground"); // Assuming "Ground" is the ID for the ground object

            // Handling ground collision first to manage resting state properly
            foreach (var body in _bodies.Where(b => !b.IsStatic))
            {
                if (body.Y + body.Height >= ground.Y)
                {
                    if (!body.IsResting)
                    {
                        body.Y = ground.Y - body.Height; // Adjust Y position to sit on the ground
                        body.VelocityY = 0; // Stop downward movement
                        body.IsResting = true;

                        CollisionResults.Add(new CollisionResult
                        {
                            IsColliding = true,
                            Direction = CollisionDirection.Bottom,
                            OtherBody = ground
                        });
                    }
                }
                else
                {
                    body.IsResting = false;
                }
            }

            // Check collisions between all bodies, skipping unnecessary checks
            for (int i = 0; i < _bodies.Count; i++)
            {
                for (int j = i + 1; j < _bodies.Count; j++)
                {
                    var bodyA = _bodies[i];
                    var bodyB = _bodies[j];

                    if (bodyA == bodyB || (bodyA.IsStatic && bodyB.IsStatic)) continue;

                    var collisionResult = CheckCollision(bodyA, bodyB);
                    if (collisionResult.IsColliding)
                    {
                        CollisionResults.Add(collisionResult);
                    }
                }
            }
        }


        private void ResolveCollisions()
        {
            // Placeholder for collision resolution logic, which should adjust positions and velocities
        }

        private CollisionResult CheckCollision(PhysicsBody bodyA, PhysicsBody bodyB)
        {
            bool isColliding = bodyA.X < bodyB.X + bodyB.Width && bodyA.X + bodyA.Width > bodyB.X &&
                               bodyA.Y < bodyB.Y + bodyB.Height && bodyA.Y + bodyA.Height > bodyB.Y;

            if (!isColliding)
                return new CollisionResult { IsColliding = false };

            // Simple determination of collision direction (improve this as needed)
            CollisionDirection direction = CollisionDirection.None;
            // Example simple logic: determine if A is above B
            if (bodyA.Y + bodyA.Height <= bodyB.Y)
                direction = CollisionDirection.Bottom;

            return new CollisionResult
            {
                IsColliding = true,
                Direction = direction,
                OtherBody = bodyB
            };
        }

        private CollisionDirection DetermineCollisionDirection(PhysicsBody bodyA, PhysicsBody bodyB)
        {
            // Placeholder - detailed logic needed to determine exact direction
            return CollisionDirection.None;
        }
        public List<PhysicsBody> GetAllBodies() => _bodies.ToList();
    }
}
