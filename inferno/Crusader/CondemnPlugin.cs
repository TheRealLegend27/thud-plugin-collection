using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno
{
    public class CondemnPlugin : BasePlugin, IInGameWorldPainter
    {
        public GroundCircleDecorator CondemnDecorator { get; set; }
        public float Thickness { get; private set; }
        public float Size { get; private set; }
        public int Transparency { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
        public CondemnPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
 
            Thickness = 3.0f;
            Size = 15f;
            Transparency = 100; //0-255
            Red = 106; //0-255
            Green = 162; //0-255
            Blue = 252; //0-255
 
            CondemnDecorator = new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(Transparency, Red, Green, Blue, Thickness),
                Radius = Size
            };
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.IsInTown) return;
            var skill = Hud.Game.Me.Powers.UsedSkills.Where(x => x.SnoPower.Sno == 266627).FirstOrDefault();
            if (skill == null) return;
            if (Hud.Render.UiHidden) return;
            var skillRune = Hud.Game.Me.Powers.UsedCrusaderPowers.Condemn.RuneNameEnglish;
            if (string.Equals(skillRune, "Vacuum"))
          
            {
                CondemnDecorator.Paint(null, Hud.Game.Me.FloorCoordinate, null);
            }
        }
    }
}

