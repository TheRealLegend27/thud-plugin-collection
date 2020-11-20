// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
// *.txt files are not loaded automatically by TurboHUD
// you have to change this file's extension to .cs to enable it
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
using System;
using System.Linq;
using System.Collections.Generic;
using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno
{
 
    public class PluginEnablerOrDisablerPlugin : BasePlugin, ICustomizer
    {
 
        public PluginEnablerOrDisablerPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
        }
 
        // "Customize" methods are automatically executed after every plugin is loaded.
        // So these methods can use Hud.GetPlugin<class> to access the plugin instances' public properties (like decorators, Enabled flag, parameters, etc)
        // Make sure you test the return value against null!
        public void Customize()
        {
               
		//NayrsBlackDeath
            Hud.GetPlugin<PlayerBottomBuffListPlugin>().RuleCalculator.Rules.Add(new BuffRule(476587) { IconIndex = 7, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = true});	   
			   
            Hud.RunOnPlugin<inferno.MonsterAppearedPopup>(plugin =>{
                //sno, name, hint, title, duration (in ms), custom decorator (ignore if not needed)
                plugin.Add(451002, "Sir William", "", "Appeared", 5000);
                plugin.Add(450999, "Princess Lilian", "", "Appeared", 5000);
                plugin.Add(357917, "No Adds", "Skills\n1. Mark of Fire (lasts 15s)\n2. Charge\n3. Heavy Smash\n4. Ancient Spear (75%)\n5. Sickle Grab (50%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Waller", "Man Carver", 100000);    
                plugin.Add(353535, "Slime (100%)", "Skills\n1. Poison Worms\n2. Poison Blast\n3. Plagued Circle (<80%)\n\nAdd(s)\n1. Slime (100%) [1 - 3 per cast (Cap: >10)]\n\nAffix(es)\nNone", "The Choker",100000);
                plugin.Add(426943, "Ratlings (100%)", "Skills\n1. Rat-nado (lasts 20s)\n2. Plagued Arena (lasts 10s)\n3. Digger (teleport) \n\nAdd(s)\n1. Ratlings x 10 (100%) [Cap: 10]\n2. Ratlings x 2 - 3 (<90%) [Cap: 20]\n\nAffix(es)\nNone", "Hamelin",100000);
                plugin.Add(358614, "No Adds", "Skills\n1. Poison Nova\n2. Plague Rings\n3. Plague Sweep\n4. Plague Storm  (<50%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Knockback", "Blighter",100000);
                plugin.Add(346563, "No Adds", "Skills\n1. Overhead Attack\n2. Whirling Mortar\n3. Fire Nova (<45%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Teleporter", "Infernal Maiden",100000);
                plugin.Add(358489, "No Adds", "Skills\n1. Dash\n2. Poison Blast\n3. Poison Balls\n4. Poison Explosion (<50%)\n\nAdd(s)\nNone\n\nAffix(es)\nNone", "Erethon",100000);
                plugin.Add(354652, "No Adds", "Skills\n1. Fireball\n2. Flame Breath\n3. Mark of Fire (lasts 15s)\n4. Flame Nova (<50%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Fast\n2. Mortar", "Agnidox",100000);
                plugin.Add(345004, "Fallen (95%) Shaman (60%)", "Skills\n1. Meteor\n\nAdd(s)\n1. Demented Fallen x 2 - 3 (95%) \n2. Fallen Shaman x 2 - 3 (60%) [Cap: 5]\n\nAffix(es)\n1. Teleporter", "Ember",100000);
                plugin.Add(359688, "No Adds", "Skills\n1. Fireball 1 (100%, slow)\n2. Blood Star (lasts 5s)\n3. Geyser (60%, lasts 3s)\n4. Fireball 2 (40%, fast)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Teleporter\n2. Knockback", "Tethrys",100000);
                plugin.Add(472772, "No Adds", "Skills\n1. Energy Barrage\n2. Gateway\n\nAdd(s)\nNone\n\nAffix(es)\n1. Wormhole\n2. Frozen Pulse", "Vesalius",100000);
                plugin.Add(360281, "Swarm (75%) Snakes (50%)", "Skills\n1. Energy Twister (lasts 30s)\n\nAdd(s)\n1. Winged Larvae x 8 - 15 (75%) [Cap: 2 sets]\n2. Snakechild x 8 - 15 (50%) [Cap: 2 sets]\n\nAffix(es)\n1. Vortex", "Saxtris",100000);
                plugin.Add(354144, "No Adds", "Skills\n1. Charge\n2. Frozen Nova\n3. Frozen Storm (50%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Frozen Pulse", "Cold Snap",100000);
                plugin.Add(353874, "No Adds", "Skills\n1. Leaping Strike\n2. Leap\n\nAdd(s)\nNone\n\nAffix(es)\n1. Fast", "Bloodmaw",100000);
                plugin.Add(344389, "Fissure (100%)", "Skills\n1. Shovel\n2. Charge\n\nAdd(s)\n1. Fissure (100%, lasts 20s) [Cap: 3]\n\nAffix(es)\n1. Knockback", "Stonesinger",100000);
                plugin.Add(343759, "No Adds", "Skills\n1. Blade Cleave\n2. Teleport Strike\n3. Volley\n\nAdd(s)\nNone\n\nAffix(es)\n1. Fast", "Perdition",100000);
                plugin.Add(359094, "Bones (95%)", "Skills\n1. Arcane Bolt\n\nAdd(s)\nBones (95%) [Cap: 5]\n1. Quick Bones\n2. Reflecting Bones\n3. Mortar Bones\n4. Knockback Bones\n\nAffix(es)\n1. Wormhole", "Bone Warlock",100000);
                plugin.Add(344119, "No Adds", "Skills\n1. Frost Pools\n2. Frost Ring\n3. Volley\n\nAdd(s)\nNone\n\nAffix(es)\n1. Teleporter", "Rime",100000);
                plugin.Add(353823, "No Adds", "Skills\n1. Lightning Orb\n2. Holy Bolt Nova (75%, 25%)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Teleporter", "Raiziel",100000);
                plugin.Add(343743, "Skeletons (100%)", "Skills\n1. Arcane Nova\n2. Teleport Strike\n3. Spinning Strike\n\nAdd(s)\nSkeletons (100%, 3 - 8/cast) [Cap: 14 - 15]\n1. Returned\n2. Returned Archer\n3. Forgotten Soldier\n\nAffix(es)\n1. Jailer", "Crusader King",100000);
                plugin.Add(343767, "Stonecrusher (100%)", "Skills\n1. Cave-In\n\nAdd(s)\n1. Stonecrusher (100%, 1 - 5/cast, lasts 20s) [Cap: None]\n\nAffix(es)\n1. Fast\n2. Teleporter", "Perendi",100000);
                plugin.Add(358429, "Spiderlings (85%)", "Skills\n1. Venomballs\n2. Net Toss\n3. Poison Spit (65%)\n\nAdd(s)\n1. Spiderlings (85%, 5-7/cast) [Cap: 10]\n\nAffix(es)\n1. Fast (65%)", "The Binder",100000);
                plugin.Add(343751, "Acid Slime (85%)", "Skills\n1. Flatulence (lasts 65s)\n2. Bile Spew (35%)\n\nAdd(s)\n1. Acid Slime (65%, 2/cast) [Cap: 4/player]\n\nAffix(es)\nNone", "Voracity",100000);
                plugin.Add(358208, "No Adds", "Skills\n1. Fireball\n2. Energy Twister (75%, lasts 30s)\n3. Cave-In (65%)\n4. Slow Time (65%, lasts 15s)\n\nAdd(s)\nNone\n\nAffix(es)\n1. Teleporter", "Sand Shaper",100000);
                plugin.Add(360636, "Echoes (100%)", "Skills\n1. Lightning Breath\n\nAdd(s)\n1. Echoes (100%) [Cap: 2-3]\n\nAffix(es)\n1. Teleporter\n2. Waller\n3. Fast", "Orlash",100000);
                plugin.Add(354050, "Bones (100%)", "Skills\n1. Repulsion\n2. Tug\n\nAdd(s)\n1. Bones (100%) [Cap: 10-13]\nCanine Bones\nSpitting Bones\nHungering Bones\nRisen Bones\n\nAffix(es)\n1. Teleporter\n2. Arcane Enchanted\n3. Fast", "Eskandiel",100000);
            } );
           
			    // turn off sell darkening
            Hud.GetPlugin<InventoryAndStashPlugin>().NotGoodDisplayEnabled = false;
            
 
           
           
           
           
        }
    }
}