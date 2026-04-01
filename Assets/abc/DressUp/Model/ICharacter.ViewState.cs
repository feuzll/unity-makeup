using System;

namespace abc.DressUp.Model
{
    public partial interface ICharacter
    {
        [Serializable]
        public class ViewState
        {
            public enum Type
            {
                Lips, Eyes, Blush, Acne
            }

            public Type type;
            public int value;
        }
        
    }
}