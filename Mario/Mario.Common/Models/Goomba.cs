namespace Mario.Common.Models
{
    public class Goomba : GameObject
    {
        public Goomba()
            : base()
        {
            Type = GameObjectType.Goomba;
            Width = .76F;
            Height = .76F;
            Color = Color.Orange;
        }
    }
}
