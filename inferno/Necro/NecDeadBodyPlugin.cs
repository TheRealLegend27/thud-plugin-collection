using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.Necro
{
    using System.Text;
 
    public class NecDeadBodyPlugin : BasePlugin, IInGameTopPainter, IInGameWorldPainter
    {
        public IFont TextFont { get; set; }
        public int yard { get; set; }
        public bool DeadBodyCircle { get; set; }
        public bool DeadBodyCount { get; set; }
        public TopLabelWithTitleDecorator DeadBodyCountDecorator { get; set; }
        public WorldDecoratorCollection DeadBodyCircleDecorator { get; set; }
        public float XWidth { get; set; }
        public float YHeight { get; set; }    
 
        public NecDeadBodyPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            DeadBodyCircle = true;
            DeadBodyCount = true;
            DeadBodyCountDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(80, 134, 238, 240, 0),
                BorderBrush = Hud.Render.CreateBrush(255, 0, 0, 0, -1),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 0, 0, true, false, false),
            };
 
            DeadBodyCircleDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 2f),
                    Radius = 1.5f
                }
                );
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            var DeadBody = Hud.Game.Actors.Where(x => x.SnoActor.Sno == ActorSnoEnum._p6_necro_corpse_flesh);
            if(DeadBodyCircle == true)
            foreach (var actor in DeadBody)
            {
                DeadBodyCircleDecorator.Paint(layer, actor, actor.FloorCoordinate, "");
            }
        }
        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            var DeadBody = Hud.Game.Actors.Where(d => d.SnoActor.Sno == ActorSnoEnum._p6_necro_corpse_flesh && d.IsOnScreen);
            int Count = 0;
            var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.game_dialog_backgroundScreenPC.game_progressBar_manaBall").Rectangle;
            var w = uiRect.Width / 1.6f;
            var h = uiRect.Height * 0.15f;
            var x = uiRect.Left + w * 0.28f;
            var y = uiRect.Top - uiRect.Height * 0.17f;
            if (DeadBodyCount == true)
            {
                foreach (var actor in DeadBody)
                {
                    Count++;
                }
                if (Count > 0)
                {
                    switch (Hud.Game.Me.HeroClassDefinition.HeroClass)
                    {
                        case HeroClass.Necromancer:
                            DeadBodyCountDecorator.Paint(x, y, w, h, "Body:" + Count);
                            break;
                    }
                }
 
            }
 
 
           
 
        }
    }
 
}