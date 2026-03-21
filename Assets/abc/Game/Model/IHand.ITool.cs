namespace abc.Game.Model
{
    public partial interface IHand
    {
        public interface ITool
        {
            public (float x, float y) RestPosition { get; }
            public (float x, float y) Position { get; }

            public void PerformJob();

            public void BindTo(IHand hand);
            public void BindTo(IContainer container);

            public void Unbind()
            {
            }

            public interface IContainer
            {
            }
        }
    }
}