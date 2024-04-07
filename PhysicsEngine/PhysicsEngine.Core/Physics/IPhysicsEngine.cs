using System.Collections.Generic;

namespace PhysicsEngine.Core.Physics
{
    public interface IPhysicsEngine
    {
        void AddBody(PhysicsBody body);
        void RemoveBody(PhysicsBody body);
        void ApplyGravity(float gravityScale = 9.8f);
        void Update(float deltaTime);
        List<PhysicsBody> GetAllBodies();
    }
}
