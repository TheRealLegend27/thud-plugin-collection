using System;
using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
 
namespace Turbo.Plugins.inferno
{
    public class DAV_BossAnimeLog : BasePlugin, IInGameWorldPainter, INewAreaHandler/*, IChatLineChangedHandler */{
        private string CurrentAnime { get; set; }
        public List<string> BossList;
        public WorldDecoratorCollection BossDecorator { get; set; }
       
        private IUiElement chatentry {
            get { return Hud.Render.GetUiElement("Root.NormalLayer.chatentry_dialog_backgroundScreen.chatentry_content.chat_editline"); }
        }
 
        public DAV_BossAnimeLog() {
            Enabled = true;
           
            BossList = new List<string> {};
        }
       
        private IQuest riftQuest { get { return Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 382695); } }
 
        public override void Load(IController hud) {
            base.Load(hud);
           
            BossDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud) {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 102, 204, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(192, 255, 255, 255, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6.5f, 255, 255, 255, 255, true, false, false)
                }
            );
        }
 
        public void PaintWorld(WorldLayer layer) { 
        //  if (Hud.Game.SpecialArea != SpecialArea.GreaterRift) return ;
           
            var bosses = Hud.Game.AliveMonsters.Where(m => m.Rarity == ActorRarity.Boss);
            foreach(IMonster monster in bosses) {
                if (monster.SummonerAcdDynamicId != 0) continue;
                if (BossList.Contains(monster.SnoMonster.NameLocalized) || BossList.Contains(monster.SnoMonster.NameEnglish)) {
                    BossDecorator.Paint(layer, monster, monster.FloorCoordinate, monster.Animation.ToString());
               
                    var TmpAnime = monster.Animation.ToString();
                    if (CurrentAnime == null || CurrentAnime != TmpAnime) {
                        CurrentAnime = TmpAnime;
                        var writelog = "\t" + monster.SnoMonster.NameLocalized + "\t" + TmpAnime;
                        Hud.TextLog.Log("BossAnimeLog", writelog, true, true);
                    }
                }
            }
        }
       
        public void OnNewArea(bool newGame, ISnoArea area) {
            if (riftQuest == null || (riftQuest != null && riftQuest.State == QuestState.none)) {
                var message = "----- New Game -----";
                if (CurrentAnime == null || CurrentAnime != message) {
                    CurrentAnime = message;
                    Hud.TextLog.Log("BossAnimeLog",message, true, true);
                }
            }
        }
    }
}