using System.Numerics;

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
        public bool IsResting { get; set; } = false; // Only apply friction if resting
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector2 Force { get; set; } = new Vector2(0, 0);
        public float FrictionCoefficient { get; set; } = 0.5f; // Friction coefficient

        public void ApplyForce(Vector2 force)
        {
            Force += force;
        }

        public void Update(float deltaTime)
        {
            if (!IsStatic)
            {
                // Apply accumulated force to velocity
                VelocityX += (Force.X / Mass) * deltaTime;
                VelocityY += (Force.Y / Mass) * deltaTime;

                // Apply friction if the body is resting on a surface (e.g., ground)
                if (IsResting)
                {
                    ApplyFriction(deltaTime);
                }

                // Move the body based on the new velocity
                X += VelocityX * deltaTime;
                Y += VelocityY * deltaTime;

                // Reset the force accumulator after each update
                Force = new Vector2(0, 0);
            }
        }

        private void ApplyFriction(float deltaTime)
        {
            // Apply friction force, which is proportional and opposite to the velocity
            Vector2 frictionForce = -FrictionCoefficient * new Vector2(VelocityX, VelocityY);
            VelocityX += frictionForce.X * deltaTime;
            VelocityY += frictionForce.Y * deltaTime;
        }
    }
}
