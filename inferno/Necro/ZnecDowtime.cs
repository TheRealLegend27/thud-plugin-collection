using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;
using SharpDX.DirectInput;

using System;
using System.Text;
using System.Collections.Generic;

namespace Turbo.Plugins.inferno
{
    using System.Text;


    public class ZnecDowtime : BasePlugin, IInGameTopPainter
    {
        private readonly int[] _skillOrder = { 2, 3, 4, 5, 0, 1 };
        private StringBuilder textBuilder;
        private IFont GreenFont;
        private IFont YellowFont;
        private IFont RedFont;
        string textFunc() => "ZH";
        double LandTimeLeft;
        double Cooldown;
        double SkillCooldown;
        bool ZnecIngame;
        public ZnecDowtime()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            GreenFont = Hud.Render.CreateFont("tahoma", 8, 255, 0, 255, 0, true, false, false);
            YellowFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 0, true, false, false);
            RedFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 0, 0, true, false, false);
            textBuilder = new StringBuilder();
        }


        public void PaintTopInGame(ClipState clipState)
        {
            if (Hud.Render.UiHidden) return;
            float x = Hud.Window.Size.Width * 0.7f;
            float y = Hud.Window.Size.Height * 0.01f;

			ZnecIngame = false;
            foreach (var player in Hud.Game.Players)//others
            {
                if (player.HeroClassDefinition.HeroClass == HeroClass.Necromancer)
                {
                    var EfficaciousToxin = player.Powers.GetBuff(403461);
                    if (EfficaciousToxin == null || !EfficaciousToxin.Active)
                    {
                        //rat
                    }
                    else
                    {
                        //znec
                        ZnecIngame = true;
                        foreach (var i in _skillOrder)
                        {
                            var skill = player.Powers.SkillSlots[i];
                            if (skill == null || skill.SnoPower.Sno != 465839) continue; //, //Land of the Dead

                            Cooldown = (skill.CooldownFinishTick - Hud.Game.CurrentGameTick) / 60.0d;
                            if (Cooldown < 0) Cooldown = 0;
                            SkillCooldown = skill.CalculateCooldown(120);
                            Cooldown = Cooldown / SkillCooldown;
                            var buff = skill.Buff;
                            if ((buff == null) || (buff.IconCounts[0] <= 0))
                            {
                                LandTimeLeft = 0;
                            }
                            else
                            {
                                LandTimeLeft = (buff.TimeLeftSeconds[0])/10.0d;
                            }

                        }
                    }
                }
            }

            double score = (LandTimeLeft - Cooldown);
			score *= 10;
            textBuilder.Clear();
            textBuilder.AppendFormat("{0:0.00}", score);
            textBuilder.AppendLine();
            textBuilder.AppendFormat("{0:0.00}", LandTimeLeft * 10.0d);
            if (ZnecIngame)
            {
                if (LandTimeLeft == 0)
                {
                    var layout = RedFont.GetTextLayout(textBuilder.ToString());
                    RedFont.DrawText(layout, x, y);
                }
                else if (LandTimeLeft >= Cooldown)
                {
                    var layout = GreenFont.GetTextLayout(textBuilder.ToString());
                    GreenFont.DrawText(layout, x, y);
                }
                else if (LandTimeLeft < Cooldown)
                {
                    var layout = YellowFont.GetTextLayout(textBuilder.ToString());
                    YellowFont.DrawText(layout, x, y);
                }
            }
        }
    }
}