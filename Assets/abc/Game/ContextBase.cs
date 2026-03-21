namespace abc.Game
{
    public abstract partial class ContextBase : IContext
    {
        protected ExecutionToken CreateToken() => new ExecutionToken(this);
        public abstract void Execute();
    }
}