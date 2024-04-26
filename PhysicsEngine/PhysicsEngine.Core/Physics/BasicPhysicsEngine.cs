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

        public void Update(float deltaTime)
        {
            ApplyGravity(deltaTime); // Apply gravity to all bodies first
            foreach (var body in _bodies)
            {
                body.Update(deltaTime);
            }
            DetectCollisions();
            ResolveCollisions();
        }

        public void ApplyGravity(float deltaTime)
        {
            foreach (var body in _bodies.Where(b => !b.IsResting && !b.IsStatic))
            {
                body.VelocityY += 9.8f * deltaTime;
            }
        }

        private void DetectCollisions()
        {
            CollisionResults.Clear();

            // Check collisions between all bodies, including ground
            for (int i = 0; i < _bodies.Count; i++)
            {
                for (int j = i + 1; j < _bodies.Count; j++)
                {
                    var bodyA = _bodies[i];
                    var bodyB = _bodies[j];

                    // Skip checking between two static bodies
                    if (bodyA.IsStatic && bodyB.IsStatic) continue;

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
            foreach (var result in CollisionResults)
            {
                if (!result.IsColliding) continue;

                // Call the response handler for each body with the corresponding other body
                HandleCollisionResponse(result.Body, result.OtherBody, result.Direction);
                HandleCollisionResponse(result.OtherBody, result.Body, result.OtherBodyDirection);
            }
        }

        private void HandleCollisionResponse(PhysicsBody body, PhysicsBody otherBody, CollisionDirection direction)
        {
            if (body.IsStatic) return; // Static bodies do not respond to forces.

            // If the other body is static, we may want to completely stop the body's movement or apply specific logic
            if (otherBody.IsStatic)
            {
                // Stop all movement if the other body is static
                switch (direction)
                {
                    case CollisionDirection.Top:
                    case CollisionDirection.Bottom:
                        body.VelocityY = 0; // Stop vertical movement
                        if (direction == CollisionDirection.Bottom)
                        {
                            body.IsResting = true; // Set resting state when on a surface
                        }
                        break;
                    case CollisionDirection.Left:
                    case CollisionDirection.Right:
                        body.VelocityX = 0; // Stop horizontal movement
                        break;
                }
            }
            else
            {
                // Handle collisions with other dynamic bodies differently
                // You may want to include some form of elastic collision response here
                // For simplicity, the below example just reduces the velocity by half as a placeholder
                switch (direction)
                {
                    case CollisionDirection.Top:
                    case CollisionDirection.Bottom:
                        body.VelocityY *= 0.5f; // Reduce vertical velocity
                        break;
                    case CollisionDirection.Left:
                    case CollisionDirection.Right:
                        body.VelocityX *= 0.5f; // Reduce horizontal velocity
                        break;
                }
            }
        }

        private CollisionResult CheckCollision(PhysicsBody bodyA, PhysicsBody bodyB)
        {
            bool isColliding = bodyA.X < bodyB.X + bodyB.Width && bodyA.X + bodyA.Width > bodyB.X &&
                               bodyA.Y < bodyB.Y + bodyB.Height && bodyA.Y + bodyA.Height > bodyB.Y;

            if (!isColliding)
                return new CollisionResult { IsColliding = false };

            // Determine collision direction for each body
            CollisionDirection directionA = DetermineCollisionDirection(bodyA, bodyB);
            CollisionDirection directionB = DetermineCollisionDirection(bodyB, bodyA); // Notice the swapped order

            return new CollisionResult
            {
                IsColliding = true,
                Direction = directionA,
                OtherBodyDirection = directionB,
                Body = bodyA,
                OtherBody = bodyB
            };
        }


        private CollisionDirection DetermineCollisionDirection(PhysicsBody bodyA, PhysicsBody bodyB)
        {
            // Determine the center points of each body
            float centerAX = bodyA.X + bodyA.Width / 2;
            float centerAY = bodyA.Y + bodyA.Height / 2;
            float centerBX = bodyB.X + bodyB.Width / 2;
            float centerBY = bodyB.Y + bodyB.Height / 2;

            // Calculate the differences
            float dx = centerBX - centerAX; // Difference in X
            float dy = centerBY - centerAY; // Difference in Y

            // Determine the absolute overlap on each axis
            float overlapX = (bodyA.Width / 2 + bodyB.Width / 2) - Math.Abs(dx);
            float overlapY = (bodyA.Height / 2 + bodyB.Height / 2) - Math.Abs(dy);

            // Use the overlaps to determine the direction
            if (overlapX > overlapY)
            {
                if (dy > 0)
                    return CollisionDirection.Bottom; // Body A is above Body B
                else
                    return CollisionDirection.Top; // Body A is below Body B
            }
            else
            {
                if (dx > 0)
                    return CollisionDirection.Right; // Body A is to the left of Body B
                else
                    return CollisionDirection.Left;
            }
        }

        public List<PhysicsBody> GetAllBodies() => _bodies.ToList();
    }
}
