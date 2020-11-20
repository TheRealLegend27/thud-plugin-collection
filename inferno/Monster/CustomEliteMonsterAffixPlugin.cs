using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno.Monster
{

    public class CustomEliteMonsterAffixPlugin : BasePlugin, IInGameWorldPainter, ICustomizer
    {

        public Dictionary<MonsterAffix, WorldDecoratorCollection> AffixDecorators { get; set; }
        public Dictionary<MonsterAffix, string> CustomAffixNames { get; set; }

        public GroundLabelDecorator EliteHealthDecorator { get; set; }

        public CustomEliteMonsterAffixPlugin()
        {
            Enabled = true;
            Order = 20000;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            CustomAffixNames = new Dictionary<MonsterAffix, string>();

            CustomAffixNames.Add(MonsterAffix.Juggernaut, "");
            CustomAffixNames.Add(MonsterAffix.Wormhole, "ASSHOLE");
            CustomAffixNames.Add(MonsterAffix.Reflect, "");
            CustomAffixNames.Add(MonsterAffix.Arcane, "");
            CustomAffixNames.Add(MonsterAffix.Jailer, "");
            CustomAffixNames.Add(MonsterAffix.FireChains, "");
            CustomAffixNames.Add(MonsterAffix.Mortar, "");
            CustomAffixNames.Add(MonsterAffix.Molten, "");
            CustomAffixNames.Add(MonsterAffix.Desecrator, "");
            CustomAffixNames.Add(MonsterAffix.Frozen, "");
            CustomAffixNames.Add(MonsterAffix.FrozenPulse, "");
            CustomAffixNames.Add(MonsterAffix.Orbiter, "");
            CustomAffixNames.Add(MonsterAffix.Electrified, "");
            CustomAffixNames.Add(MonsterAffix.Thunderstorm, "");
            CustomAffixNames.Add(MonsterAffix.Poison, "");
            CustomAffixNames.Add(MonsterAffix.Shielding, "");
            CustomAffixNames.Add(MonsterAffix.Illusionist, "");
            CustomAffixNames.Add(MonsterAffix.Waller, "");
            CustomAffixNames.Add(MonsterAffix.Teleporter, "");
            CustomAffixNames.Add(MonsterAffix.HealthLink, "");
            CustomAffixNames.Add(MonsterAffix.ExtraHealth, "");
            CustomAffixNames.Add(MonsterAffix.Fast, "");
            CustomAffixNames.Add(MonsterAffix.Knockback, "");
            CustomAffixNames.Add(MonsterAffix.Nightmarish, "");
            CustomAffixNames.Add(MonsterAffix.Vampiric, "");
            CustomAffixNames.Add(MonsterAffix.Vortex, "");
            CustomAffixNames.Add(MonsterAffix.Avenger, "");
            CustomAffixNames.Add(MonsterAffix.Horde, "");
            CustomAffixNames.Add(MonsterAffix.MissileDampening, "");

            var BorderBrush = Hud.Render.CreateBrush(128, 0, 0, 0, 2);
            var LabelFont = Hud.Render.CreateFont("tahoma", 6f, 240, 240, 240, 240, true, false, true);

            AffixDecorators = new Dictionary<MonsterAffix, WorldDecoratorCollection>();

            // Important: Juggernaut, Reflect, Wormhole - red
            AffixDecorators.Add(MonsterAffix.Juggernaut, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 200, 0, 0, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Wormhole, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 200, 0, 0, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Reflect, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 200, 0, 0, 0),
                }
                ));

            // Arcane element - magenta
            AffixDecorators.Add(MonsterAffix.Arcane, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 120, 0, 120, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Jailer, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 120, 0, 120, 0),
                }
                ));

            // Fire element	- brown
            AffixDecorators.Add(MonsterAffix.FireChains, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 170, 50, 0, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Mortar, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 170, 50, 0, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Molten, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 170, 50, 0, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Desecrator, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 170, 50, 0, 0),
                }
                ));

            // Cold element - dark blue
            AffixDecorators.Add(MonsterAffix.Frozen, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 120, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.FrozenPulse, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 120, 0),
                }
                ));

            // Lightning element - blue
            AffixDecorators.Add(MonsterAffix.Orbiter, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 40, 40, 240, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Thunderstorm, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 40, 40, 240, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Electrified, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 40, 40, 240, 0),
                }
                ));
            // Shielding - orange
            AffixDecorators.Add(MonsterAffix.Shielding, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 255, 128, 80, 0),
                }
                ));
				
				AffixDecorators.Add(MonsterAffix.HealthLink, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));

            // Illusionist - teal
            AffixDecorators.Add(MonsterAffix.Illusionist, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 128, 128, 0),
                }
                ));

            // Others - grey
            AffixDecorators.Add(MonsterAffix.Waller, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Teleporter, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.ExtraHealth, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Fast, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Knockback, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Nightmarish, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Vampiric, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Vortex, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Avenger, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.Horde, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));
            AffixDecorators.Add(MonsterAffix.MissileDampening, new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BorderBrush = BorderBrush,
                    TextFont = LabelFont,
                    BackgroundBrush = Hud.Render.CreateBrush(255, 50, 50, 50, 0),
                }
                ));

            EliteHealthDecorator = new GroundLabelDecorator(Hud)
            {
                BorderBrush = BorderBrush,
                TextFont = LabelFont,
                BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0),
            };
        }

        public void Customize()
        {
            Hud.TogglePlugin<EliteMonsterAffixPlugin>(false);
        }


        public void PaintWorld(WorldLayer layer)
        {
            var monsters = Hud.Game.AliveMonsters;
            foreach (var monster in monsters)
            {
                if (monster.Rarity != ActorRarity.Champion && monster.Rarity != ActorRarity.Rare) continue;
                if (monster.SummonerAcdDynamicId != 0) continue;

                if (layer == WorldLayer.Ground)
                {
                    var hpValue = (monster.CurHealth * 100 / monster.MaxHealth).ToString("f0") + "%"; // code for HP - working, but not as intended
                    EliteHealthDecorator.Paint(monster, monster.FloorCoordinate, hpValue); // code for HP - working, but not as intended
                }

                foreach (var snoMonsterAffix in monster.AffixSnoList)
                {
                    WorldDecoratorCollection decorator;
                    if (!AffixDecorators.TryGetValue(snoMonsterAffix.Affix, out decorator)) continue;

                    string affixName = null;
                    if (CustomAffixNames.ContainsKey(snoMonsterAffix.Affix))
                    {
                        affixName = CustomAffixNames[snoMonsterAffix.Affix];
                    }
                    else affixName = snoMonsterAffix.NameLocalized;

                    decorator.Paint(layer, monster, monster.FloorCoordinate, affixName);
                }
            }
        }

    }

}