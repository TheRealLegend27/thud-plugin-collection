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
            int startTick = Hud.Game.Me.Powers.SkillSlots[2].CooldownStartTick;
            font.DrawText(startTick + "", 200, 100);
            int finishTick = Hud.Game.Me.Powers.SkillSlots[2].CooldownFinishTick;
            font.DrawText(finishTick + "", 200, 300);
        }
    }
}