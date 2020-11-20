using Turbo.Plugins.Default;  
using System;
using System.Linq;    
using System.Collections.Generic;
using System.Threading;

namespace Turbo.Plugins.RNN
{        
	public class TrialsOfTempestsIcon : BasePlugin, IInGameTopPainter, ICustomizer
	{		
		private uint[] Textures { get; set; } = new uint[7] { 3405442230, 2639019912, 4122635698, 1541236666, 3513867492, 928374825, 569405584};  // 0,1, 2-Fuego, 3-Frío, 4-Veneno, 5-Físico, 6-Rayos
		private IBrush[] Brushes { get; set; }
		private ITexture TextureBG { get; set; }	
		private long[] timers { get; set; } = new long[] {0,0,0,0};
		private float SizeIconWidth  { get; set; } 
		private float SizeIconHeight  { get; set; }
		
		private SharpDX.DirectWrite.TextLayout layout { get; set; } = null;	 
		private IFont FontStacks0 { get; set; }
		private IFont FontStacks1 { get; set; }
		private IFont FontStacksY { get; set; }
		private IFont FontStacksG { get; set; }
		private IFont FontCounter { get; set; }
		private IFont FontTimeLeft { get; set; }
		private IBrush BrushBorder { get; set; }
		
		public bool PortraitMe { get; set; }
		public bool PortraitOthers { get; set; }
		public float OffsetX  { get; set; }
		public float OffsetY  { get; set; } 
		public float OffsetXPortrait  { get; set; }
		public float OffsetYPortrait  { get; set; } 

		public float Opacity { get; set; } 		
		public float SizeMultiplier  { get; set; }
		
		public bool OnlyGR { get; set; }
		public bool OnlyMe { get; set; }
		public bool VisibleFar { get; set; }

		public bool SoundEnabled { get; set; }
		public string FileSound { get; set; }
		
		public TrialsOfTempestsIcon()
		{
			Enabled = true;    
		}
			
		public override void Load(IController hud)
		{
			base.Load(hud);	
			Order = 30001;

			OnlyGR = false;
			OnlyMe = false;
			VisibleFar = true;		// Keep showing the icon of the players who are far away: the data will freeze, at this distance the values ​​are not updated
			
			PortraitMe = true;		// Show icon near portrait or next to our character (for Me)
			PortraitOthers = true;	// Show icon near portrait or next to our character (for Others Players)
			OffsetX	 = +0.020f;		// To modify the position of the icon. Reference point: character	
			OffsetY	 = -0.020f;
			OffsetXPortrait	 = -0.010f;	// To modify the position of the icon. Reference point: portrait	
			OffsetYPortrait	 = +0.042f;
			
			Opacity = 0.85f;
			SizeMultiplier = 0.75f;

			SoundEnabled = false;
			FileSound = "notification_5.wav";  // File to be played. It must be in the Sounds\ folder
			
			TextureBG = Hud.Texture.GetTexture(3144819863);

			var tr = 1.0f;
			BrushBorder = Hud.Render.CreateBrush( 255, 255, 255, 255, tr);
			Brushes = new IBrush[7] // 0,1, 2-Fuego, 3-Frío, 4-Veneno, 5-Físico, 6-Rayos			
			{ 
				BrushBorder,
				BrushBorder,
				Hud.Render.CreateBrush( 255, 255, 0, 0, tr),
				BrushBorder,
				Hud.Render.CreateBrush( 255, 0, 255, 0, tr),
				Hud.Render.CreateBrush( 255, 80, 80, 80, tr),
				Hud.Render.CreateBrush( 255, 0, 128, 255, tr),						
			};
		}
		
