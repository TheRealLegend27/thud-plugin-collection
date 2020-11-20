using System.Linq;
using System.Collections.Generic;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno
{
    public class DAV_BossWarmingPlugin : BasePlugin, IInGameWorldPainter {
        public Dictionary<AnimSnoEnum, string> WarmingMessage;
       
        public WorldDecoratorCollection MessageDecorator { get; set; }
        public float WarmingOffsetX { get; set; }
        public float WarmingOffsetY { get; set; }
        public float WarmingOffsetZ { get; set; }
        public bool ShowOrlashClone { get; set; }
        public bool GRonly { get; set; }
       
        public DAV_BossWarmingPlugin() {
            Enabled = true;
            WarmingOffsetX = -20.0f;
            WarmingOffsetY = 0.0f;
            WarmingOffsetZ = 10.0f;
            ShowOrlashClone = false;
            GRonly = true   ;
        }
 
        public override void Load(IController hud) {
            base.Load(hud);
 
            WarmingMessage = new Dictionary<AnimSnoEnum, string>();
           
            MessageDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud) {
                    BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 15, 255, 255, 255, 51, true, true, true),
                }
            );
        }
       
        public void PaintWorld(WorldLayer layer) {
            if (GRonly && Hud.Game.SpecialArea != SpecialArea.GreaterRift) return ;
           
            var bosses = Hud.Game.AliveMonsters.Where(m => m.Rarity == ActorRarity.Boss);
            foreach(IMonster m in bosses) {
                if (!ShowOrlashClone && m.SummonerAcdDynamicId != 0) continue;
               
                string outmessage;
                if (!WarmingMessage.TryGetValue(m.Animation, out outmessage)) continue;
               
                MessageDecorator.Paint(layer, m, m.FloorCoordinate.Offset(WarmingOffsetX, WarmingOffsetY, WarmingOffsetZ), outmessage);
            }
        }
    }
}