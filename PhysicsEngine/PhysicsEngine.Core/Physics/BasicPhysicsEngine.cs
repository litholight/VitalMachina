using System.Collections.Generic;
using System.Linq;

namespace PhysicsEngine.Core.Physics
{
    public class BasicPhysicsEngine : IPhysicsEngine
    {
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();

        public void AddBody(PhysicsBody body) => _bodies.Add(body);

        public void RemoveBody(PhysicsBody body) => _bodies.Remove(body);

        public void ApplyGravity(float gravityScale = 0.8f)
        {
            foreach (var body in _bodies)
            {
                body.VelocityY += gravityScale;
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var body in _bodies)
            {
                body.X += body.VelocityX * deltaTime;
                body.Y += body.VelocityY * deltaTime;
            }
        }

        public List<PhysicsBody> GetAllBodies() => _bodies.ToList();
    }
}
