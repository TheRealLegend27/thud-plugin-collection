using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno.gems
{
    public class DHBoss : BasePlugin, IInGameWorldPainter
    {
        public GroundCircleDecorator DHDecorator { get; set; }
        public DHBoss()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            DHDecorator = new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 192, 96, 0, 1.5f),
                Radius = 20f
            };
        }

        public void PaintWorld(WorldLayer layer)
        {
            var me = Hud.Game.Me;
            var monsters = Hud.Game.AliveMonsters.Where(x => x.SnoMonster.Priority == MonsterPriority.boss);
            foreach (var monster in monsters)
            {
                if (me.Powers.BuffIsActive(338859, 0))
                    DHDecorator.Paint(monster, monster.FloorCoordinate, null);
            }

        }

    }

}