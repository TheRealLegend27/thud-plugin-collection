﻿namespace Turbo.Plugins.inferno.mods
{
    public class SnoPowerId
    {
        public uint Sno { get; set; }
        public int? Icon { get; set; }

        public SnoPowerId(uint sno, int? icon = null)
        {
            Sno = sno;
            Icon = icon;
        }
    }
}
