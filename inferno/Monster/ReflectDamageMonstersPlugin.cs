using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{

    public class ReflectDamageMonstersPlugin : BasePlugin, IInGameWorldPainter
	{

        public WorldDecoratorCollection reflectdamagemonsterDecorator { get; set; }
		public float RFoffset { get; set; }
		public float RadiusOffset { get; set; }
		public bool ShowGroundLabel { get; set; }
		public bool ShowMiniMapLabel { get; set; }
		public bool Up { get; set; }
		public string RFLabel, monsterG2Label, monsterG3Label, monsterG4Label, monsterG1_1, monsterG1_2, monsterG1_3, monsterG1_4, monsterG2_1, monsterG2_2, monsterG2_3, monsterG2_4, 
						monsterG3_1, monsterG3_2, monsterG3_3, monsterG3_4, monsterG4_1, monsterG4_2, monsterG4_3, monsterG4_4,
						RFLabelColour, monsterG2LabelColour, monsterG3LabelColour, monsterG4LabelColour, monsterGroupLabel, monsterGroupColour,
						affixLabel_1, affixLabel_2, affixLabel_3, affixname_1, affixname_2, affixname_3, affixLabel_1Colour, affixLabel_2Colour, affixLabel_3Colour;						
		public float groundLabelsize, mapLabelsize, groundfontsize, mapfontsize, groundLabelsizeG1, groundLabelsizeG2, groundLabelsizeG3, groundLabelsizeG4, 
						groundLabelsizeaffix1, groundLabelsizeaffix2, groundLabelsizeaffix3, groundLabelsizeaffix4,
						mapLabelsizeG1, mapLabelsizeG2, mapLabelsizeG3, mapLabelsizeG4, mapLabelsizeaffix1, mapLabelsizeaffix2, mapLabelsizeaffix3;
		private int a, r, g, b;
		private List<uint> reflectdamagemonsters { get; set; }
		private Dictionary<MonsterAffix, string> affixEnglishName;
		
        public ReflectDamageMonstersPlugin()
		{
            Enabled = true;
			Order = 20100;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);
 
			ShowGroundLabel = true; //GroundLabel on, off
			ShowMiniMapLabel = true; //MiniMapLabel on, off
			// Offset
			RFoffset = -2.0f; // Customize ★GroundLabel offset 
			RadiusOffset = 15.0f; // Customize ★Label minimap offset
			// Label
			RFLabel = "‍💥"; // Customize ReflectDamageMonsters' Label + monsterG1 Label
			monsterG2Label = "‍🛑👻🛑"; // Customize Label
			monsterG3Label = "‍🛑";
			monsterG4Label = "💥";
			affixLabel_1 = "";
			affixLabel_2 = "";
			affixLabel_3 = "";
			// Label Colour
			RFLabelColour = "yellow"; // Customize Label colour  //white, yellow, red, green, blue, blueviolet, orange, black, fuchsia, gold, deeppink. Choose from these colors.
			monsterG2LabelColour = "deeppink";  
			monsterG3LabelColour = "yellow";
			monsterG4LabelColour = "orange";
			affixLabel_1Colour = "white"; // // Customize the Label colour of EliteAffix //white, yellow, red, green, blue, blueviolet, orange, black, fuchsia, gold, deeppink. Choose from these colors.
			affixLabel_2Colour = "white";
			affixLabel_3Colour = "white";
			// Label Size
			groundLabelsizeG1 = 12.0f; // Customize groundLabelsize
			groundLabelsizeG2 = 8.0f;
			groundLabelsizeG3 = 15.0f;
			groundLabelsizeG4 = 12.0f;
			groundLabelsizeaffix1 = 12.0f; // Customize the groundLabelsize of EliteAffix
			groundLabelsizeaffix2 = 12.0f;;
			groundLabelsizeaffix3 = 12.0f;;
			mapLabelsizeG1 = 8.0f; // Customize MapLabelsize 
			mapLabelsizeG2 = 4.0f;
			mapLabelsizeG3 = 8.0f;
			mapLabelsizeG4 = 8.0f;
			mapLabelsizeaffix1 = 8.0f; // Customize the MapLabelsize of EliteAffix
			mapLabelsizeaffix2 = 8.0f;
			mapLabelsizeaffix3 = 8.0f;
			
			// Monster Group 1 that wants to label. monsterG1Label uses ReflectDamageMonsters'  label
			monsterG1_1 = ""; // Insert the Name of the monster. you can use English or your default game language
			monsterG1_2 = "";  
			monsterG1_3 = "Spewing Horror";      
			monsterG1_4 = "Retching Cadaver";
            // Monster Group 2 that wants to label. 
			monsterG2_1 = "Unholy Thrall";  // monsterGroup 2 Label Monsters
			monsterG2_3 = "";
			monsterG2_4 = "";
			// Monster Group 3 that wants to label. 
			monsterG3_1 = "Tomb Guardian";  // monsterGroup 3 Label Monsters
			monsterG3_2 = "Vengeful Summoner";
			monsterG3_3 = "Returned Summoner";
			monsterG3_4 = "Tortured Summoner";
			// Monster Group 4 that wants to label. 
			monsterG4_1 = "Vicious Magewraith";  // monsterGroup 4 Label Monsters
			monsterG4_2 = "";
			monsterG4_3 = "";
			monsterG4_4 = "";
			// Elite Affix Label
			affixname_1 = "Wormhole"; // Insert the the Affix of Elite. you can use English or your default game language. ex) Wormhole, Juggernaut, Illusionist
			affixname_2 = "Juggernaut";
			affixname_3 = "Illusionist";
 			Up = true;

            reflectdamagemonsters = new List<uint>()
            {                
             116075, //Sand Dweller - adventure mode
             26245, //Sand Dweller
             116305, //Sand Dweller
             196846, //Sand Dweller
             26066, //Dune Dervish
            };

			affixEnglishName = new Dictionary<MonsterAffix, string>();
            affixEnglishName.Add(MonsterAffix.Illusionist, "");
            affixEnglishName.Add(MonsterAffix.Wormhole, "");
            affixEnglishName.Add(MonsterAffix.Juggernaut, "");
            affixEnglishName.Add(MonsterAffix.Arcane, "");
            affixEnglishName.Add(MonsterAffix.Molten, "Molten");
            affixEnglishName.Add(MonsterAffix.Desecrator, "");
            affixEnglishName.Add(MonsterAffix.Electrified, "");
            affixEnglishName.Add(MonsterAffix.FireChains, "");
            affixEnglishName.Add(MonsterAffix.Frozen, "");
            affixEnglishName.Add(MonsterAffix.FrozenPulse, "");
            affixEnglishName.Add(MonsterAffix.Jailer, "");
            affixEnglishName.Add(MonsterAffix.Mortar, "Mortar");
            affixEnglishName.Add(MonsterAffix.Orbiter, "");
            affixEnglishName.Add(MonsterAffix.Plagued, "");
            affixEnglishName.Add(MonsterAffix.Poison, "");
            affixEnglishName.Add(MonsterAffix.Reflect, "");
            affixEnglishName.Add(MonsterAffix.Thunderstorm, "");
            affixEnglishName.Add(MonsterAffix.Waller, "Waller");
            affixEnglishName.Add(MonsterAffix.Fast, "");
            affixEnglishName.Add(MonsterAffix.Knockback, "");
            affixEnglishName.Add(MonsterAffix.Nightmarish, "");
            affixEnglishName.Add(MonsterAffix.Shielding, "");
            affixEnglishName.Add(MonsterAffix.Teleporter, "");
            affixEnglishName.Add(MonsterAffix.Vampiric, "");
            affixEnglishName.Add(MonsterAffix.Vortex, "");
            affixEnglishName.Add(MonsterAffix.Avenger, "");
            affixEnglishName.Add(MonsterAffix.Horde, "");
			affixEnglishName.Add(MonsterAffix.ExtraHealth, "");
			affixEnglishName.Add(MonsterAffix.HealthLink, "");
            affixEnglishName.Add(MonsterAffix.MissileDampening, "");			
        }


        public void PaintWorld(WorldLayer layer)
        {
            var monsters = Hud.Game.AliveMonsters.Where(m => ((m.SummonerAcdDynamicId == 0 && m.IsElite) || !m.IsElite)) ;
			foreach (var monster in monsters)
				{
					var RFmonster = monster.SnoMonster.NameLocalized == "Vicious Magewraith" || monster.SnoMonster.NameEnglish == "Vicious Magewraith";
					var monsterG1 = monster.SnoMonster.NameLocalized == monsterG1_1 || monster.SnoMonster.NameEnglish == monsterG1_1 || monster.SnoMonster.NameLocalized == monsterG1_2 || monster.SnoMonster.NameEnglish == monsterG1_2 || monster.SnoMonster.NameLocalized == monsterG1_3 || monster.SnoMonster.NameEnglish == monsterG1_3 || monster.SnoMonster.NameLocalized == monsterG1_4 || monster.SnoMonster.NameEnglish == monsterG1_4;
					var	scanmonsterG1 = reflectdamagemonsters.Contains(monster.SnoMonster.Sno) || RFmonster || monsterG1;
					var scanmonsterG2 = monster.SnoMonster.NameLocalized == monsterG2_1 || monster.SnoMonster.NameEnglish == monsterG2_1 || monster.SnoMonster.NameLocalized == monsterG2_2 || monster.SnoMonster.NameEnglish == monsterG2_2 || monster.SnoMonster.NameLocalized == monsterG2_3 || monster.SnoMonster.NameEnglish == monsterG2_3 || monster.SnoMonster.NameLocalized == monsterG2_4 || monster.SnoMonster.NameEnglish == monsterG2_4;
					var scanmonsterG3 = monster.SnoMonster.NameLocalized == monsterG3_1 || monster.SnoMonster.NameEnglish == monsterG3_1 || monster.SnoMonster.NameLocalized == monsterG3_2 || monster.SnoMonster.NameEnglish == monsterG3_2 || monster.SnoMonster.NameLocalized == monsterG3_3 || monster.SnoMonster.NameEnglish == monsterG3_3 || monster.SnoMonster.NameLocalized == monsterG3_4 || monster.SnoMonster.NameEnglish == monsterG3_4;
					var scanmonsterG4 = monster.SnoMonster.NameLocalized == monsterG4_1 || monster.SnoMonster.NameEnglish == monsterG4_1 || monster.SnoMonster.NameLocalized == monsterG4_2 || monster.SnoMonster.NameEnglish == monsterG4_2 || monster.SnoMonster.NameLocalized == monsterG4_3 || monster.SnoMonster.NameEnglish == monsterG4_3 || monster.SnoMonster.NameLocalized == monsterG4_4 || monster.SnoMonster.NameEnglish == monsterG4_4;

				if (scanmonsterG1)
					{
						monsterGroupLabel = RFLabel;
						monsterGroupColour = RFLabelColour;
						groundLabelsize = groundLabelsizeG1;
						mapLabelsize = mapLabelsizeG1;
						SelectFont();
						drawLabel(layer,monster);
					}					
				if (scanmonsterG2)
					{
						monsterGroupLabel = monsterG2Label;
						monsterGroupColour = monsterG2LabelColour;
						groundLabelsize = groundLabelsizeG2;
						mapLabelsize = mapLabelsizeG2;
						SelectFont();
						drawLabel(layer,monster);
					}
				if (scanmonsterG3)
					{
						monsterGroupLabel = monsterG3Label;
						monsterGroupColour = monsterG3LabelColour;
						groundLabelsize = groundLabelsizeG3;
						mapLabelsize = mapLabelsizeG3;
						SelectFont();
						drawLabel(layer,monster);
					}
				if (scanmonsterG4)
					{
						monsterGroupLabel = monsterG4Label;
						monsterGroupColour = monsterG4LabelColour;
						groundLabelsize = groundLabelsizeG4;
						mapLabelsize = mapLabelsizeG4;
						SelectFont();
						drawLabel(layer,monster);
					}
				if (monster.IsElite)
					{
						drawAffixLabel(layer,monster);
					}
				monsterGroupLabel = null;
				}
        }

		public void drawAffixLabel(WorldLayer layer, IMonster monster )
		{

			if (monster.SummonerAcdDynamicId == 0 && monster.IsElite && monster.Rarity != ActorRarity.RareMinion)
			{
			foreach (var snoMonsterAffix in monster.AffixSnoList)
				{
					string affixName = null;
                    if (affixEnglishName.ContainsKey(snoMonsterAffix.Affix))
						affixEnglishName.TryGetValue(snoMonsterAffix.Affix, out affixName);
                    				
				if (affixName == affixname_1 || snoMonsterAffix.NameLocalized == affixname_1)  
					{	
						monsterGroupLabel = affixLabel_1;
						monsterGroupColour = affixLabel_1Colour;
						groundLabelsize = groundLabelsizeaffix1;
						mapLabelsize = mapLabelsizeaffix1;
						SelectFont();
						drawLabel(layer,monster); 
						return;
					}
				else if (affixName == affixname_2 || snoMonsterAffix.NameLocalized == affixname_2) 
					{	
						monsterGroupLabel = affixLabel_2;
						monsterGroupColour = affixLabel_2Colour;
						groundLabelsize = groundLabelsizeaffix2;
						mapLabelsize = mapLabelsizeaffix2;
						SelectFont();
						drawLabel(layer,monster);
						return;
					}
				else if (affixName == affixname_3 || snoMonsterAffix.NameLocalized == affixname_3)
					{
						monsterGroupLabel = affixLabel_3;
						monsterGroupColour = affixLabel_3Colour;
						groundLabelsize = groundLabelsizeaffix3;
						mapLabelsize = mapLabelsizeaffix3;
						SelectFont();
						drawLabel(layer,monster);
						return;
					}
				}			
			}
		}

		public void drawLabel(WorldLayer layer, IMonster monster )
		{
		if (ShowGroundLabel)
			{
				reflectdamagemonsterDecorator = new WorldDecoratorCollection(
				new GroundLabelDecorator(Hud)
				{
					TextFont = Hud.Render.CreateFont("tahoma", groundfontsize, a, r, g, b, true, false, false),
				});
				reflectdamagemonsterDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, RFoffset), monsterGroupLabel);
			}
		if (ShowMiniMapLabel)
			{
				var layout = Hud.Render.CreateFont("tahoma", mapfontsize, a, r, g, b, true, false, false).GetTextLayout(monsterGroupLabel);
				float mapX, mapY;
				Hud.Render.GetMinimapCoordinates(monster.FloorCoordinate.X, monster.FloorCoordinate.Y, out mapX, out mapY);
				Hud.Render.CreateFont("tahoma", mapfontsize, a, r, g, b, true, false, false).DrawText(layout, mapX - layout.Metrics.Width / 2, mapY - RadiusOffset);
			}
		}	
		
		public void SelectFont()
		{
			if (monsterGroupColour == "red")
				{
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 0; b = 0; return;
				}
			else if(monsterGroupColour == "green")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 0; g = 128; b = 0; return;
				}
			else if (monsterGroupColour == "blue")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 0; g = 0; b = 255; return;
				}
			else if (monsterGroupColour == "blueviolet")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 138; g = 43; b = 226; return;
				}
			else if (monsterGroupColour == "yellow")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 255; b = 0; return;
				}
			else if (monsterGroupColour == "orange")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r =255; g = 165; b = 0; return;
				}
			else if (monsterGroupColour == "fuchsia")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 0; b = 255; return;
				}
			else if (monsterGroupColour == "black")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 0; g = 0; b = 0; return;
				}
			else if (monsterGroupColour == "gold")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 215; b = 0; return;
				}
			else if (monsterGroupColour == "deeppink")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 20; b = 255; return;
				}
			else if (monsterGroupColour == "white")
				{				
					groundfontsize = groundLabelsize; mapfontsize = mapLabelsize; a = 255; r = 255; g = 255; b = 255; return;
				}
		}
    }
}

//֍֎♚♛♜♝♞♟☻⚉⚑♟◙◉😈👹💀☠👻💥☣⚛🔆🏴‍☠️🛑 