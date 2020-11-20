using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno.Plugins
{
    public class GargsMasterPlugin : BasePlugin, IInGameWorldPainter
    {
        public bool ShowInTown { get; set; }
        public WorldDecoratorCollection PlayerGargs { get; set; }
        public WorldDecoratorCollection OtherPlayersGargs { get; set; }
        public HashSet<ActorSnoEnum> GargSno = new HashSet<ActorSnoEnum>() 
        {
            (ActorSnoEnum)432690, 
            (ActorSnoEnum)432691, 
            (ActorSnoEnum)432692, 
            (ActorSnoEnum)432693, 
            (ActorSnoEnum)432694, 
            (ActorSnoEnum)122305, 
            (ActorSnoEnum)179776, 
            (ActorSnoEnum)171491, 
            (ActorSnoEnum)179778, 
            (ActorSnoEnum)171501, 
            (ActorSnoEnum)171502, 
            (ActorSnoEnum)179780, 
            (ActorSnoEnum)179779,
            (ActorSnoEnum)179772,
                    //wd
            (ActorSnoEnum)51353,    //WD_ZombieDog    Zombie Dog            //2 rune
            (ActorSnoEnum)103215,    //WD_ZombieDogRune_fire                //4 rune
            (ActorSnoEnum)105763,    //WD_ZombieDogRune_healthGlobe        //0 rune
            (ActorSnoEnum)110959,    //WD_ZombieDogRune_healthLink        //3 rune
            (ActorSnoEnum)103235,    //WD_ZombieDogRune_lifeSteal        //5    rune
            (ActorSnoEnum)103217,    //WD_ZombieDogRune_poison            //1 rune
            (ActorSnoEnum)87189,    //Fetish_Melee_A    Fetish Army            //0 //2 //3
            (ActorSnoEnum)89934,    //Fetish_Skeleton_A                        //1
            (ActorSnoEnum)409656,    //Fetish_Melee_fire                        //4
            (ActorSnoEnum)410238,    //Fetish_Melee_poison                    //5
            (ActorSnoEnum)409590,    //Fetish_Melee_Sycophants                //Belt    //passive
 
            //monk male
            (ActorSnoEnum)169904,    //Monk_male_mysticAlly    
            (ActorSnoEnum)169905,    //Monk_male_mysticAlly_alabaster    
            (ActorSnoEnum)169906,    //Monk_male_mysticAlly_crimson    
            (ActorSnoEnum)169908,    //Monk_male_mysticAlly_golden    
            (ActorSnoEnum)169907,    //Monk_male_mysticAlly_indigo    
            (ActorSnoEnum)169909,    //Monk_male_mysticAlly_obsidian
            //monk female
            (ActorSnoEnum)123885,    //Monk_female_mysticAlly    
            (ActorSnoEnum)169891,    //Monk_female_mysticAlly_alabaster    
           (ActorSnoEnum)168878,    //Monk_female_mysticAlly_crimson    
            (ActorSnoEnum)169123,    //Monk_female_mysticAlly_golden    
            (ActorSnoEnum)169890,    //Monk_female_mysticAlly_indigo    
            (ActorSnoEnum)169077,    //Monk_female_mysticAlly_obsidian    
            //runes active
            (ActorSnoEnum)367774,    //x1_Monk_female_mysticAllyMini_crimson        //feuerVerbündeter
            (ActorSnoEnum)363935,    //X1_projectile_mystically_runec_boulder    //roling stone    //OtherPlayersMysticAlly
 
            //dh
 
            //crus
            //nur bogenschützen && leibwächter mit PlayerPhalanx decorator
            (ActorSnoEnum).330728,    //x1_Crusader_Phalanx3_projectile                //no rune
            (ActorSnoEnum)369795,    //x1_Crusader_PhalanxArcher    Avatar of the Order    //bogenschützen
            //338598,    //x1_Crusader_Phalanx3_addProjectiles            //pfeile bogenschützen
            (ActorSnoEnum)357358,    //x1_Crusader_Phalanx3_projectile_chargers        //schildansturm
            (ActorSnoEnum)338678,    //x1_Crusader_Phalanx3_projectile_horse            //stampede
           (ActorSnoEnum)338807,    //x1_Crusader_Phalanx3_blocker                    //schildträger
            (ActorSnoEnum)345682,    //x1_Crusader_Phalanx    Avatar of the Order        //leibwächter
 
            (ActorSnoEnum)90443,    //Barbarian_CallOfTheAncients_1    Talic
           (ActorSnoEnum)90535,    //Barbarian_CallOfTheAncients_2    Korlic
            (ActorSnoEnum)90536,    //Barbarian_CallOfTheAncients_3    Madawc
 
            //3 heads = 3 Sno = 3 circle
            (ActorSnoEnum)81515,    //Wizard_HydraHead_Arcane_1    
            //81231,    //Wizard_HydraHead_Arcane_2    
            //81232,    ///Wizard_HydraHead_Arcane_3    
            (ActorSnoEnum)83959,    //Wizard_HydraHead_Big    Fire Hydra
            (ActorSnoEnum)80745,    //Wizard_HydraHead_Default_1    
            //80757,    //Wizard_HydraHead_Default_2    
            //80758,    //Wizard_HydraHead_Default_3    
            (ActorSnoEnum)325807,    //Wizard_HydraHead_fire2_1    
            //325813,    //Wizard_HydraHead_fire2_2    
            //325815,    //Wizard_HydraHead_fire2_3    
            (ActorSnoEnum)82972,    //Wizard_HydraHead_Frost_1    
            //83024,    //Wizard_HydraHead_Frost_2    
            //83025,    //Wizard_HydraHead_Frost_3    
            (ActorSnoEnum)82109,    //Wizard_HydraHead_Lightning_1    
            //81229,    //Wizard_HydraHead_Lightning_2    
            //81230    //Wizard_HydraHead_Lightning_3
 
            //Necromancer
            //Command Skeletons Skill ID:453793
           (ActorSnoEnum)473147, //No Rune
            (ActorSnoEnum)473428, //Enforcer Rune
            (ActorSnoEnum)473426, //Frenzy Rune
            (ActorSnoEnum)473420, //Dark Mending Rune
            (ActorSnoEnum)473417, //Freezing Grasps Rune
            (ActorSnoEnum)473418, //Kill Command Rune
           
            //Command Golem Skill ID: 460062
            (ActorSnoEnum)471646, //No Rune & Flesh Golem Rune
            (ActorSnoEnum)471647, //Ice Golem Rune
            (ActorSnoEnum)465239, //Bone Golem Rune
            (ActorSnoEnum)471619, //Decay Golem Rune
            (ActorSnoEnum)460042 //Blood Golem Rune
                       
        };
 
        public GargsMasterPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            ShowInTown = true;
 
            PlayerGargs = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 0, 255, 0, 5),
                    ShapePainter = new CircleShapePainter(Hud),
                    Radius = 2f,
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 0, 255, 0, 5),
                    Radius = 4f
                });
 
            OtherPlayersGargs = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 5),
                    ShapePainter = new CircleShapePainter(Hud),
                    Radius = 2f,
                },
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 5),
                    Radius = 4f
                });
        }
 
        public void PaintWorld(WorldLayer layer)				
{				
if (Hud.Game.IsInGame && !(Hud.Game.Me.IsInTown && !ShowInTown))				
{				
var player = Hud.Game.Me;				
var actors = Hud.Game.Actors.Where(a => GargSno.Contains(a.SnoActor.Sno));				
foreach (var actor in actors)				
{				
if (actor.SummonerAcdDynamicId == player.SummonerId)				
PlayerGargs.Paint(layer, actor, actor.FloorCoordinate, "");				
/*else				
OtherPlayersGargs.Paint(layer, actor, actor.FloorCoordinate, ""); */				
}				
				
            }
        }
    } // class
}