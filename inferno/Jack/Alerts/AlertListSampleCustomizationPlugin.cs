// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
// *.txt files are not loaded automatically by TurboHUD
// you have to change this file's extension to .cs to enable it
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

namespace Turbo.Plugins.inferno.Jack.Alerts
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Turbo.Plugins.Default;
    using Turbo.Plugins.inferno.Jack.Models;

    public class AlertListSampleCustomizationPlugin : BasePlugin, ICustomizer
    {
        public AlertListSampleCustomizationPlugin()
        {
            Enabled = true;
        }

        public void Customize()
        {

            Hud.RunOnPlugin<inferno.Jack.Alerts.PlayerTopAlertListPlugin>(plugin =>
            {
                plugin.AlertList.Alerts.Add(new inferno.Jack.Alerts.Alert(Hud)
                {
                    MessageFormat = "!!! Goblin !!!",
                    Rule =
                    {
                        CustomCondition = player =>
                        {
                            if (Hud.Game.Me.IsInTown) return false;
                            return Hud.Game.AliveMonsters.Any(m => m.SnoMonster.Priority == MonsterPriority.goblin);
                        },
                    },
                    Label =
					{
						TextFont = Hud.Render.CreateFont("tahoma", 12, 255, 30, 244, 30, false, false, 242, 0, 0, 0, true),
					}
                });
            });

            Hud.RunOnPlugin<PlayerBottomAlertListPlugin>(plugin =>
            {
                var list = plugin.AlertList;
                list.Up = false; // up or down
                list.RatioY = 0.6f;
                list.RatioWidth = 0.2f; // based on screen height to preserve proportions
                list.RatioX = 0.5f; // center horizontally

                // Oculus
                list.Alerts.Add(new Alert(Hud, HeroClass.None)
                {
                    //AlertTextFunc = (id) => "Oculus",
                    TextSnoId = 3563390301,
                    MessageFormat = "આ {0} આ",//⚔
                    Rule =
                    {
                        ActiveBuffs = new [] { new SnoPowerId(402461, 2) },
                    },
                    Label =
                    {
                        TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 244, 244, 244, false, false, 242, 0, 0, 0, true),
                        BackgroundBrush = Hud.Render.CreateBrush(100, 255, 255, 50, 0),
                        BorderBrush = Hud.Render.CreateBrush(178, 0, 0, 0, 1)
                    },
                });
            });

            //
            var itemsIds = new HashSet<uint>() { 1844495708 };
            Hud.RunOnPlugin<PlayerTopAlertListPlugin>(plugin =>
            {
                plugin.AlertList.Alerts.Add(new Alert(Hud)
                {
                    MessageFormat = "🎁 {0} 🎁",//🎁
                    AlertTextFunc = (id) =>
                    {
                        var items = Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.Unidentified/**/ && itemsIds.Contains(item.SnoItem.Sno));

                        if (items.Count() > 1)
                            return string.Join(", ", items.GroupBy(k => k.FullNameLocalized).Select(item => string.Format(CultureInfo.InvariantCulture, "{0} {1}", item.Count(), item.Key)));

                        return items.Count() == 1 ? items.First().FullNameLocalized : string.Empty;
                    },
                    Rule =
                    {
                        ShowInTown = true,
                        VisibleCondition = (player) => Hud.Game.Items.Any(item => item.Location == ItemLocation.Floor && item.Unidentified/**/ && itemsIds.Contains(item.SnoItem.Sno)),
                    }
                });
            });

            // loot list
            Hud.RunOnPlugin<MinimapLeftAlertListPlugin>(plugin =>
            {
                var ancientRank = -1;
                plugin.AlertList.VerticalCenter = false;
                plugin.AlertList.RatioSpacerY = 0;

                plugin.AlertList.Alerts.Add(new Alert(Hud)
                {
                    MultiLine = true,
                    LinesFunc = () =>
                    {
                        return Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.Unidentified && item.SetSno != uint.MaxValue && item.AncientRank > ancientRank).Select(item => string.Format(CultureInfo.InvariantCulture, "🎁 {0} 🎁", item.FullNameLocalized));
                    },
                    Label =
                    {
                        TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 0, 170, 0, false, false, true),
                    },
                    Rule =
                    {
                        ShowInTown = true,
                        VisibleCondition = (player) => Hud.Game.Items.Any(item => item.Location == ItemLocation.Floor && item.Unidentified && item.SetSno != uint.MaxValue && item.AncientRank > ancientRank),
                    }
                });

                plugin.AlertList.Alerts.Add(new Alert(Hud)
                {
                    MultiLine = true,
                    LinesFunc = () =>
                    {
                        return Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.Unidentified && item.SetSno == uint.MaxValue && item.AncientRank > ancientRank).Select(item => string.Format(CultureInfo.InvariantCulture, "🎁 {0} 🎁", item.FullNameLocalized));
                    },
                    Label =
                    {
                        TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 235, 120, 0, false, false, true),
                    },
                    Rule =
                    {
                        ShowInTown = true,
                        VisibleCondition = (player) => Hud.Game.Items.Any(item => item.Location == ItemLocation.Floor && item.Unidentified && item.SetSno == uint.MaxValue && item.AncientRank > ancientRank),
                    }
                });
            });

            // Ancient Parthan Defenders
            Hud.RunOnPlugin<Jack.Alerts.PlayerLeftAlertListPlugin>(plugin =>
            {
                plugin.AlertList.Alerts.Add(new Jack.Alerts.Alert(Hud)
                {
                    AlertTextFunc = (id) =>
                    {
                        var count = Hud.Game.AliveMonsters.Count(m => (m.Stunned || m.Frozen) && m.NormalizedXyDistanceToMe <= 25);
                        var percent = 9;

                        if (Hud.Game.Me.CubeSnoItem2.LegendaryPower.Sno == Hud.Sno.SnoPowers.AncientParthanDefenders.Sno)
                        {
                            percent = 12;
                        }

                        return string.Format("{0} * {1}% = {2}%", count, percent, count * percent);
                    },
                    Label =
                    {
                        TextFont = Hud.Render.CreateFont("tahoma", 11, 255, 30, 244, 30, false, false, 242, 0, 0, 0, true),
                    },
                    Rule =
                    {
                        ShowOutOfCombat = false,
                        ActiveBuffs = new[] { new SnoPowerId(Hud.Sno.SnoPowers.AncientParthanDefenders.Sno), },
                        CustomCondition = (player) => Hud.Game.AliveMonsters.Count(m => (m.Stunned || m.Frozen) && m.NormalizedXyDistanceToMe <= 25) > 0,
                    }
                });

                plugin.AlertList.Alerts.Add(new Jack.Alerts.Alert(Hud)
                {
                    TextSnoId = 962282719,
                    MessageFormat = "⚠ {0} ⚠", //⚠
                    Rule =
                    {
                        ShowOutOfCombat = false,
                        ActiveBuffs = new[] { new SnoPowerId(Hud.Sno.SnoPowers.AncientParthanDefenders.Sno), },
                        CustomCondition = (player) => Hud.Game.AliveMonsters.Count(m => (m.Stunned || m.Frozen) && m.NormalizedXyDistanceToMe <= 25) == 0,
                    }
                });
            });
        }
    }
}