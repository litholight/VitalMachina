using System.Numerics;
using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Input;

namespace Mario.Common.Models
{
    public class Player : GameObject, IInputHandler
    {
        public bool IsDecelerating { get; private set; } = false;
        public PhysicsEngine.Core.Physics.PhysicsBody PhysicsBody { get; set; }
        public PlayerState CurrentState { get; private set; } = PlayerState.StandingRight;
        private const float MaxSpeed = 400.0f; // Max speed in units per second

        internal Player()
            : base()
        {
            Type = GameObjectType.Player;
            Width = 1F;
            Height = .45F;
            Color = Color.Orange;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            ClampVelocity();

            // Check if player should transition to standing state during deceleration
            if (IsDecelerating && Math.Abs(PhysicsBody.VelocityX) < 20.0f)
            {
                PhysicsBody.VelocityX = 0;
                IsDecelerating = false;

                UpdateStandingState();
            }
        }

        private void UpdateStandingState()
        {
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
            PhysicsBody.ApplyForce(new Vector2(-4000, 0)); // Apply a leftward force
            CurrentState = PlayerState.MovingLeft;
            CurrentAnimation = "SmallMovingLeft";
        }

        private void MoveRight()
        {
            PhysicsBody.ApplyForce(new Vector2(4000, 0)); // Apply a rightward force
            CurrentState = PlayerState.MovingRight;
            CurrentAnimation = "SmallMovingRight";
        }

        private void StopMoving()
        {
            IsDecelerating = true;
        }

        private void Jump()
        {
            if (PhysicsBody.IsResting)
            {
                var jumpForce = new Vector2(0, -25000); // Jumping force
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
                        break;
                    case GameAction.MoveRight:
                        MoveRight();
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
