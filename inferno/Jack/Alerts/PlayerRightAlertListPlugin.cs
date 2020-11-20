namespace Turbo.Plugins.inferno.Jack.Alerts
{
    using System.Collections.Generic;
    using Turbo.Plugins.Default;

    public class PlayerRightAlertListPlugin : BasePlugin, IInGameTopPainter, IInGameWorldPainter
    {
        public AlertList AlertList { get; set; }

        public PlayerRightAlertListPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            AlertList = new AlertList(Hud)
            {
                TextAlign = HorizontalAlign.Center,
                Up = false,
                RatioY = 0.5f,
                RatioWidth = 0.2f,
                RatioX = 0.65f,
                VerticalCenter = true,
            };
    }

        public void PaintWorld(WorldLayer layer)
        {
            AlertList.PaintWorld(layer);
        }

        public void PaintTopInGame(ClipState clipState)
        {
            AlertList.PaintTopInGame(clipState);
        }
    }
}