		public void Customize()
		{
			FontStacks1 = Hud.Render.CreateFont("tahoma", 8f * SizeMultiplier, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
			FontStacksY = Hud.Render.CreateFont("tahoma", 8f * SizeMultiplier, 255, 255, 255, 0, false, false, 255, 0, 0, 0, true);
			FontStacksG = Hud.Render.CreateFont("tahoma", 8f * SizeMultiplier, 255, 195, 195, 195, false, false, 255, 0, 0, 0, true);
			FontCounter = Hud.Render.CreateFont("tahoma", 9f * SizeMultiplier, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
			FontTimeLeft = Hud.Render.CreateFont("tahoma", 8f * SizeMultiplier, 255, 0, 255, 0, false, false, 255, 0, 0, 0, true);	
			
			SizeIconWidth = TextureBG.Width   * Hud.Window.Size.Height/1200.0f * SizeMultiplier * 0.90f;
			SizeIconHeight = TextureBG.Height * Hud.Window.Size.Height/1200.0f * SizeMultiplier;  
		}

        public void Play_Sound(string Sonido)
        {
			var playSonido = Hud.Sound.LoadSoundPlayer(Sonido);            
			ThreadPool.QueueUserWorkItem(state =>
			{
				try  { playSonido.PlaySync(); }
				catch (Exception)  {   } 
			}	);
		}
		
		public void PaintTopInGame(ClipState clipState)
		{
			if (clipState != ClipState.BeforeClip) return;
			if (!Hud.Game.IsInGame) return;
			if (OnlyGR && !Hud.Game.Me.InGreaterRift) return;
			if (Hud.Game.Me.Powers.BuffIsActive(484426))  // Jugando en temporada
			{
				var players = Hud.Game.Players.Where(p => p.IsMe || (!OnlyMe && (p.HasValidActor || VisibleFar)));
				foreach(var player in players)
				{
					var buff = player.Powers.GetBuff(484426);
					if (buff != null)
					{
						float x, y;
						if ( player.IsMe?PortraitMe:PortraitOthers )
						{
							x = player.PortraitUiElement.Rectangle.X + Hud.Window.Size.Width * OffsetXPortrait;
							y = player.PortraitUiElement.Rectangle.Y + Hud.Window.Size.Height * OffsetYPortrait;
						}
						else
						{
							x = player.FloorCoordinate.ToScreenCoordinate().X + Hud.Window.Size.Width * OffsetX;
							y = player.FloorCoordinate.ToScreenCoordinate().Y + Hud.Window.Size.Height * OffsetY;							
						}
						
						TextureBG.Draw(x, y, SizeIconWidth, SizeIconHeight, Opacity);
						(player.HasValidActor?Hud.Texture.BuffFrameTexture:Hud.Texture.DebuffFrameTexture).Draw(x, y, SizeIconWidth, SizeIconHeight, Opacity);
						
						var t = buff.TimeLeftSeconds[1];	//layout = FontCounter.GetTextLayout(t.ToString( (t < 1)? "F1" : "F0") );
						if (t < 1.0f) 
						{ 
							layout = FontCounter.GetTextLayout(String.Format("{0:N1}", t)); 	
						}
						else 
						{
							layout = FontCounter.GetTextLayout((t < 60)? String.Format("{0:0}", t ) : String.Format("{0:0}:{1:00}", (int) (t/60),  t%60 )) ;
						}		
						FontCounter.DrawText(layout, x + ((SizeIconWidth - (float)Math.Ceiling(layout.Metrics.Width))/2.0f), y + ((SizeIconHeight - (float)Math.Ceiling(layout.Metrics.Height))/2.0f) );

						layout = FontStacks1.GetTextLayout(buff.IconCounts[8].ToString());						
						FontStacks1.DrawText(layout, x + ((SizeIconWidth - (float)Math.Ceiling(layout.Metrics.Width))/1.17f), y + ((SizeIconHeight - (float)Math.Ceiling(layout.Metrics.Height))/1.1f) );

						FontStacks0 = FontStacksG;						
						for(int i = 2; i < 7; i++ )
						{
							t = buff.TimeLeftSeconds[i];
							if (t > 0)
							{							
								layout = FontTimeLeft.GetTextLayout(t.ToString( (t < 1)? "F1" : "F0") );
								FontTimeLeft.DrawText(layout, x + ((SizeIconWidth - (float)Math.Ceiling(layout.Metrics.Width))/7.0f), y + ((SizeIconHeight - (float)Math.Ceiling(layout.Metrics.Height))/1.1f) );
								Hud.Texture.GetTexture(Textures[i]).Draw(x + 0.08f * SizeIconWidth, y + 0.08f * SizeIconHeight, SizeIconHeight * 0.28f, SizeIconHeight * 0.28f, 1.0f);
								Brushes[i].DrawRectangle(x + 0.08f * SizeIconWidth, y + 0.08f * SizeIconHeight, SizeIconHeight * 0.28f, SizeIconHeight * 0.28f);
								FontStacks0 = FontStacksY;
								if ( SoundEnabled && (buff.TimeElapsedSeconds[i] < 1) )
								{
									if (Hud.Game.CurrentRealTimeMilliseconds > timers[player.Index])	// if ((Hud.Game.CurrentRealTimeMilliseconds - timers[player.Index]) > 2000) 
									{
										timers[player.Index] = (long) t * 1000 + Hud.Game.CurrentRealTimeMilliseconds; // timers[player.Index] = Hud.Game.CurrentRealTimeMilliseconds;
										// Play_Sound(FileSound);
									}									
								}								
								break;
							}
						}
						layout = FontStacks0.GetTextLayout(buff.IconCounts[9].ToString());						
						FontStacks0.DrawText(layout, x + ((SizeIconWidth - (float)Math.Ceiling(layout.Metrics.Width))/1.17f), y + ((SizeIconHeight - (float)Math.Ceiling(layout.Metrics.Height))/12.0f) );
					}					
				}
			}		
		}			
	}
}