//Last Updated: April 24, 2019
//To Do: add proc history tracking
//To Do: add TTS queuing (so if multiple party members proc, they are spoken one after another)

using Turbo.Plugins.Default;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace Turbo.Plugins.inferno
{
	public class ProcInfo {
		public string PlayerName { get; set; }
		public uint HeroId { get; set; }
		public HeroClass PlayerClass { get; set; }
		public ITexture Texture { get; set; }
		
		public int Count { get; set; } = 1; //not used yet, this stores the number of procs seen in a session
		public int StartTick { get; set; }
		public int FinishTick { get; set; } //gametick / 60 = seconds
		public double Duration { get; set; }
		public int SoundPlayedTick { get; set; }
		
		public ProcInfo() {}
	}

	public class PartyProcTracker : BasePlugin, IInGameTopPainter/*IInGameWorldPainter*/, IAfterCollectHandler, INewAreaHandler
    {
		public IFont TimeLeftFont { get; set; }
		public IFont PlayerFont { get; set; }
		public Dictionary<uint, ProcInfo> Snapshots;
		
		public bool ShowTracker { get; set; } 
		
		public float PositionX { get; set; } = -1f; //horizontal position on the screen where the proc icon is drawn (-1 = use default value)
		public float PositionY { get; set; } = -1f; //vertical position on the screen where the proc icon is drawn (-1 = use default value)
		public float Gap { get; set; }

		public string ProcSoundYou { get; set; }
		public bool PlaySounds { get; set; }
		public string SayHasProcced { get; set; }
		public string SayYouHaveProcced { get; set; }
		private SoundPlayer SoundYou;
		//public long LastSpeakTime { get; set; } //milliseconds
		
		public IBrush ShadowBrush { get; set; }
		public IBrush BorderBrush { get; set; }
		public IBrush ProcBrushDefault { get; set; }
		public Dictionary<HeroClass, int[]> HeroColors { get; set; }
		public Dictionary<HeroClass, IBrush> ProcBrush { get; set; }
		
		public float BarWidth { get; set; }
		public float BarHeight { get; set; }
		public float IconSize { get; set; }

		public BuffRuleCalculator ProcRuleCalculator { get; set; }
		
        public PartyProcTracker()
        {
            Enabled = true;

			ShowTracker = true; //show/hide the countdown bars but keep recording the data to share with other plugins regardless

			PlaySounds = false; //play proc sound announcements
			ProcSoundYou = "ProcYou.wav"; //use empty string "" if you want to use TTS for your own notification
			
			SayHasProcced = " Pommmmmes ist ein Hommmmooo"; //the TTS line spoken when other party members proc
			SayYouHaveProcced = "Pommmmes ist ein Hommmo"; //the TTS line spoken when you proc
			
			Gap = 10f; //spacing inbetween each timer bar
			
			BarWidth = 45f; //a ratio of the screen width for the width of the proc timer bars
			BarHeight = 10f; //a ratio of the screen height for the height of the proc timer bars
			IconSize = 0.65f; //a ratio of the full size of the proc icons
			
			HeroColors = new Dictionary<HeroClass, int[]>() {
				{HeroClass.Barbarian, new int[3] {255, 67, 0}}, //r, g, b
				{HeroClass.Crusader, new int[3] {0, 200, 250}},
				{HeroClass.DemonHunter, new int[3] {0, 0, 255}},
				{HeroClass.Monk, new int[3] {252, 239, 0}},
				{HeroClass.Necromancer, new int[3] {252, 235, 191}},
				{HeroClass.WitchDoctor, new int[3] {163, 244, 65}},
				{HeroClass.Wizard, new int[3] {153, 51, 255}}
			};
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
			
			Snapshots = new Dictionary<uint, ProcInfo>();

			ProcBrush = new Dictionary<HeroClass, IBrush>();
			foreach(KeyValuePair<HeroClass, int[]> entry in HeroColors)
				ProcBrush.Add(entry.Key, Hud.Render.CreateBrush(175, entry.Value[0], entry.Value[1], entry.Value[2], 0));
			
			ShadowBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0); //change the opacity dynamically
			BorderBrush = Hud.Render.CreateBrush(255, 255, 0, 0, 2);
			
			ProcBrushDefault = Hud.Render.CreateBrush(200, 255, 0, 0, 0);

			TimeLeftFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
			PlayerFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
			
			ProcRuleCalculator = new BuffRuleCalculator(Hud);
			ProcRuleCalculator.SizeMultiplier = IconSize;
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Monk_Passive_NearDeathExperience.Sno) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Passive (downtime) 			
			 //ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Necromancer_Passive_FinalService.Sno) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Cooldown
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Crusader_Passive_Indestructible.Sno) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // passive (cooldown)
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.DemonHunter_Passive_Awareness.Sno) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Passive (cooldown)
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.WitchDoctor_Passive_SpiritVessel.Sno) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Passive (cooldown)
			ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Barbarian_IgnorePain.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Babarien IP			


			//testing only
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Barbarian_Sprint.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Rune 0, Rune 1, Rune 2, Rune 3, Rune 4, Rune 5 (Base Effect)
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Monk_SweepingWind.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});        // Rune 0, Rune 1, Rune 2, Rune 3, Rune 4, Rune 5 (Base Effect)
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Monk_Epiphany.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.DemonHunter_Vengeance.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Necromancer_SkeletalMage.Sno) { IconIndex = 6, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});
			ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.Wizard_Archon.Sno) { IconIndex = 2, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = false});
			//ProcRuleCalculator.Rules.Add(new BuffRule(Hud.Sno.SnoPowers.WitchDoctor_SoulHarvest.Sno) { IconIndex = 0, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = true});
			
			if (PlaySounds && ProcSoundYou != "") SoundYou = Hud.Sound.LoadSoundPlayer(ProcSoundYou);
        }
		
		public void OnNewArea(bool newGame, ISnoArea area)
		{
			if (newGame) {
				Snapshots.Clear();
			}
		}

        public void PaintTopInGame(ClipState clipState)
		//public void PaintWorld(WorldLayer layer)
        {
			if (!ShowTracker) return;
			
			var h = Hud.Window.Size.Height * 0.001667f * BarHeight; //8
			var w = Hud.Window.Size.Width * 0.00155f * BarWidth; //55
			
			if (PositionX == -1 || PositionY == -1) {
				var objectivesPosition = Hud.Render.GetUiElement("Root.NormalLayer.eventtext_bkgrnd.eventtext_region.title").Rectangle;
				PositionX = objectivesPosition.X - ProcRuleCalculator.StandardIconSize - Gap; // Hud.Render.MinimapUiElement.Rectangle.X - Gap*2 - w - Gap - ProcRuleCalculator.StandardIconSize; //left of the minimap
				PositionY = objectivesPosition.Y; //Hud.Window.Size.Height * 0.015f; //0.14f;
			}
			
			var x = PositionX;
			var y = PositionY;
			
			IEnumerable<KeyValuePair<uint, ProcInfo>> ActiveSnapshots = Snapshots.Where(kvp => kvp.Value.FinishTick > Hud.Game.CurrentGameTick);
			
			//draw background
			int active = ActiveSnapshots.Count();
			if (active > 0) {
				ShadowBrush.Opacity = 0.25f;
				ShadowBrush.DrawRectangle(x + ProcRuleCalculator.StandardIconSize, y, w + Gap*2 + BorderBrush.StrokeWidth, active * (ProcRuleCalculator.StandardIconSize + Gap) - Gap);
			}
			
			//draw countdowns
			foreach (KeyValuePair<uint, ProcInfo> entry in ActiveSnapshots) {
				//render the proc snapshot if it is not expired
				ProcInfo snapshot = entry.Value;
				float timeLeft = (float)(snapshot.FinishTick - Hud.Game.CurrentGameTick) / 60f; //seconds
				float w2 = (float)(timeLeft * w / snapshot.Duration);
				float opacity = (timeLeft >= 1 || (timeLeft < 1 && (Hud.Game.CurrentRealTimeMilliseconds / 200) % 2 == 0) ? 0.9f : 0.2f);
				
				snapshot.Texture.Draw(x, y, ProcRuleCalculator.StandardIconSize, ProcRuleCalculator.StandardIconSize, opacity); //draw custom icon
				BorderBrush.DrawRectangle(x+1, y+1, ProcRuleCalculator.StandardIconSize-2, ProcRuleCalculator.StandardIconSize-2); //draw custom border
				
				float x2 = x + ProcRuleCalculator.StandardIconSize + Gap + BorderBrush.StrokeWidth;
				float y2 = y + ProcRuleCalculator.StandardIconSize*0.5f - h*0.5f;

				ShadowBrush.Opacity = 0.25f;
				ShadowBrush.DrawRectangle(x2-4, y2-4, w+8, h+8);
				
				IBrush brush;
				if (!ProcBrush.TryGetValue(snapshot.PlayerClass, out brush))
					brush = ProcBrushDefault;
				
				brush.DrawRectangle(x2, y2, w2, h);
				BorderBrush.DrawRectangle(x2-2, y2-2, w+4, h+4);

				//draw countdown text
				TextLayout layout = TimeLeftFont.GetTextLayout(timeLeft.ToString(timeLeft > 1 ? "F0" : "F1"));
				TimeLeftFont.DrawText(layout, x2 + w*0.5f - layout.Metrics.Width*0.5f, y2 + h*0.5f - layout.Metrics.Height*0.5f);
				
				//draw player name
				layout = PlayerFont.GetTextLayout(snapshot.PlayerName);
				PlayerFont.DrawText(layout, x - layout.Metrics.Width - Gap, y + ProcRuleCalculator.StandardIconSize*0.5f - layout.Metrics.Height*0.5f);
				
				y += ProcRuleCalculator.StandardIconSize + Gap;
			} //end foreach
        } //end PaintTopInGame
		
		public void AfterCollect()
		{
			if (!Hud.Game.IsInGame) return;
			
			var toRemove = Snapshots.Where(pair => !Hud.Game.Players.Any(p => p.HeroId == pair.Key)).Select(pair => pair.Key).ToList();
			foreach (var key in toRemove)
				Snapshots.Remove(key);
			
			foreach (IPlayer player in Hud.Game.Players.Where(p => p.HasValidActor && p.SnoArea == Hud.Game.Me.SnoArea && p.CoordinateKnown)) {
				ProcRuleCalculator.CalculatePaintInfo(player); //check for procs				
				
				if (ProcRuleCalculator.PaintInfoList.Count > 0) { //proc detected
					BuffPaintInfo info = ProcRuleCalculator.PaintInfoList[0];
					int finishtick = Hud.Game.CurrentGameTick + (int)(info.TimeLeft*60d);
					int starttick = Hud.Game.CurrentGameTick - (int)(info.Elapsed*60d);
					double duration = info.TimeLeft + info.Elapsed;
					bool play = false;
					
					ProcInfo snapshot;
					if (!Snapshots.TryGetValue(player.HeroId, out snapshot)) {
						play = true;
						
						snapshot = new ProcInfo() {
							PlayerName = player.BattleTagAbovePortrait,
							HeroId = player.HeroId,
							PlayerClass = player.HeroClassDefinition.HeroClass,
							StartTick = starttick, 
							FinishTick = finishtick, //Hud.Game.CurrentRealTimeMilliseconds + (long)(info.TimeLeft*1000), //long endtime
							Duration = duration,
							Texture = info.Texture,
							SoundPlayedTick = Hud.Game.CurrentGameTick
						};

						Snapshots.Add(player.HeroId, snapshot);

					} else if (Math.Abs(finishtick - snapshot.FinishTick) > 2) { //different end time
						//is the old record expired?
						//if (snapshot.FinishTick < Hud.Game.CurrentGameTick) {
						if (starttick > snapshot.SoundPlayedTick) {
							play = true;
							snapshot.SoundPlayedTick = Hud.Game.CurrentGameTick;
						}
						
						snapshot.StartTick = starttick;
						snapshot.FinishTick = finishtick;
						snapshot.Duration = duration;
						snapshot.Count += 1;
						snapshot.Texture = info.Texture;
						
						//if (snapshot.PlayerName != player.BattleTagAbovePortrait) { //that is a desync, need to resync...would this ever happen?
						//}

						Snapshots[player.HeroId] = snapshot;
					}
					
					if (play && PlaySounds) {
						ThreadPool.QueueUserWorkItem(state => {
							try {
								if (player.IsMe)
									if (SoundYou != null) SoundYou.Play();
									else Hud.Sound.Speak(SayYouHaveProcced);
								else
									Hud.Sound.Speak(player.BattleTagAbovePortrait + SayHasProcced);
							} catch (Exception) {}
						});
					}
				} else {
					//no proc detected, but double check that the proc is not supposed to still be running (there may be an issue with proc buffs in that they momentarily register as inactive)
					ProcInfo snapshot;
					if (Snapshots.TryGetValue(player.HeroId, out snapshot)) {
						double timeLeft = (double)(snapshot.FinishTick - Hud.Game.CurrentGameTick) / 60d;
						IQuest riftQuest = Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == Hud.Sno.SnoQuests.GreaterNephalemRift_382695.Sno);
						
						if (timeLeft < 0 //expired snapshot
							|| timeLeft > snapshot.Duration //this happens when data lingers between games
							|| (riftQuest != null && (snapshot.FinishTick - (int)(snapshot.Duration*60)) < riftQuest.CreatedOn) //if the proc started before the GR was opened
							|| player.IsDeadSafeCheck) //player.HeadStone != null
							Snapshots[player.HeroId].FinishTick = 0;
					}
				}
			}
		}
    }
}