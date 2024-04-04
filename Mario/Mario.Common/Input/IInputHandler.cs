namespace Mario.Common.Input
{
    public interface IInputHandler
    {
        void HandleAction(GameAction action, bool isKeyDown);
    }
}
