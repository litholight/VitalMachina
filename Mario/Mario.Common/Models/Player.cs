using System.Threading.Tasks;
using Mario.Common.Abstractions;
using Mario.Common.Input;

namespace Mario.Common.Models
{
    public class Player : GameObject, IInputHandler
    {
        public PhysicsEngine.Core.Physics.PhysicsBody PhysicsBody { get; set; }
        public float Speed { get; set; } = 80.0f;
        public PlayerState CurrentState { get; private set; } = PlayerState.StandingRight;

        internal Player()
            : base()
        {
            Type = GameObjectType.Player;
            Width = 66;
            Height = 30;
            Color = Color.Orange;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdateMovement(deltaTime);
            UpdateAnimation();
        }

        private void UpdateMovement(float deltaTime)
        {
            // Adjust position based on state
            switch (CurrentState)
            {
                case PlayerState.MovingRight:
                    X += Speed * deltaTime;
                    break;
                case PlayerState.MovingLeft:
                    X -= Speed * deltaTime;
                    break;
            }
        }

        private void UpdateAnimation()
        {
            // Decide which animation to play based on state
            switch (CurrentState)
            {
                case PlayerState.StandingRight:
                    CurrentAnimation = "SmallFacingRight";
                    break;
                case PlayerState.MovingRight:
                    CurrentAnimation = "SmallMovingRight";
                    break;
                case PlayerState.StandingLeft:
                    CurrentAnimation = "SmallFacingLeft";
                    break;
                case PlayerState.MovingLeft:
                    CurrentAnimation = "SmallMovingLeft";
                    break;
            }
        }

        public void HandleAction(GameAction action, bool isKeyDown)
        {
            if (isKeyDown)
            {
                switch (action)
                {
                    case GameAction.MoveLeft:
                        CurrentState = PlayerState.MovingLeft;
                        PhysicsBody.VelocityX = -Speed;
                        break;
                    case GameAction.MoveRight:
                        CurrentState = PlayerState.MovingRight;
                        PhysicsBody.VelocityX = Speed;
                        break;
                }
            }
            else
            {
                // On key up, transition to standing state if we're currently moving in the direction of the key that was released
                switch (action)
                {
                    case GameAction.MoveLeft when CurrentState == PlayerState.MovingLeft:
                        CurrentState = PlayerState.StandingLeft;
                        PhysicsBody.VelocityX = 0;
                        break;
                    case GameAction.MoveRight when CurrentState == PlayerState.MovingRight:
                        CurrentState = PlayerState.StandingRight;
                        PhysicsBody.VelocityX = 0;
                        break;
                }
            }
        }
    }
}
