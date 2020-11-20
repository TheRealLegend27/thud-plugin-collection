using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.Minimap
{
    public class BloodSpringPlugin : BasePlugin, IInGameWorldPainter
    {
 
        public WorldDecoratorCollection BloodSpringDecoratorSmall { get; set; }
        public WorldDecoratorCollection BloodSpringDecoratorMedium { get; set; }
        public WorldDecoratorCollection BloodSpringDecoratorLarge { get; set; }
 
        public BloodSpringPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);          
            BloodSpringDecoratorSmall = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 3),
                    Radius = 6.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 555),
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 5, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 6.0f,
                }
            );
 
            BloodSpringDecoratorMedium = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 3),
                    Radius = 11.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 555),
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 5, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 11.0f,
                }
            );
 
            BloodSpringDecoratorLarge = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 3),
                    Radius = 11.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 555),
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(120, 255, 0, 0, 5, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 19.0f,
                }
            );
        }
 
        public void PaintWorld(WorldLayer layer)
        {
 
            var actors = Hud.Game.Actors;
            foreach (var actor in actors)
            {
               
                switch (actor.SnoActor.Sno)
                {
           
                    case ActorSnoEnum._x1_bog_bloodspring_small: //small blood spring
                        BloodSpringDecoratorSmall.Paint(layer, actor, actor.FloorCoordinate, null);
                        break;
                    case ActorSnoEnum._x1_bog_bloodspring_medium: //medium blood spring
                        BloodSpringDecoratorMedium.Paint(layer, actor, actor.FloorCoordinate, null);
                        break;
                    case ActorSnoEnum._x1_bog_bloodspring_large: //large blood spring
                        BloodSpringDecoratorLarge.Paint(layer, actor, actor.FloorCoordinate, null);
                        break;
                }  
            }
 
        }
    }
}