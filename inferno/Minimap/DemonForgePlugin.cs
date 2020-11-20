using System.Linq;
using Turbo.Plugins.Default;
using System.Collections.Generic;
 
namespace Turbo.Plugins.Inferno.Plaayers.Minimap
{
    public class DemonForgePlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection DemonForgeDecorator { get; set; }
       
        private HashSet<ActorSnoEnum> demonicForgesIds = new HashSet<ActorSnoEnum>() { 
             
             (ActorSnoEnum)174900,
             (ActorSnoEnum)185391 
            
            
            };
       
        public DemonForgePlugin()
        {
            Enabled = true;
        }
        public override void Load(IController hud)
        {
            base.Load(hud);          
            DemonForgeDecorator = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 0),
                    Radius = 10.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 333),
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(100, 255, 255, 220, 5, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 45,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(160, 255, 255, 220, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 0, 0, true, false, false),                    
                }
                );
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            var demonforge = Hud.Game.Actors.Where(x => demonicForgesIds.Contains(x.SnoActor.Sno));
            foreach (var actor in demonforge)
            {
                DemonForgeDecorator.Paint(layer, actor, actor.FloorCoordinate, "!!! " + actor.SnoActor.NameLocalized + " !!!");
            }
        }
    }
}