using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.gems
{
    public class Necrocicle : BasePlugin, IInGameWorldPainter
    {
        public GroundCircleDecorator ZeiDecorator { get; set; }
        public Necrocicle()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
           
            ZeiDecorator = new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255,192,96,0, 1.5f),
                Radius = 50f
            };
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            var me = Hud.Game.Me;
            var monsters = Hud.Game.AliveMonsters.Where(x => x.SnoMonster.Priority == MonsterPriority.boss);
            foreach (var monster in monsters)
            {
            if (me.Powers.BuffIsActive(403468, 0))
                    ZeiDecorator.Paint(monster, monster.FloorCoordinate, null);
			if (Hud.Game.Me.HeroClassDefinition.HeroClass != HeroClass.Necromancer || Hud.Game.IsInTown)return;	
            }
 
        }
 
    }
 
}