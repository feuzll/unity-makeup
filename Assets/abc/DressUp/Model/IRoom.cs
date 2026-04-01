using System.Collections.Generic;

namespace abc.DressUp.Model
{
    public partial interface IRoom : IToolReadyMotionBuilder
    {
        protected Dictionary<ITool, HandToolReadyPlace> AvailableTools { get; }
        protected IFaceZone FaceZone { get; }
        protected IHand Hand { get; }
        protected IBrush.IColorSource PendingBrushColor { get; set; }
   
    }
}