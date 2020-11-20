using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{
    public class GlobePlugin : BasePlugin, IInGameWorldPainter
	{

        public WorldDecoratorCollection Globe { get; set; }
        public List<ActorSnoEnum> GlobeSno = new List<ActorSnoEnum>        
        {
            (ActorSnoEnum)366139, 
            (ActorSnoEnum)367978, 
            (ActorSnoEnum)375132, 
            (ActorSnoEnum)4267, 
            (ActorSnoEnum)85798, 
            (ActorSnoEnum)209093, 
            (ActorSnoEnum)209120, 
            (ActorSnoEnum)375124, 
            (ActorSnoEnum)375125, 
            (ActorSnoEnum)85816
        };

        public GlobePlugin()
		{
            Enabled = true;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);

            Globe = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 5),
                    ShapePainter = new CircleShapePainter(Hud),
		    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 1333),		
                    Radius = 8f,
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 204, 0, 0, 0),
		    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 850),
                    Radius = 1f
                },

		new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 255, 102, 2),
                    Radius = 1f
                
                });
        }

        void IInGameWorldPainter.PaintWorld(WorldLayer layer)
		{
            var actors = Hud.Game.Actors.Where(a => GlobeSno.Contains(a.SnoActor.Sno));
            foreach (var actor in actors)
			{
                Globe.Paint(layer, actor, actor.FloorCoordinate, "");
			}
		}
    }

}
