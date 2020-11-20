using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{
    public class JawbreakerCircle : BasePlugin, IInGameWorldPainter
    {
		public GroundCircleDecorator JawbreakerDecorator { get; set; }
        public float Thickness { get; private set; }
        public float Size { get; private set; }
        public int Transparency { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
        public JawbreakerCircle()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            Thickness = 3.0f;
            Size = 30f;
            Transparency = 30; //0-255
            Red = 222; //0-255
            Green = 184; //0-255
            Blue = 135; //0-255

            JawbreakerDecorator = new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(Transparency, Red, Green, Blue, Thickness),
                Radius = Size
            };
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.IsInTown) return;
            if (Hud.Render.UiHidden) return;
            var EfficaciousToxin = Hud.Game.Me.Powers.GetBuff(318432);
            if (EfficaciousToxin != null && EfficaciousToxin.Active)
            {
                JawbreakerDecorator.Paint(null, Hud.Game.Me.FloorCoordinate, null);
            }
        }
    }
}