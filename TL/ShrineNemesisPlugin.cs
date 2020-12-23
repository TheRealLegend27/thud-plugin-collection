using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.TL
{
    public class ShrineNemesisPlugin : BasePlugin, ICustomizer, IInGameWorldPainter
    {
        public WorldDecoratorCollection DecoratorGreen { get; set; }
        public WorldDecoratorCollection DecoratorRed { get; set; }
        public WorldDecoratorCollection DecoratorOrange { get; set; }

        public ShrineNemesisPlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);

          DecoratorGreen = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 16, 255, 0, 255, 0, true, true, true),
          });

          DecoratorRed = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 16, 255, 255, 0, 0, true, true, true),
          });

          DecoratorOrange = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 16, 255, 255, 255, 0, true, true, true),
          });
        }

        public void Customize()
        {
            Hud.TogglePlugin<ShrinePlugin>(false);
        }

        public void PaintWorld(WorldLayer layer)
        {
            List<string> missingPlayers = new List<string>();
            List<string> nemesisPlayers = new List<string>();
            bool nemesisEquiped = false;
            bool nemesisInGroup = false;
            bool missingPlayer = false;
            
            foreach (var player in Hud.Game.Players.OrderBy(p => p.PortraitIndex))
            {
                if (player == null) continue;

                if (!player.IsMe && player.IsInGame)
                    if (player.IsDead || player.SnoArea != Hud.Game.Me.SnoArea || !player.CoordinateKnown)
                        missingPlayers.Add(player.BattleTagAbovePortrait);

                var nemesisBuff = player.Powers.GetBuff(318820);

                if (nemesisBuff == null || !nemesisBuff.Active) {} 
                else
                    if (player.IsMe) nemesisEquiped = true;
                    else nemesisPlayers.Add(player.BattleTagAbovePortrait);
            }

            nemesisInGroup = nemesisPlayers.Count > 0;
            missingPlayer = missingPlayers.Count > 0;

            string message = string.Empty;
            WorldDecoratorCollection dec = null;

            if (nemesisEquiped) {
                if (missingPlayer) {
                    message = "Wait for:\n" + string.Join("\n", missingPlayers.ToArray());
                    dec = DecoratorOrange;
                }
                else {
                    message = "Take";
                    dec = DecoratorGreen;
                }
            }
            else {
                dec = DecoratorRed;
                if (nemesisInGroup) {
                    message = "Leave for:\n" + string.Join("\n", nemesisPlayers.ToArray());
                }
                else {
                    message = "No nemesis :(";
                }
            }


            var shrines = Hud.Game.Shrines.Where(x => !x.IsDisabled && !x.IsOperated);
            foreach (var shrine in shrines)
            {
                if (shrine.Type == ShrineType.HealingWell) continue;
                if (shrine.Type == ShrineType.PoolOfReflection) continue;
                
                if(shrine.FloorCoordinate.Offset(0, 0, 10).IsOnScreen()) dec.Paint(layer, null, shrine.FloorCoordinate.Offset(0, 0, 10), message);
            }

        }
    }
}