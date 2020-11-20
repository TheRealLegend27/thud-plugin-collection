using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.Klasse
{
 
    public class OtherPlayersNameColorByClassPlugin : BasePlugin, ICustomizer
    {
 
        public OtherPlayersNameColorByClassPlugin()
        {
            Enabled = false;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
        }
 
 
        public void Customize()
        {
            Hud.RunOnPlugin<OtherPlayersPlugin>(plugin =>
            {
                plugin.DecoratorByClass[HeroClass.Barbarian].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 255, 0, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.Barbarian].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 0, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 255, 0, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.Crusader].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 128, 128, 128, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.Crusader].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 128, 128, 128, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 128, 128, 128, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.DemonHunter].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 0, 0, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.DemonHunter].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 255, 0, 0, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 0, 0, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.Monk].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 255, 255, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.Monk].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 255, 255, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.Necromancer].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255,255,255, 99, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.Necromancer].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 33, 106, 99, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255,255,255, 99, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.WitchDoctor].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 0, 0, 255, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.WitchDoctor].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 0, 0, 255, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 0, 0, 255, false, false, 128, 0, 0, 0, true);
                });
               
                plugin.DecoratorByClass[HeroClass.Wizard].GetDecorators<MapLabelDecorator>().ForEach(d =>
                {
                    d.LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 153, 0, 153, false, false, 128, 0, 0, 0, true);
                    d.Up = true;
                });
                plugin.DecoratorByClass[HeroClass.Wizard].GetDecorators<GroundLabelDecorator>().ForEach(d =>
                {
                    d.BorderBrush = Hud.Render.CreateBrush(255, 153, 0, 153, 1);
                    d.TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 153, 0, 153, false, false, 128, 0, 0, 0, true);
                });
            });
        }
 
    }
 
}