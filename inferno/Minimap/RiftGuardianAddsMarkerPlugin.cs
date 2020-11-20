using System.Linq;
using Turbo.Plugins.Default;
namespace Turbo.Plugins.inferno.Minimap
{
    public class RiftGuardianAddsMarkerPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection RiftGuardianAddDecorator { get; set; }
 
        private bool GuardianIsAlive { get { return riftQuest == null || riftQuest.QuestStepId == 16; } }
        private IQuest riftQuest { get { return Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 337492); } }
 
        public RiftGuardianAddsMarkerPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
 
            RiftGuardianAddDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(200, 17, 255, 69, 2, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 4f,
                });
        }
 
        public  void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.SpecialArea != SpecialArea.GreaterRift || !GuardianIsAlive) return;
 
            var monsters = Hud.Game.AliveMonsters.Where(m => m.Rarity != ActorRarity.Boss);
            foreach (var monster in monsters)
            {
                RiftGuardianAddDecorator.Paint(layer, monster, monster.FloorCoordinate, string.Empty);
            }
        }
    }
}