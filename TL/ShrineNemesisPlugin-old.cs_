using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.TL
{
    public class ShrineNemesisPlugin : BasePlugin, ICustomizer, IInGameWorldPainter
    {
        public WorldDecoratorCollection GoodMessageDec { get; set; }
        public WorldDecoratorCollection BadMessageDec { get; set; }
        public WorldDecoratorCollection MedMessageDec { get; set; }

        public ShrineNemesisPlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);

          GoodMessageDec = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 16, 255, 0, 255, 0, true, true, true),
          });

          BadMessageDec = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 16, 255, 255, 0, 0, true, true, true),
          });

          MedMessageDec = new WorldDecoratorCollection(
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
            string NemesisMessage = string.Empty;

            bool nemsInGroup = false;
            bool take = false;
            bool save = false;
            List<string> missing = new List<string>();

            foreach (var player in Hud.Game.Players.OrderBy(p => p.PortraitIndex))
            {
                if (player == null) continue;

                if (!player.IsMe && player.IsInGame) {
                    if (player.IsDead || player.SnoArea != Hud.Game.Me.SnoArea) {
                        missing.Add(player.BattleTagAbovePortrait);
                    }
                }

                var Nemo = player.Powers.GetBuff(318820);

                if (Nemo == null || !Nemo.Active) {} 
                else
                {
                    if (player.IsMe) take = true;
                    else
                    {
                        nemsInGroup = true;
                        if (NemesisMessage == string.Empty) NemesisMessage += Environment.NewLine + player.BattleTagAbovePortrait;
                        else NemesisMessage += Environment.NewLine + " or " + player.BattleTagAbovePortrait;
                    }
                }
            }
            
            if (take && missing.Count > 0) {
                NemesisMessage = "Wait for";
                foreach (string missingName in missing)
                {
                    NemesisMessage += Environment.NewLine + missingName;
                }
                take = false;
            }
            else if (take) NemesisMessage = "HIT ME!";
            else if (nemsInGroup) NemesisMessage = "Leave for" + NemesisMessage;
            else NemesisMessage = "No nems :(";

            var GriftBar = Hud.Render.GreaterRiftBarUiElement;
            var RiftPercentage = Hud.Game.RiftPercentage;

            if (GriftBar.Visible && RiftPercentage > 95 && take)
             {
                    save = true;
                    NemesisMessage = "Keep for boss!";
             }

             var decorator = MedMessageDec;
             if (take) decorator = GoodMessageDec;
             else if (nemsInGroup || save) decorator = BadMessageDec;

             var shrines = Hud.Game.Shrines.Where(x => !x.IsDisabled && !x.IsOperated);
            foreach (var shrine in shrines)
            {
                if (shrine.Type == ShrineType.HealingWell) continue;
                if (shrine.Type == ShrineType.PoolOfReflection) continue;
                
                if(shrine.FloorCoordinate.Offset(0, 0, 10).IsOnScreen()) decorator.Paint(layer, null, shrine.FloorCoordinate.Offset(0, 0, 10), NemesisMessage);
            }
        }
    }
}