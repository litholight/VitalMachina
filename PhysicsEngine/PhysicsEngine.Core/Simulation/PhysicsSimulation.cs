// PhysicsEngine.Core/Simulation/PhysicsSimulation.cs
namespace PhysicsEngine.Core.Simulation
{
    public class PhysicsSimulation
    {
        private List<PhysicsBody> bodies = new List<PhysicsBody>();
        public Vector2 Gravity { get; set; } = new Vector2(0, -9.8f);

        public void AddBody(PhysicsBody body)
        {
            bodies.Add(body);
        }

        public void Update(float deltaTime)
        {
            foreach (var body in bodies)
            {
                body.ApplyForce(Gravity * body.Mass);
                body.Update(deltaTime);
            }
        }
    }
}
