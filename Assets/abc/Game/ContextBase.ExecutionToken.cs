namespace abc.Game
{
    public abstract partial class ContextBase
    {
        public sealed class ExecutionToken
        {
            public IContext IssuedBy { get; }
            internal ExecutionToken(IContext context) => IssuedBy = context;
        }
    }
}