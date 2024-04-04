namespace Mario.Common.Input
{
    public static class InputTranslator
    {
        public static GameAction? TranslateKeyToGameAction(string key)
        {
            switch (key)
            {
                case "ArrowUp":
                    return GameAction.MoveUp;
                case "ArrowDown":
                    return GameAction.MoveDown;
                case "ArrowLeft":
                    return GameAction.MoveLeft;
                case "ArrowRight":
                    return GameAction.MoveRight;
                default:
                    return null;
            }
        }
    }
}
