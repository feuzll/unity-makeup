namespace abc.Game.Model
{
    public interface IHand
    {
        protected void RequestMoveTo(float x, float y);

        public interface IUser
        {
            protected IHand Target { get; }
            public void TryMoveTo(float x, float y) => Target.RequestMoveTo(x, y);
        }
    }
}