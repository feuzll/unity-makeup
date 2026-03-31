using UnityEngine;

namespace abc.DressUp.Interactions
{
    public abstract class Interaction
    {
        protected ExecutionToken Token { get; set; }

        protected Interaction()
        {
            Token = new ExecutionToken(this);
        }
        
        public sealed class ExecutionToken
        {
            public Interaction IssuedBy { get; }
            internal ExecutionToken(Interaction interaction) =>  
                IssuedBy = interaction;
        }
        
    }
}