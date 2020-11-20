using System.Collections.Generic;
using System.Linq;
using System;
using Turbo.Plugins.Default;
namespace Turbo.Plugins.inferno.mods
{
    public class HealthBarOnElitePlugin : BasePlugin, IInGameWorldPainter
    {
        public IFont TextFont { get; set; }
        public IBrush BorderBrush { get; set; }
        public IBrush BackgroundBrush { get; set; }
        public IBrush RareBrush { get; set; }
        public IBrush RareJBrush { get; set; }
        public IBrush ChampionBrush { get; set; }
        public IFont TextFontHaunt { get; set; }
        public IFont TextFontLocust { get; set; }
 
        public HealthBarOnElitePlugin()
        {
            Enabled = false;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, true);
            BorderBrush = Hud.Render.CreateBrush(255, 0, 100, 0, -1);
            BackgroundBrush = Hud.Render.CreateBrush(255, 128, 128, 128, 0);
            RareBrush = Hud.Render.CreateBrush(255, 255, 148, 20, 0);
            RareJBrush = Hud.Render.CreateBrush(255, 255, 50, 0, 0);
            ChampionBrush = Hud.Render.CreateBrush(255, 0, 128, 255, 0);
 
            TextFontLocust = Hud.Render.CreateFont("tahoma", 9, 255, 0, 200, 0, false, false, true);
            TextFontHaunt = Hud.Render.CreateFont("tahoma", 9, 255, 255, 0, 0, false, false, true);
            TextFontLocust.SetShadowBrush(255, 0, 200, 0, true);
            TextFontHaunt.SetShadowBrush(255, 255, 0, 0, true);
        }
 
        public void PaintWorld(WorldLayer layer)
        {
            var h = 17;
            var w1 = 30;
            var textLocust = "L";
            var layoutLocust = TextFontLocust.GetTextLayout(textLocust);
            var textHaunt = "H";
            var layoutHaunt = TextFontHaunt.GetTextLayout(textHaunt);
            var py = Hud.Window.Size.Height / 600;
            var monsters = Hud.Game.AliveMonsters.Where(x => x.IsAlive);
            List<IMonster> monstersElite = new List<IMonster>();
            foreach (var monster in monsters)
            {
            if (monster.SummonerAcdDynamicId == 0)
            {
                if (monster.Rarity == ActorRarity.Champion || monster.Rarity == ActorRarity.Rare)
                {
                    monstersElite.Add(monster);
                }
            }
            }
            foreach (var monster in monstersElite)
            {
                    var hptext = ValueToString(monster.CurHealth * 100 / monster.MaxHealth, ValueFormat.NormalNumberNoDecimal);
                    var layout = TextFont.GetTextLayout(hptext);
                    var w = monster.CurHealth * w1 / monster.MaxHealth;
                    var monsterX = monster.FloorCoordinate.ToScreenCoordinate().X - w1 / 2;
                    var monsterY = monster.FloorCoordinate.ToScreenCoordinate().Y + py * 12;
                    var locustX = monsterX - w1 / 2;
                    var hauntX = monsterX + w1 + 5;
                    var buffY = monsterY - 1;
                    var hpX = monsterX + 7;
 
                    BorderBrush.DrawRectangle(monsterX, monsterY, w1, h);
                    BackgroundBrush.DrawRectangle(monsterX, monsterY, w1, h);
                    if (monster.Rarity == ActorRarity.Champion) ChampionBrush.DrawRectangle(monsterX, monsterY, (float)w, h);
                    if (monster.Rarity == ActorRarity.Rare)
                    {
                        bool flagJ = false;
                        foreach (var snoMonsterAffix in monster.AffixSnoList)
                        {
                            if (snoMonsterAffix.Affix == MonsterAffix.Juggernaut)
                            {
                                flagJ = true;
                                break;
                            }
                        }
                        if (flagJ) RareJBrush.DrawRectangle(monsterX, monsterY, (float)w, h);
                        else RareBrush.DrawRectangle(monsterX, monsterY, (float)w, h);
                    }
                    if (monster.Locust) TextFontLocust.DrawText(layoutLocust, locustX, buffY);
                    if (monster.Haunted) TextFontHaunt.DrawText(layoutHaunt, hauntX, buffY);
                    TextFont.DrawText(layout, hpX, buffY);
               
            }
            monstersElite.Clear();
        }
    }
}