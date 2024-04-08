namespace PhysicsEngine.Core.Physics
{
    public class CollisionResult
    {
        public bool IsColliding { get; set; }
        public CollisionDirection Direction { get; set; }
        public PhysicsBody OtherBody { get; set; }
    }
}
