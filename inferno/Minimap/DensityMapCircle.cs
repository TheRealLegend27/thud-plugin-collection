using Turbo.Plugins.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Turbo.Plugins.inferno
{

    public class DensityMapCircle : BasePlugin, IInGameWorldPainter
    {

        public WorldDecoratorCollection CircleDecorator { get; set; }
        public float CircleSize { get; set; }

        public DensityMapCircle()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            CircleSize = 10;
        }

        public void PaintWorld(WorldLayer layer)
        {
            // 5% of Rift circle part
            CircleDecorator = new WorldDecoratorCollection(
               new MapShapeDecorator(Hud)
               {
                   Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 2),
                   ShapePainter = new CircleShapePainter(Hud),
                   Radius = CircleSize,
               }
             );

            if ((Hud.Game.Me.InGreaterRift || Hud.Game.SpecialArea == SpecialArea.Rift || Hud.Game.SpecialArea == SpecialArea.ChallengeRift) && Hud.Game.RiftPercentage < 100)
            {
                var monsters = Hud.Game.AliveMonsters.OrderByDescending(x => x.SnoMonster.RiftProgression);
                foreach (var monster in monsters)
                {
                    CircleSize = 10;
                    NewLoop:
                    var CircleSizeYardMonsters = monsters.Where(x => x.FloorCoordinate.XYDistanceTo(monster.FloorCoordinate) <= CircleSize);
                    float RiftPercentage = 0f;
                    foreach (var CircleSizeYardMonster in CircleSizeYardMonsters)
                    {
                        RiftPercentage = RiftPercentage + CircleSizeYardMonster.SnoMonster.RiftProgression;
                        if (CircleSizeYardMonster.Rarity == ActorRarity.Rare) RiftPercentage = RiftPercentage + 28.6f; // 4.4% of 650 (4 progression orb drops per yellow)
                        else if (CircleSizeYardMonster.Rarity == ActorRarity.Champion) RiftPercentage = RiftPercentage + 7.15f; // 1.1% of 650 (1 progression orb drop per blue)
                    }

                    float PercentOfRift = 32.5f; // 5% of 650
                    if (Hud.Game.RiftPercentage > 95) PercentOfRift = (float)(((100 - Hud.Game.RiftPercentage) / 100) * 650); // if less than 5% rift completion left, use that percentage instead.

                    if (RiftPercentage >= PercentOfRift)
                    {
                        CircleDecorator.Paint(layer, null, monster.FloorCoordinate, null);
                        break;
                    }
                    else if (CircleSize < 56) // Within 55 yards max
                    {
                        CircleSize++;
                        goto NewLoop;
                    }

                }
            }
        }
    }

}