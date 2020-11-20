using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.TL
{
    public class TestPlugin : BasePlugin, IInGameWorldPainter
    {
        public IFont font;

        public TestPlugin()
        {
            Enabled = false;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            
            font = Hud.Render.CreateFont("tahoma", 16f, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
        }

        public void PaintWorld(WorldLayer layer)
        {
            SpecialArea finishTick = Hud.Game.SpecialArea;
            font.DrawText(finishTick + "", 200, 100);
        }
    }
}