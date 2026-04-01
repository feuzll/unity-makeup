using System;

namespace abc.DressUp.Entities
{
    public partial interface IBrush
    {
        public interface IPage
        {
            public void RaiseToolInteract(IBrush brush) => brush.RaiseOnInteract();
        }
    }
}