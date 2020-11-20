namespace Turbo.Plugins.inferno
{
    using Turbo.Plugins.Default;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class NecromancerSkeletonIndicatorPlugin : BasePlugin, IInGameWorldPainter, IInGameTopPainter
    {
        public WorldDecoratorCollection CommandSkeletonDecorator, SkeletonMageDecorator;
        public TopLabelDecorator CommandSkeletonCountLabel, SkeletonMageCountLabel, CombinedSkeletonCountLabel;
        public string CommandSkeletonLabel, SkeletonMageLabel;
        public int CommandSkeletonCount, SkeletonMageCount;
        public float barW, barH, barX, barY, cmb_barW, cmb_barH, cmb_barX, cmb_barY;
        public bool CommandSkeletonEnabled, SkeletonMageEnabled, CombinedCountEnabled;

        private static ActorSnoEnum CommandSkeletonSkillSNO =  (ActorSnoEnum).453801;
        public HashSet<ActorSnoEnum> CommandSkeletonActorSNOs = new HashSet<ActorSnoEnum>
        {
             (ActorSnoEnum)473147, // CommandSkeleton - No Rune
             (ActorSnoEnum)473428, // CommandSkeleton - Enforcer
             (ActorSnoEnum)473426, // CommandSkeleton - Frenzy
             (ActorSnoEnum)473420, // CommandSkeleton - Dark Mending
             (ActorSnoEnum)473417, // CommandSkeleton - Freezing Grasp
             (ActorSnoEnum)473418  // CommandSkeleton - Kill Command
        };

        private static ActorSnoEnum SkeletonMageSkillSNO =  (ActorSnoEnum).462089;
        public HashSet<ActorSnoEnum> SkeletonMageActorSNOs = new HashSet<ActorSnoEnum>
        {
            (ActorSnoEnum)472275, // Skeleton Mage - No Rune
             (ActorSnoEnum)472588, // Skeleton Mage - Gift of Death
             (ActorSnoEnum)472769, // Skeleton Mage - Contamination
             (ActorSnoEnum)472801, // Skeleton Mage - Archer
             (ActorSnoEnum)472606, // Skeleton Mage - Singularity
             (ActorSnoEnum)472715  // Skeleton Mage - Life Support
        };


        public NecromancerSkeletonIndicatorPlugin()
        {
            Enabled = true;

         

            // Enable both indicator or disable the indicator a specific skill
            CommandSkeletonEnabled = true;
            SkeletonMageEnabled = true;

            // Inits variables
            CommandSkeletonCount = 0;
            SkeletonMageCount = 0;
            CommandSkeletonLabel = "";
            SkeletonMageLabel = "";
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            // Unlike the old XML system where it draws x,y,w,h in terms of percentage of screen size, the new plugin uses actual pixel coordinates
            // To convert x,y,w,h sizes from the XML system to the new plugin system, multiply the screensize with percentage. ie. XML size of 2 => 0.02f * screensize

            // Display coordinated for non-combined indicators
            barW = Hud.Window.Size.Width * 0.013f;
            barH = Hud.Window.Size.Height * 0.0177f;
            barX = Hud.Window.Size.Width * 0.475f;
            barY = Hud.Window.Size.Height * 0.36f;

            // Display coordinated for combined indicators
            cmb_barW = Hud.Window.Size.Width * 0.0175f;
            cmb_barH = Hud.Window.Size.Height * 0.0175f;
            cmb_barX = (Hud.Window.Size.Width * 0.5f) - (cmb_barW * 0.5f);
            cmb_barY = Hud.Window.Size.Height * 0.33f;

            // Decorator under each sekeleton melee
            CommandSkeletonDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 255, 0, 14),
                    Radius = 0.5f,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(220, 200, 200, 0, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 255, 255, 255, 255, 255, true, false, false)
                },
                new MapShapeDecorator(Hud)
                {
                    ShapePainter = new RotatingTriangleShapePainter(Hud),
                    Brush = Hud.Render.CreateBrush(220, 200, 200, 0, 0),
                    Radius = 0f,
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 250)
                }
            );

            // Decorator under each sekeleton mages
            SkeletonMageDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(240, 200, 50, 0, 1),
                    Radius = 0.5f,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(220, 200, 200, 0, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, true, false, false)
                },
                new MapShapeDecorator(Hud)
                {
                    ShapePainter = new RotatingTriangleShapePainter(Hud),
                    Brush = Hud.Render.CreateBrush(220, 200, 200, 0, 0),
                    Radius = 0f,
                    RadiusTransformator = new StandardPingRadiusTransformator(Hud, 250)
                }
            );

            // Label Decorator for non-combined indicator
            CommandSkeletonCountLabel = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 240, 0, false, false, false),
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity2 = 0.25f
            };
            SkeletonMageCountLabel = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 240, 0, false, false, false),
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity2 = 0.25f
            };

            // Label Decorator for combined indicator
            CombinedSkeletonCountLabel = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 240, 0, false, false, false),
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity2 = 0.25f
            };
        }

        public void PaintWorld(WorldLayer layer)
        {
            // Don't draw if not playing a Necromancer; don't draw if is in town
            //if (Hud.Game.Me.HeroClassDefinition.HeroClass != HeroClass.Necromancer || Hud.Game.IsInTown) return;
			var monsters = Hud.Game.AliveMonsters.Where(x => x.SnoMonster.Priority == MonsterPriority.boss);
			foreach (var monster in monsters)
		{
            // For Skeleton Melee, only when equipping the skill
            if (CommandSkeletonEnabled != null)
            {
                // Iterate all the game actors, find out and count which are skeleton melees summoned by player
                var CommandSkeletonActors = Hud.Game.Actors.Where(EachActor => CommandSkeletonActorSNOs.Contains(EachActor.SnoActor.Sno)); //&& // Find out which are skeleton melees actors
                                            //EachActor.SummonerAcdDynamicId == Hud.Game.Me.SummonerId); // Then find out if they are summoned by the player
                CommandSkeletonCount = CommandSkeletonActors.Count(); // And then count how many are found

                // Paint circle decorator under each skeleton melee 
                foreach (var EachActor in CommandSkeletonActors)
                {
                    var text = string.IsNullOrWhiteSpace(CommandSkeletonLabel) ? EachActor.SnoActor.NameLocalized : CommandSkeletonLabel;
                    CommandSkeletonDecorator.Paint(layer, EachActor, EachActor.FloorCoordinate, text);
                }
            }
            else
            {
                CommandSkeletonCount = 0;
            }
        }
			foreach (var monster in monsters)
		{
            // For Skeleton Mages, only when equipping the skill
            if (SkeletonMageEnabled != null)
            {
                // Iterate all the game actors, find out and count which are skeleton mages summoned by player
                var SkeletonMageActors = Hud.Game.Actors.Where(EachActor => SkeletonMageActorSNOs.Contains(EachActor.SnoActor.Sno)); //&& // Find out which are skeleton mages actors
                                         //EachActor.SummonerAcdDynamicId == Hud.Game.Me.SummonerId); // Then find out if they are summoned by the player
                SkeletonMageCount = SkeletonMageActors.Count(); // And then count how many are found

                // Paint circle decorator under each skeleton melee
                foreach (var EachActor in SkeletonMageActors)
                {
                    var text = string.IsNullOrWhiteSpace(SkeletonMageLabel) ? EachActor.SnoActor.NameLocalized : SkeletonMageLabel;
                    SkeletonMageDecorator.Paint(layer, EachActor, EachActor.FloorCoordinate, text);
                }
            }
			
            else
            {
                SkeletonMageCount = 0;
            }
        }
		}

        public void PaintTopInGame(ClipState clipState)
        {
					 
			
			// Don't draw if not playing a Necromancer; don't draw if is in town
            //if (Hud.Game.Me.HeroClassDefinition.HeroClass != HeroClass.Necromancer || Hud.Game.IsInTown) return;
            if (clipState != ClipState.BeforeClip) return;

			
            
				
			}
        }
    }
