namespace PhysicsEngine.Core.Physics
{
    public class CollisionResult
    {
        public bool IsColliding { get; set; }
        public CollisionDirection Direction { get; set; } // Direction of collision from the perspective of the first body
        public CollisionDirection OtherBodyDirection { get; set; } // Direction of collision from the perspective of the second body
        public PhysicsBody Body { get; set; } // The first body involved in the collision
        public PhysicsBody OtherBody { get; set; } // The second body involved in the collision
    }
}
