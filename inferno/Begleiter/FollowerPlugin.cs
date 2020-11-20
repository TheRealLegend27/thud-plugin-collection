using System.Linq;
using System.Collections.Generic;
 
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.Begleiter

{
    public class FollowerPlugin : BasePlugin, IInGameWorldPainter
    {
        //4482  Hireling_Enchantress    Enchantress
        //52694 Hireling_Scoundrel      Scoundrel
        //52693 Hireling_Templer        Templer
		 // 453793
        private readonly ActorSnoEnum[] snoIds = new ActorSnoEnum[] { 
		(ActorSnoEnum)4482, 
		(ActorSnoEnum)52693, 
		(ActorSnoEnum)52694, 
		(ActorSnoEnum)453793, 
		(ActorSnoEnum)473420 
		};
        
		
		public Dictionary<ActorSnoEnum, WorldDecoratorCollection> FollowerMapping { get; set; }
       
        public FollowerPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
           
            FollowerMapping = new Dictionary<ActorSnoEnum, WorldDecoratorCollection>();
           
            foreach (ActorSnoEnum sno in snoIds)
                FollowerMapping.Add(sno, createWDC());
        }
       
        private WorldDecoratorCollection createWDC(int a = 220, int r=255, int g=51, int b=153, float strokeWidth_T=3f, float strokeWidth_C=5f, float strokeWidth_S=5f, float radiusM=8f, float radiusG=1f)
        {
            return new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(a, r, g, b, strokeWidth_T),
                    ShapePainter = new TriangleShapePainter(Hud),
                    Radius = radiusM
                },
                new GroundShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(a, r, g, b, strokeWidth_S),
                    Radius = radiusG,
                    ShapePainter = WorldStarShapePainter.NewCross(Hud),
                    // RotationTransformator = new CircularRotationTransformator(Hud, 10)
                },
                new GroundCircleDecorator(Hud) {
                    Brush = Hud.Render.CreateBrush(a, r, g, b, strokeWidth_C),
                    Radius = radiusG
                }
            );
        }
       
        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.NumberOfPlayersInGame > 1) return;
           
            foreach (var actor in Hud.Game.Actors.Where(x => snoIds.Contains(x.SnoActor.Sno)))
            {
                FollowerMapping[actor.SnoActor.Sno].Paint(layer, actor, actor.FloorCoordinate, string.Empty);
            }
        }
    }
}