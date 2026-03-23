using System.Drawing;
using abc.Game.Model;
using Brush = abc.Game.Model.Brush;

namespace abc.Game.Contexts
{
    public class ColorBrushContext : ContextBase
    {
        private readonly Brush _brush;
        private readonly BrushColor     _color;

        public ColorBrushContext(Brush brush, BrushColor color)
        {
            _brush = brush;
            _color = color;
        }

        public override void Execute()
        {
            var role = new Brush.ColorRole(_brush, CreateToken());
            role.SetColor(_color);
        }        
    }
}