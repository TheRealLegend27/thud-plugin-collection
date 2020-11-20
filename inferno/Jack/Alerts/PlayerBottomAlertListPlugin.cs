﻿namespace Turbo.Plugins.inferno.Jack.Alerts
{
    using System.Collections.Generic;
    using Turbo.Plugins.Default;

    public class PlayerBottomAlertListPlugin : BasePlugin, IInGameTopPainter, IInGameWorldPainter
    {
        public AlertList AlertList { get; set; }

        public PlayerBottomAlertListPlugin()
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
                RatioY = 0.62f,
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