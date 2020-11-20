using SharpDX.DirectInput;
using System.Linq;
using System;
using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{
    using System.Text;

    // by 我想静静 黑白灰 小米 Jack Céparou
    public class MonstersCountPlugin : BasePlugin, IInGameTopPainter, IInGameWorldPainter, IKeyEventHandler
    {
        public IFont DefaultTextFont { get; set; }
		public IFont GreenTextFont { get; set; }
        public IFont OrangeTextFont { get; set; }
        public IFont RedTextFont { get; set; }
        public WorldDecoratorCollection StatisticalRangeDecorator { get; set; }
        public WorldDecoratorCollection MaxStatisticalRangeDecorator { get; set; }
        private float currentYard;
        public float BaseYard
        {
            get { return baseMapShapeDecorator.Radius; }
            set {
                baseMapShapeDecorator.Radius = value;
                currentYard = BaseYard;
            }
        }
        public float MaxYard
        {
            get { return maxMapShapeDecorator.Radius; }
            set { maxMapShapeDecorator.Radius = value; }
        }
        public bool ShowCircle { get; set; }
        public IKeyEvent ToggleKeyEvent { get; set; }
        public bool ShowMonstersCount { get; set; }

        public bool ShowTotalProgression { get; set; }
        public bool ShowTrashProgression { get; set; }
        public bool ShowEliteProgression { get; set; }
        public bool ShowRareMinionProgression { get; set; }
        public bool ShowRiftGlobeProgression { get; set; }

        public bool ShowCursedCount { get; set; }
        public bool ShowTime { get; set; }
        public bool ToggleEnable { get; set; }
        public float XWidth { get; set; }
        public float YHeight { get; set; }

        private IFont currentFont;
        private bool TurnedOn;

        private MapShapeDecorator baseMapShapeDecorator;
        private MapShapeDecorator maxMapShapeDecorator;

        private StringBuilder textBuilder;

        public MonstersCountPlugin()
        {
            Enabled = true;
            ShowCircle = true;
            ToggleEnable = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            ToggleKeyEvent = Hud.Input.CreateKeyEvent(true, Key.LeftControl, true, false, false);
            DefaultTextFont = Hud.Render.CreateFont("tahoma", 9, 255, 180, 147, 109, false, false, 250, 0, 0, 0, true);
			GreenTextFont = Hud.Render.CreateFont("tahoma", 9, 255, 180, 255, 109, false, false, 250, 0, 0, 0, true);
            OrangeTextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 128, 0, false, false, 250, 0, 0, 0, true);
            RedTextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 0, 0, false, false, 250, 0, 0, 0, true);

            TurnedOn = false;
            ShowMonstersCount = true;
            ShowTotalProgression = true;
            ShowTrashProgression = true;
            ShowEliteProgression = true;
            ShowRareMinionProgression = true;
            ShowRiftGlobeProgression = true;
            ShowTime = true;
            ShowCursedCount = true;

            XWidth = 0.84f;
            YHeight = 0.750f;//0.627f;	//0.61f
            textBuilder = new StringBuilder();

            baseMapShapeDecorator = new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 180, 147, 109, 1),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = 40,
            };
            StatisticalRangeDecorator = new WorldDecoratorCollection(baseMapShapeDecorator);
            currentYard = BaseYard;
            maxMapShapeDecorator = new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 180, 147, 109, 1),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = 120,
            };
            MaxStatisticalRangeDecorator = new WorldDecoratorCollection(maxMapShapeDecorator);
        }

        public void OnKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.IsPressed && ToggleKeyEvent.Matches(keyEvent) && ToggleEnable)
            {
                TurnedOn = !TurnedOn;
                currentYard = TurnedOn ? MaxYard : BaseYard;
            }
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.IsInTown || !ShowCircle) return;
            if (TurnedOn)
            {
                MaxStatisticalRangeDecorator.Paint(layer, null, Hud.Game.Me.FloorCoordinate, null);
            }
            else
            {
                StatisticalRangeDecorator.Paint(layer, null, Hud.Game.Me.FloorCoordinate, null);
            }
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            var inRift = Hud.Game.SpecialArea == SpecialArea.Rift || Hud.Game.SpecialArea == SpecialArea.GreaterRift;
            var inGR = Hud.Game.SpecialArea == SpecialArea.GreaterRift;
            if (DefaultTextFont == null)
            {
                return;
            }

            double totalMonsterRiftProgression = 0;
            double TrashMonsterRiftProgression = 0;
            double EliteProgression = 0;
            double RareMinionProgression = 0;
            double RiftGlobeProgression = 0;
			double curseHealth = 0;
			double[] CondiAdjustment = new double[] {0,0,0,0,0,0,0};

            int monstersCount = 0;
            int EliteCount = 0;
            int RareMinionCount = 0;
            int ElitePackCount = 0;
            // Frailty
            int FrailtyCount = 0;
            int EliteFrailtyCount = 0;
            // Leech
            int LeechCount = 0;
            int EliteLeechCount = 0;
            // Decrepify
            int DecrepifyCount = 0;
            int EliteDecrepifyCount = 0;

            float XPos = Hud.Window.Size.Width * XWidth;
            float YPos = Hud.Window.Size.Height * YHeight;

            var monsters = Hud.Game.AliveMonsters.Where(m => ((m.SummonerAcdDynamicId == 0 && m.IsElite) || !m.IsElite) && m.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= currentYard);
            Dictionary<IMonster, string> eliteGroup = new Dictionary<IMonster, string>();
            foreach (var monster in monsters)
            {
                var Elite = monster.Rarity == ActorRarity.Rare || monster.Rarity == ActorRarity.Champion || monster.Rarity == ActorRarity.Unique || monster.Rarity == ActorRarity.Boss;
                monstersCount++;
				
				if (inRift)
				{
                    double monsterHP = monster.CurHealth;
                    var curse = monster.GetAttributeValue(Hud.Sno.Attributes.Power_Buff_2_Visual_Effect_None, 471845);//471845	1	power: Frailty
                    if (curse == 1)
                    {
                        monsterHP -= monster.MaxHealth * 0.15;
                    }
					if(monsterHP<=0) monsterHP = 0;
					curseHealth += monsterHP;

                    //if there are less than 7 enemies artificially increase the health to compensate less ticks on condi
                    for (int k = 0; k < 7; k++)
                    {
                        if (monsterHP > CondiAdjustment[k])
                        {
                            for (int j = 6; j > k; j--)
                            {
                                CondiAdjustment[j] = CondiAdjustment[j - 1];
                            }
                            CondiAdjustment[k] = monsterHP;
                            break;
                        }
                    }

                }
				
                if (!monster.IsElite)
                {
                    if (inRift) TrashMonsterRiftProgression += monster.SnoMonster.RiftProgression * 100.0d / this.Hud.Game.MaxQuestProgress;
                }
                else
                {
                    if (monster.Rarity == ActorRarity.RareMinion)
                    {
                        RareMinionCount++;
                        if (inRift) RareMinionProgression += monster.SnoMonster.RiftProgression * 100.0d / this.Hud.Game.MaxQuestProgress;
                    }
                    else
                    {
                        if (monster.Rarity == ActorRarity.Unique || monster.Rarity == ActorRarity.Boss)
                        {
                            EliteCount++;
                            ElitePackCount++;
                        }

                        if (monster.Rarity == ActorRarity.Champion)
                        {
                            EliteCount++;
                            eliteGroup.Add(monster, String.Join(", ", monster.AffixSnoList));
                            if (inRift) EliteProgression += monster.SnoMonster.RiftProgression * 100.0d / this.Hud.Game.MaxQuestProgress;
                        }

                        if (monster.Rarity == ActorRarity.Rare)
                        {
                            EliteCount++;
                            ElitePackCount++;
                            if (inRift)
                            {
                                EliteProgression += 4 * 1.15d;
                                EliteProgression += monster.SnoMonster.RiftProgression * 100.0d / this.Hud.Game.MaxQuestProgress;
                            }
                        }
                    }
                }
                var test = monster.GetAttributeValue(Hud.Sno.Attributes.Power_Buff_2_Visual_Effect_None, 471845);//471845	1	power: Frailty
                if (test == -1)
                {
                    if (ShowCursedCount)
                    {
                        FrailtyCount++;
                        if (Elite) EliteFrailtyCount++;
                    }
                }
                test = monster.GetAttributeValue(Hud.Sno.Attributes.Power_Buff_2_Visual_Effect_None, 471869);//471869	1	power: Leech
                if (test == -1)
                {
                    if (ShowCursedCount)
                    {
                        LeechCount++;
                        if (Elite) EliteLeechCount++;
                    }
                }
                test = monster.GetAttributeValue(Hud.Sno.Attributes.Power_Buff_2_Visual_Effect_None, 471738);//471738	1	power: Decrepify
                if (test == -1)
                {
                    if (ShowCursedCount)
                    {
                        DecrepifyCount++;
                        if (Elite) EliteDecrepifyCount++;
                    }
                }
            }
            Dictionary<IMonster, string> eliteGroup1 = eliteGroup.OrderBy(p => p.Value).ToDictionary(p => p.Key, o => o.Value);
            //if (monstersCount == 0) return;
            var actors = Hud.Game.Actors.Where(x => x.SnoActor.Kind == ActorKind.RiftOrb);
            foreach (var actor in actors)
            {
                RiftGlobeProgression += 1.15d;
            }
            string preStr = null;
            foreach (var elite in eliteGroup1)
            {
                if (elite.Key.Rarity == ActorRarity.Champion)
                {
                    if (preStr != elite.Value)
                    {

                        EliteProgression += 3 * 1.15f;
                        ElitePackCount++;
                    }
                    preStr = elite.Value;
                }
            }
            textBuilder.Clear();
            if (ShowMonstersCount && !inRift)
            {
                textBuilder.AppendFormat("Monsters: {0}", monstersCount);
                textBuilder.AppendLine();
                if (EliteCount > 0) textBuilder.AppendFormat("Elite: {0}(Pack: {1})", EliteCount, ElitePackCount);
                if (RareMinionCount > 0) textBuilder.AppendFormat("Minion: {0}", RareMinionCount);
                textBuilder.AppendLine();
            }

            if (inRift)
            {
                totalMonsterRiftProgression = TrashMonsterRiftProgression + EliteProgression + RareMinionProgression + RiftGlobeProgression;
                long totalTime = (long)totalMonsterRiftProgression * 90000000;
                long TrashTime = (long)TrashMonsterRiftProgression * 90000000;
                long EliteTime = (long)EliteProgression * 90000000;
                long RareMinionTime = (long)RareMinionProgression * 90000000;
                long RiftGlobeTime = (long)RiftGlobeProgression * 90000000;



                if (ShowMonstersCount)
                {
                    textBuilder.AppendFormat("Monsters: {0} TotalRP: {1}%", monstersCount, (totalMonsterRiftProgression + Hud.Game.RiftPercentage).ToString("f2"));
                    textBuilder.AppendLine();
                    if (EliteCount > 0) textBuilder.AppendFormat("Elite: {0}(Pack: {1})", EliteCount, ElitePackCount);
                    if (RareMinionCount > 0) textBuilder.AppendFormat("Minion: {0}", RareMinionCount);
                    textBuilder.AppendLine();
                }

                if (totalMonsterRiftProgression > 0 && ShowTotalProgression)
                {
                    if (ShowTime && inGR)
                    {
                        textBuilder.AppendFormat("TotalRP: {0}% = {1}", totalMonsterRiftProgression.ToString("f2"), ValueToString((long)totalTime, ValueFormat.LongTime));
                    }
                    else
                    {
                        textBuilder.AppendFormat("TotalRP: {0}%", totalMonsterRiftProgression.ToString("f2"));
                    }
                    textBuilder.AppendLine();
                }

                if (TrashMonsterRiftProgression > 0 && ShowTrashProgression)
                {
                    if (ShowTime && inGR)
                    {
                        textBuilder.AppendFormat("TrashRP: {0}% = {1}", TrashMonsterRiftProgression.ToString("f2"), ValueToString((long)TrashTime, ValueFormat.LongTime));
                    }
                    else
                    {
                        textBuilder.AppendFormat("TrashRP: {0}%", TrashMonsterRiftProgression.ToString("f2"));
                    }
                    textBuilder.AppendLine();
                }
                if (EliteProgression > 0 && ShowEliteProgression)
                {
                    if (ShowTime && inGR)
                    {
                        textBuilder.AppendFormat("EliteRP: {0}% = {1}", EliteProgression.ToString("f2"), ValueToString((long)EliteTime, ValueFormat.LongTime));
                    }
                    else
                    {
                        textBuilder.AppendFormat("EliteRP: {0}%", EliteProgression.ToString("f2"));
                    }
                    textBuilder.AppendLine();
                }
                if (RareMinionProgression > 0 && ShowRareMinionProgression)
                {
                    if (ShowTime && inGR)
                    {
                        textBuilder.AppendFormat("MinionRP: {0}% = {1}", RareMinionProgression.ToString("f2"), ValueToString((long)RareMinionTime, ValueFormat.LongTime));
                    }
                    else
                    {
                        textBuilder.AppendFormat("MinionRP: {0}%", RareMinionProgression.ToString("f2"));
                    }
                    textBuilder.AppendLine();
                }
                if (RiftGlobeProgression > 0 && ShowRiftGlobeProgression)
                {
                    if (ShowTime && inGR)
                    {
                        textBuilder.AppendFormat("GlobeRP: {0}% = {1}", RiftGlobeProgression.ToString("f2"), ValueToString((long)RiftGlobeTime, ValueFormat.LongTime));
                    }
                    else
                    {
                        textBuilder.AppendFormat("GlobeRP: {0}%", RiftGlobeProgression.ToString("f2"));
                    }
                    textBuilder.AppendLine();
                }
            }
            if (ShowCursedCount)
            {
                textBuilder.AppendFormat("Not Cursed: F{0}({1}) L{2}({3}) D{4}({5})", FrailtyCount, EliteFrailtyCount, LeechCount, EliteLeechCount, DecrepifyCount, EliteDecrepifyCount);
                textBuilder.AppendLine();
            }

			double condiDamage = 610248.7;
			
			if(Hud.Game.Me.InGreaterRift)
			{
                //adjust condi damage if you have <7 targets
                for (int i = 0; i < 6; i++)
                {
                    CondiAdjustment[i] = CondiAdjustment[i] - CondiAdjustment[i + 1];
                }
                for (int i = 0; i < 6; i++)
                {
                    curseHealth += CondiAdjustment[i] * (6 - i);
                }
                condiDamage *= 1463698.8;
				condiDamage /= Math.Pow(1.17,90);
				condiDamage *= Math.Pow(1.17,Hud.Game.Me.InGreaterRiftRank);
				condiDamage *= 0.4;

                curseHealth /= condiDamage;
				curseHealth /= 2;
				curseHealth /= 4;
                curseHealth /= 7;
				
			}
			
			
			textBuilder.AppendFormat("condi: {0:0.00}", curseHealth);
            textBuilder.AppendLine();

            //DEBUG
            //DEBUG
            //HexingPantsOfMrYan { get; } // 318817 - ItemPassive_Unique_Ring_635_x1
            var buff = Hud.Game.Me.Powers.GetBuff(318817);
            if ((buff != null) && (buff.IconCounts[0] > 0))
            {
                textBuilder.AppendFormat("Hexing: {0}", buff.IconCounts[1]);
                textBuilder.AppendLine();
            }

            int[] _skillOrder = { 2, 3, 4, 5, 0, 1 };
            foreach (var player in Hud.Game.Players)//others
            {
                if (player.HeroClassDefinition.HeroClass == HeroClass.Wizard)
                {
                    foreach (var i in _skillOrder)
                    {
                        var skill = player.Powers.SkillSlots[i];
                        if (skill == null || skill.SnoPower.Sno != 134872) continue; //Archon

                        textBuilder.AppendFormat("{0:0.00}", skill.Rune);
                    }
                }
            }


            if (totalMonsterRiftProgression >= (100.0d-5.83d) - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100  || totalMonsterRiftProgression >= 100d - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100 || TrashMonsterRiftProgression >= 100d - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100)
            {
				if (totalMonsterRiftProgression >= (100.0d-5.83d) - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100) currentFont = GreenTextFont;
                if (totalMonsterRiftProgression >= 100d - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100) currentFont = OrangeTextFont;
                if (TrashMonsterRiftProgression >= 100d - Hud.Game.RiftPercentage && Hud.Game.RiftPercentage != 100) currentFont = RedTextFont;
            }
            else
            {
                currentFont = DefaultTextFont;
            }
            var layout = currentFont.GetTextLayout(textBuilder.ToString());
            currentFont.DrawText(layout, XPos, YPos);
        }
    }
}