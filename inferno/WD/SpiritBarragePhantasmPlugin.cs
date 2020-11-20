using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.WD
{
    public class SpiritBarragePhantasmPlugin : BasePlugin, IInGameWorldPainter
    {
 
        public WorldDecoratorCollection SpiritBarragePhantasmDecorator { get; set; }
       
        public SpiritBarragePhantasmPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            SpiritBarragePhantasmDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 0, 128, 255, 2),
                    Radius = 10,
                },
                new GroundLabelDecorator(Hud)
                {
                    CountDownFrom = 5,
                    TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 100, 255, 150, true, false, 128, 0, 0, 0, true),
                },
                new GroundTimerDecorator(Hud)
                {
                    CountDownFrom = 5,
                    BackgroundBrushEmpty = Hud.Render.CreateBrush(100, 0, 0, 0, 0),
                    BackgroundBrushFill = Hud.Render.CreateBrush(200, 0, 128, 255, 0),
                    Radius = 25,
                }
            );
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            var actors = Hud.Game.Actors;
            foreach (var actor in actors)
            {
                if (actor.SummonerAcdDynamicId == Hud.Game.Me.SummonerId)
                {
                    switch (actor.SnoActor.Sno)
                    {
                        case ActorSnoEnum._wd_spiritbarragerune_aoe_ghostmodel:
                            SpiritBarragePhantasmDecorator.Paint(layer, actor, actor.FloorCoordinate, null);
                            break;
                    }
                }
            }
        }
 
    }
 
}