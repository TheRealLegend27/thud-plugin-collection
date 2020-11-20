using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;
using System;
 
namespace Turbo.Plugins.inferno
{
    public class ChantodoStacksPlugin : BasePlugin, IInGameTopPainter
    {
         public TopLabelWithTitleDecorator ChantodoDecorator { get; set; }
        public bool ShowInTown { get; set; }
        public IBrush GreenBrush { get; set; }
        public IBrush OrangeBrush { get; set; }
        public IBrush RedBrush { get; set; }
        private float w  { get; set; }
        private float h  { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        private IFont currentFont;
        public ChantodoStacksPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);

            GreenBrush = Hud.Render.CreateBrush(160, 0, 255, 0, 0);
            OrangeBrush = Hud.Render.CreateBrush(160, 255, 165, 0, 0);
            RedBrush = Hud.Render.CreateBrush(160, 255, 0, 0, 0);

            ShowInTown = false;
            w = Hud.Window.Size.Width * 0.03f;
            h = Hud.Window.Size.Height * 0.02f;
            XPos = Hud.Window.Size.Width * 0.5f - w/2;
            YPos = Hud.Window.Size.Height * 0.5f + Hud.Window.Size.Height * 0.00001f;

            ChantodoDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                BackgroundBrush = GreenBrush,
                BorderBrush = Hud.Render.CreateBrush(255, 0, 0, 0, -1),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 0, 0, 0, true, false, false),
            };

        }
 
        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            if (Hud.Render.UiHidden) return;
           

            IBuff chantodo = null;
            if (Hud.Game.IsInTown && ShowInTown == false) return;

            if (!Hud.Game.Me.Powers.BuffIsActive(440235, 0)) return;
                       
                chantodo = Hud.Game.Me.Powers.GetBuff(440235);                     
                if (chantodo != null)               
                {
                    int stacks = chantodo.IconCounts[0];
              
                    if (stacks < 10)
                    {
                        ChantodoDecorator.BackgroundBrush = RedBrush;
                        ChantodoDecorator.Paint(XPos, YPos, w, h, stacks.ToString());
                    }
                    else if (stacks >= 10 && stacks < 18)
                    {
                        ChantodoDecorator.BackgroundBrush = OrangeBrush;
                        ChantodoDecorator.Paint(XPos, YPos, w, h, stacks.ToString());
                    }
                    else 
                    {
                        ChantodoDecorator.BackgroundBrush = GreenBrush;
                        ChantodoDecorator.Paint(XPos, YPos, w, h, stacks.ToString());
                    }
            }
        }
    }
}