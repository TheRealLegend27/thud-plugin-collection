using SharpDX.DirectInput;
using System.Linq;
using System;
using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno.Zy
{
    using System.Text;


    public class PylonRange : BasePlugin, IInGameWorldPainter
    {

		public WorldDecoratorCollection StatisticalRangeDecorator { get; set; }
        private MapShapeDecorator baseMapShapeDecorator;


        public PylonRange()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            

            baseMapShapeDecorator = new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 0, 0, 0, 1),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = 75,
            };
            StatisticalRangeDecorator = new WorldDecoratorCollection(baseMapShapeDecorator);
        }


        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.IsInTown) return;
            StatisticalRangeDecorator.Paint(layer, null, Hud.Game.Me.FloorCoordinate, null);
        }

       
    }
}