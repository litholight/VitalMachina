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
        public bool IsResting { get; set; } = false;

        // Bounding box dimensions for collision detection
        public float Width { get; set; }
        public float Height { get; set; }

        // Vector to accumulate forces applied to the body
        public Vector2 Force { get; set; } = new Vector2(0, 0);

        // Method to apply force to the body
        public void ApplyForce(Vector2 force)
        {
            Force += force;
        }

        // Update the physics state of the body
        public void Update(float deltaTime)
        {
            if (!IsStatic)
            {
                // Apply accumulated force to velocity
                VelocityX += (Force.X / Mass) * deltaTime;
                VelocityY += (Force.Y / Mass) * deltaTime;

                // Move the body based on the new velocity
                X += VelocityX * deltaTime;
                Y += VelocityY * deltaTime;

                // Reset the force accumulator after each update
                Force = new Vector2(0, 0);
            }
        }
    }
}
