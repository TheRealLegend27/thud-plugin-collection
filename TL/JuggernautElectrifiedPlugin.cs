namespace Turbo.Plugins.TL
{
    using System.Collections.Generic;
    using System.Linq;
    using Turbo.Plugins.Default;

    public delegate string AffixNameFunc(ISnoMonsterAffix affix);

    public class JuggernautElectrifiedPlugin : BasePlugin, IInGameWorldPainter
    {
        public JuggernautElectrifiedPlugin()
        {
            Enabled = true;
            Order = 1;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public void PaintWorld(WorldLayer layer)
        {
            var players = Hud.Game.Players;
            var necCount = 0;
            var barbCount = 0;

            foreach (var player in players)
            {
                if (player.HeroClassDefinition.HeroClass == HeroClass.Barbarian)
                    barbCount++;
                else if (player.HeroClassDefinition.HeroClass == HeroClass.Necromancer)
                    necCount++;
            }

            var monsters = Hud.Game.AliveMonsters.Where(m => m.Rarity == ActorRarity.Rare);
            foreach (var monster in monsters)
            {
                var affixList = monster.AffixSnoList;
                bool isJugger = false;
                bool isElectrified = false;
                foreach(var affix in affixList)
                {
                    if (affix.Affix == MonsterAffix.Juggernaut)
                        isJugger = true;
                    else if (affix.Affix == MonsterAffix.Electrified)
                        isElectrified = true;
                }

                if (isJugger && isElectrified)
                {
                    var font = Hud.Render.CreateFont("tahoma", 10f, 200, 255, 0, 0, false, false, 128, 0, 0, 0, true);
                    var text = "☠";
                    var layout = font.GetTextLayout(text);
                    float mapX, mapY;
                    Hud.Render.GetMinimapCoordinates(monster.FloorCoordinate.X, monster.FloorCoordinate.Y, out mapX, out mapY);

                    font.DrawText(layout, mapX - layout.Metrics.Width / 2, mapY);
                }
            }
        }
    }
}