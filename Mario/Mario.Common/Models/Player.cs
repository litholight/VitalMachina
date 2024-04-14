using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Input;
using System.Numerics;

namespace Mario.Common.Models
{
    public class Player : GameObject, IInputHandler
    {
        public PhysicsEngine.Core.Physics.PhysicsBody PhysicsBody { get; set; }
        public PlayerState CurrentState { get; private set; } = PlayerState.StandingRight;
        private const float MaxSpeed = 400.0f;  // Max speed in units per second

        internal Player() : base()
        {
            Type = GameObjectType.Player;
            Width = 66;
            Height = 30;
            Color = Color.Orange;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            // Clamp the velocity to the maximum speed
            if (PhysicsBody.VelocityX > MaxSpeed)
            {
                PhysicsBody.VelocityX = MaxSpeed;
            }
            else if (PhysicsBody.VelocityX < -MaxSpeed)
            {
                PhysicsBody.VelocityX = -MaxSpeed;
            }
        }

        private void MoveLeft()
        {
            CurrentState = PlayerState.MovingLeft;
            PhysicsBody.ApplyForce(new Vector2(-4000, 0));  // Apply a leftward force
        }

        private void MoveRight()
        {
            CurrentState = PlayerState.MovingRight;
            PhysicsBody.ApplyForce(new Vector2(4000, 0));  // Apply a rightward force
        }

        private void StopMoving()
        {
            // Apply a deceleration force that is proportional to the negative of the current velocity
            PhysicsBody.ApplyForce(new Vector2(-PhysicsBody.VelocityX * 0.5f, 0));

            // Check if the velocity has reduced to near zero and update the state and animation if it has
            if (Math.Abs(PhysicsBody.VelocityX) < 50.0f)  // 1.0f can be adjusted based on what you consider as "stopped"
            {
                PhysicsBody.VelocityX = 0;  // Set velocity to zero to stop completely

                // Update state and animation based on the previous direction
                if (CurrentState == PlayerState.MovingLeft)
                {
                    CurrentState = PlayerState.StandingLeft;
                    CurrentAnimation = "SmallFacingLeft";
                }
                else if (CurrentState == PlayerState.MovingRight)
                {
                    CurrentState = PlayerState.StandingRight;
                    CurrentAnimation = "SmallFacingRight";
                }
            }
        }

        private void Jump()
        {
            if (PhysicsBody.IsResting)
            {
                var jumpForce = new Vector2(0, -25000);  // Jumping force
                PhysicsBody.ApplyForce(jumpForce);
                PhysicsBody.IsResting = false;
            }
        }
        public void HandleAction(GameAction action, bool isKeyDown)
        {
            if (isKeyDown)
            {
                switch (action)
                {
                    case GameAction.MoveLeft:
                        MoveLeft();
                        CurrentAnimation = "SmallMovingLeft";
                        break;
                    case GameAction.MoveRight:
                        MoveRight();
                        CurrentAnimation = "SmallMovingRight";
                        break;
                    case GameAction.Jump:
                        Jump();
                        break;
                }
            }
            else
            {
                switch (action)
                {
                    case GameAction.MoveLeft:
                    case GameAction.MoveRight:
                        StopMoving();
                        break;
                }
            }
        }
    }
}
