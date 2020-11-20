using Turbo.Plugins.Default;
namespace Turbo.Plugins.inferno.partybuff
{
    public class PartyBuffConfigPlugin : BasePlugin, ICustomizer
	{
        public PartyBuffConfigPlugin()
		{
            Enabled = true;
		}
        public void Customize()
        {
 
Hud.RunOnPlugin<PartyBuffPlugin>(plugin => 
{ 
    uint[] onWiz = {
		//onWiz
     208823,
     359580,	 
     445814,
	}; 
    uint[] onMonk = {
		//onMonk

	}; 
    uint[] onWD = {
		//onWD
        
	}; 
    uint[] onBarb = {
		//onBarb
		
	}; 
    uint[] onCrus = {
		//onCrus
        
	}; 
    uint[] onDH = {
		//onDH
		365311,
	}; 
    uint[] onAll = {
		//onAll
		465350, //Simulacrum
		453801,  //  Command Skeletons
		359580, //2件Firebird's Finery
		365311,	//Companion
        403524,	
			403464,
			243141, //Black Hole
			30796, //Wave of Force
			208474, // Anomalie
            483552, // Squirts
			
	}; 
    uint[] onMe = {
		//onMe
		359583, // restraint and Focus
        Hud.Sno.SnoPowers.Wizard_Passive_ArcaneDynamo.Sno,
        
	};  
    //pass buffs to plugin -> apply them 
    uint[] onNec = {
     465350,
	 453801	
	};
    plugin.DisplayOnAll(onAll); 
    plugin.DisplayOnMe(onMe); 
    plugin.DisplayOnClassExceptMe(HeroClass.Wizard, onWiz); 
    plugin.DisplayOnClassExceptMe(HeroClass.Necromancer, onNec); 
    plugin.DisplayOnClassExceptMe(HeroClass.Monk, onMonk); 
    plugin.DisplayOnClassExceptMe(HeroClass.Barbarian, onBarb); 
    plugin.DisplayOnClassExceptMe(HeroClass.WitchDoctor, onWD); 
    plugin.DisplayOnClassExceptMe(HeroClass.DemonHunter, onDH); 
    plugin.DisplayOnClassExceptMe(HeroClass.Crusader, onCrus); 
            }); 
        }
    }
}