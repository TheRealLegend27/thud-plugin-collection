// http://www.ownedcore.com/forums/diablo-3/turbohud/turbohud-plugin-review-zone/632463-v7-3-international-glq-grprogressiontotime.html
 
using Turbo.Plugins.Default;
using System.Linq;
using System;
using System.Globalization;
 
namespace Turbo.Plugins.inferno.Riftprogress
{
    public class GLQ_GRProgressionToTime : BasePlugin, IInGameTopPainter, IAfterCollectHandler
    {
        public IFont BonusTimeFont { get; set; }
        public IFont MalusTimeFont { get; set; }
       
        public string SecondsFormat { get; set; }
        public string MinutesSecondsFormat { get; set; }
 
        private IFont currentFont;
       
        public bool IsGreaterRift
        {
            get
            {
                return riftQuest != null &&
                       (riftQuest.QuestStepId == 13 || riftQuest.QuestStepId == 16 || riftQuest.QuestStepId == 34 ||
                        riftQuest.QuestStepId == 46);
            }
        }
 
        private IQuest riftQuest
        {
            get
            {
                return Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 337492) ?? // rift
                       Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 382695);   // gr
            }
        }
       
        private const uint greaterriftMaxTimeMilliseconds = 900000;
        private IWatch pauseTimer;  //for solo
 
        public GLQ_GRProgressionToTime()
        {
            Enabled =true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            MalusTimeFont = Hud.Render.CreateFont("tahoma", 7, 255, 250, 100, 100, true, false, 160, 0, 0, 0, true);
            BonusTimeFont = Hud.Render.CreateFont("tahoma", 7, 255, 128, 255, 0, true, false, 160, 0, 0, 0, true);
           
            MinutesSecondsFormat = "{0:%m}m {0:ss}s";
           
            pauseTimer = Hud.Time.CreateWatch();     //solo
        }
       
        //pauseTimer handling for GR-Solos
        public void AfterCollect()
        {
            if (ResetTimers()) return;
 
            if (GamePauseTimers()) return;
 
            RestartStopTimers();
        }
 
        private void RestartStopTimers()
        {
            // (re)start/stop timers if needed
            if (pauseTimer.IsRunning)
                pauseTimer.Stop();
        }
 
        private bool GamePauseTimers()
        {
            // game pause
            if (Hud.Game.IsPaused || (IsGreaterRift && Hud.Game.NumberOfPlayersInGame == 1 && Hud.Game.IsLoading))
            {
                if (!pauseTimer.IsRunning)
                    pauseTimer.Start();
 
                return true;
            }
            return false;
        }
 
        private bool ResetTimers()
        {
            // reset states if needed
            if (riftQuest == null || (riftQuest != null && riftQuest.State == QuestState.none))
            {
                if (pauseTimer.IsRunning || pauseTimer.ElapsedMilliseconds > 0)
                {
                    pauseTimer.Reset();
                }
               
                return true;
            }
            return false;
        }
       
        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            if (!IsGreaterRift) return;
           
            var ui = Hud.Render.GreaterRiftBarUiElement;
           
            if (ui == null || !ui.Visible) return;
            var secondsElapsed = riftQuest.StartedOn.ElapsedMilliseconds;
            var percent = (float)Hud.Game.RiftPercentage;
           
            if (secondsElapsed > greaterriftMaxTimeMilliseconds) return;
 
            string text;
            var _myTime_ms = percent*9000f - secondsElapsed - pauseTimer.ElapsedMilliseconds;
            var _myTime_span = TimeSpan.FromMilliseconds(_myTime_ms);
           
            if (_myTime_ms <= 0)
            {
                text = "- " + String.Format(CultureInfo.InvariantCulture, MinutesSecondsFormat, _myTime_span);
                currentFont = MalusTimeFont;
            }
            else
            {
                text = "+ " + String.Format(CultureInfo.InvariantCulture, MinutesSecondsFormat, _myTime_span);
                currentFont = BonusTimeFont;
            }
           
            var textLayout = currentFont.GetTextLayout(text);
            var x = ui.Rectangle.Left + ui.Rectangle.Width / 100.0f * percent - textLayout.Metrics.Width / 2;
            var y = ui.Rectangle.Top - ui.Rectangle.Height * 0.1f - textLayout.Metrics.Height;
            currentFont.DrawText(textLayout, x, y);
        }
    }
}