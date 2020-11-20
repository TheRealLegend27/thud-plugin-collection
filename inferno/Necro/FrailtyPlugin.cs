using SharpDX.DirectInput;
using System.Linq;
using System;
using System.Collections.Generic;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno
{
	using System.Text;
    public class FrailtyPlugin : BasePlugin, IInGameWorldPainter, IInGameTopPainter, ITransparentCollection
    {
        
        public float Thickness { get; private set; }
        public float Size { get; private set; }
        public int Transparency { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
       
		public IBrush FillBrush { get; set; }
        public IBrush OutlineBrush { get; set; }
        public IFader Fader { get; set; }
		
	    public float XWidth { get; set; }
        public float YHeight { get; set; }
		public IFont DefaultTextFont { get; set; }
		public float TotalRadius { get; set; }
		
		private StringBuilder textBuilder;
		public FrailtyPlugin()
        {
            Enabled = true;
        }
		
		public IEnumerable<ITransparent> GetTransparents()
        {
            yield return FillBrush;
            yield return OutlineBrush;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
			
			XWidth = 0.84f;
            YHeight = 0.63f;
            textBuilder = new StringBuilder();
						
			FillBrush = Hud.Render.CreateBrush(255, 255, 255, 64, 2);
            OutlineBrush = Hud.Render.CreateBrush(255, 255, 255, 64, 2);
            Fader = new StandardFader(hud, this);
        }
 
			public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
			
			float XPos = Hud.Window.Size.Width * XWidth;
            float YPos = Hud.Window.Size.Height * YHeight;
			TotalRadius = (float)Math.Floor((Hud.Game.Me.Stats.PickupRange/2) + 15);
			
			//###################################
			// Code below just for debugging
			//###################################
			
			//textBuilder.Clear();
			//textBuilder.AppendFormat("Aura of Frailty Range: {0} yards", TotalRadius.ToString("f2"));
			//textBuilder.AppendLine();
            //currentFont = DefaultTextFont;
			//var layout = currentFont.GetTextLayout(textBuilder.ToString());
            //currentFont.DrawText(layout, XPos, YPos);
        }
 
        public void PaintWorld(WorldLayer layer)
        {
			var visible = !Hud.Game.IsInTown && (Hud.Game.MapMode == MapMode.Minimap);
            if (!Fader.TestVisiblity(visible)) return;
			        			
            var skill = Hud.Game.Me.Powers.UsedSkills.Where(x => x.SnoPower.Sno == Hud.Sno.SnoPowers.Necromancer_Frailty.Sno).FirstOrDefault();
            if (skill == null) return;
            if (Hud.Render.UiHidden) return;
            var skillRune = Hud.Game.Me.Powers.UsedNecromancerPowers.Frailty.RuneNameEnglish;
            if (string.Equals(skillRune, "Aura of Frailty"))
           {
            OutlineBrush.DrawWorldEllipse(TotalRadius, -1, Hud.Game.Me.FloorCoordinate);
            FillBrush.DrawWorldEllipse(TotalRadius, -1, Hud.Game.Me.FloorCoordinate);
            }
        }
    }
}

