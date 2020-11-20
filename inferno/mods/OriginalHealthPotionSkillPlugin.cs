using SharpDX;
using System;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{

    public class OriginalHealthPotionSkillPlugin : BasePlugin, IInGameTopPainter
    {

        public SkillPainter Decorator { get; set; }
        private bool IsOnFakeCooldown { get; set; } // fixes cooldown timer on potion when first entgering game

        public OriginalHealthPotionSkillPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            if (Enabled)
            {
                Hud.GetPlugin<Turbo.Plugins.Default.OriginalHealthPotionSkillPlugin>().Enabled = false; // this file replaces it
            }

            IsOnFakeCooldown = false;

            Decorator = new SkillPainter(Hud, true)
            {
                TextureOpacity = 0.0f,
                EnableSkillDpsBar = false,
                EnableDetailedDpsHint = false,
            };
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (!Hud.Game.Me.IsInGame)
            {
                IsOnFakeCooldown = true;
                return;
            }

            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;

            var ui = Hud.Render.GetPlayerSkillUiElement(ActionKey.Heal);
            var rect = new RectangleF((float)Math.Round(ui.Rectangle.X) + 0.5f, (float)Math.Round(ui.Rectangle.Y) + 0.5f, (float)Math.Round(ui.Rectangle.Width), (float)Math.Round(ui.Rectangle.Height));
            var skill = Hud.Game.Me.Powers.HealthPotionSkill;

            IsOnFakeCooldown = true;

            if (skill.IsOnCooldown && (skill.CooldownFinishTick > Hud.Game.CurrentGameTick))
            {
                var remaining = (skill.CooldownFinishTick - Hud.Game.CurrentGameTick) / 60.0d;
                if (remaining <= 30.0d)
                {
                    IsOnFakeCooldown = false;
                }
            }

            if (!IsOnFakeCooldown)
            {
                Decorator.Paint(skill, rect);
            }
        }
    }
}
