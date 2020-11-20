using Turbo.Plugins.Default;
 
namespace Turbo.Plugins.inferno
{
    public class DAV_BossConfigPlugin : BasePlugin, ICustomizer {
        public DAV_BossConfigPlugin() {
            Enabled = true;
        }
       
        public void Customize() {
           
            // Add the Boss Watch List
            Hud.RunOnPlugin<DAV_BossAnimeLog>(plugin => {
                 plugin.BossList.Add("Agnidox"); // (empowered Demonic Hellflyer)
                 plugin.BossList.Add("Blighter"); // (empowered Herald of Pestilence)
                 plugin.BossList.Add("Bloodmaw"); // (empowered Executioner)
                 plugin.BossList.Add("Bone Warlock"); // (empowered Skeletal Summoner)
                 plugin.BossList.Add("Cold Snap"); // (empowered Izual)
                 plugin.BossList.Add("Crusader King"); // (empowered Skeleton King)
                 plugin.BossList.Add("Ember"); // (empowered Morlu Caster)
                 plugin.BossList.Add("Erethon"); // (empowered Corrupted Angel)
                 plugin.BossList.Add("Eskandiel"); // (empowered Corpse Raiser)
                 plugin.BossList.Add("Hamelin"); // (empowered Rat King)
                 plugin.BossList.Add("Infernal Maiden"); // (empowered Fire Maiden)
                 plugin.BossList.Add("Man Carver"); // (empowered Butcher)
                 plugin.BossList.Add("Orlash"); // (empowered Terror Demon)
                 plugin.BossList.Add("Perdition"); // (empowered Rakanoth)
                plugin.BossList.Add("Perendi"); // (empowered Mallet Lord)
                 plugin.BossList.Add("Raiziel"); // (empowered Exarch)
                 plugin.BossList.Add("Rime"); // (empowered Xah'Rith the Keywarden)
                 plugin.BossList.Add("Sand Shaper"); // (empowered Zoltun Kulle)
                 plugin.BossList.Add("Saxtris"); // (empowered Deceiver)
                 plugin.BossList.Add("Stonesinger"); // (empowered Sand Dweller)
                 plugin.BossList.Add("Tethrys"); // (empowered Succubus)
                 plugin.BossList.Add("The Binder"); // (empowered Cydaea)
                 plugin.BossList.Add("The Choker"); // (empowered Barbed Lurker)
                 plugin.BossList.Add("Vesalius"); // (empowered Vidian)
                 plugin.BossList.Add("Voracity"); // (empowered Ghom)
            });
           
        Hud.RunOnPlugin<DAV_BossWarmingPlugin>(plugin => {
                plugin.WarmingOffsetX = -20.0f;
                plugin.WarmingOffsetY = 0.0f;
                plugin.WarmingOffsetZ = 10.0f;
                plugin.ShowOrlashClone = false;
                plugin.GRonly = true;
                 //Orlash   
				plugin.WarmingMessage.Add(AnimSnoEnum._terrordemon_attack_firebreath, "Lightning Breath");
                plugin.WarmingMessage.Add(AnimSnoEnum._terrordemon_attack_01, "Smack");
                plugin.WarmingMessage.Add(AnimSnoEnum._terrordemon_generic_cast, "Summoning");
                
                
                //Bloodmaw
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_westmarchbrute_taunt, "JUMP");
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_westmarchbrute_b_attack_06_in, "MELEE");
                
                
                //Crusader King
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_cast_summon, "SUMMONING");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_whirlwind_start, "TRIPLE SWIG");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_whirlwind_loop, "TRIPLE SWING");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_whirlwind_end, "TRIPLE SWING");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_teleport, "TELEPORT");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonking_attack_02, "MELEE");
                
                
                //Infernal Maiden
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_deathmaiden_fire_attack_01, "MELEE");
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_deathmaiden_attack_04_aoe, "Overhead Attack");
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_deathmaiden_attack_special_360_01, "Spinning Attack\n\nSPINNING ATTACK + MORTAR (when <50%)");
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_deathmaiden_attack_special_flip_01, "FIRE NOVA");
                
                
                //Man Carver
                plugin.WarmingMessage.Add(AnimSnoEnum._butcher_attack_chain_01_in, "Sickle Grab");
                plugin.WarmingMessage.Add(AnimSnoEnum._butcher_attack_charge_01_in, "CHARGE");
                plugin.WarmingMessage.Add(AnimSnoEnum._butcher_attack_fanofchains, "Fan of Spears");
                plugin.WarmingMessage.Add(AnimSnoEnum._butcher_attack_05_telegraph, "Heavy Smash");
                
                
                //Saxtris
                plugin.WarmingMessage.Add(AnimSnoEnum._snakeman_melee_generic_cast_01, "Summoning");
                plugin.WarmingMessage.Add(AnimSnoEnum._snakeman_melee_attack_01, "MELEE");
                
                
                //Hamelin
                plugin.WarmingMessage.Add(AnimSnoEnum._p4_ratking_spawn_01, "Summoning");
                plugin.WarmingMessage.Add(AnimSnoEnum._p4_ratking_burrow_in, "Burrow");
                plugin.WarmingMessage.Add(AnimSnoEnum._p4_ratking_summon_01, "Rat-nado");
                plugin.WarmingMessage.Add(AnimSnoEnum._p4_ratking_roar_summon, "Plagued Arena");
                
                
                //Bone Warlock
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonsummoner_generic_cast, "Wormhole\nShort Teleport");
                plugin.WarmingMessage.Add(AnimSnoEnum._skeletonsummoner_attack_01, "Arcane Bolt");
                
                
                //Perendi
                plugin.WarmingMessage.Add(AnimSnoEnum._malletdemon_generic_cast, "Cave In\nShort Teleport");
                plugin.WarmingMessage.Add(AnimSnoEnum._malletdemon_attack_01, "MELEE");
                
                
                //The Choker
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_squigglet_generic_cast, "Summoning"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_squigglet_rangedattack_v2, "Squigglet Cone"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_squigglet_strafe_attack_left_01, "Squigglet Bolt"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_squigglet_strafe_attack_right_01, "Squigglet Bolt"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_squigglet_taunt_01, "PLAGUED CIRCLE\nSTUN in 1s"); 
                
                
                //Eskandiel
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_dark_angel_generic_cast, "Summoning\nArcane Blob\nShort Teleport"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_dark_angel_cast, "REPULSION WAVE"); 
                
                
                //Voracity
                plugin.WarmingMessage.Add(AnimSnoEnum._gluttony_attack_chomp, "MELEE"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._gluttony_attack_areaeffect, "Fart Cloud"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._gluttony_attack_ranged_01, "Summoning"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._gluttony_attack_sneeze, "Bile Spew"); 
                
                
                //Vesalius
                plugin.WarmingMessage.Add(AnimSnoEnum._p6_envy_cast_02, "ENERGY BARRAGE"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._p6_envy_teleport_start_02, "GATEWAY"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._p6_envy_attack_01, "MELEE"); 
                
                
                //Stonesinger
                plugin.WarmingMessage.Add(AnimSnoEnum._sandmonster_temp_rock_throw, "Shovel"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._sandmonsterblack_attack_03_sandwall, "Summoning"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._sandmonster_attack_01, "MELEE"); 


                
                //Agnidox
                plugin.WarmingMessage.Add(AnimSnoEnum._demonflyer_mega_fireball_01, "Fireball\nFire Nova"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._demonflyer_mega_firebreath_01, "Flame Breath"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._demonflyer_mega_attack_01, "MELEE"); 
                
                
                //Cold Snap
                plugin.WarmingMessage.Add(AnimSnoEnum._bigred_firebreath_combo_01, "Frozen Nova"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._bigred_charge_01, "Charge"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._bigred_attack_02, "MELEE"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._bigred_generic_cast_01, "Freezing Pulse\nFrozen Storm"); 
                
                
                //The Binder
                plugin.WarmingMessage.Add(AnimSnoEnum._mistressofpain_attack_01, "MELEE"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._mistressofpain_attack_spellcast_summon_webpatch, "Net Toss"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._mistressofpain_attack_spellcast_poison, "Poison Spit"); 
                
                
                //Ember
                plugin.WarmingMessage.Add(AnimSnoEnum._morluspellcaster_generic_cast, "Summoning"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._morluspellcaster_attack_aoe_01, "METEOR");




                //Tethrys
                plugin.WarmingMessage.Add(AnimSnoEnum._succubus_generic_cast_01, "Blood Star\nFire Ball\nGEYSER (60%)"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._succubus_attack_melee_01, "MELEE"); 
                
                
                //Raiziel
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_sniperangel_firebomb_01, "Lightning Orb"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_sniperangel_temp_cast_01, "Holy Bolt Nova"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_sniperangel_lightning_spray_01, "MELEE"); 
                
                
                //Sand Shaper
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_aoe_01, "Cave In"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_direct_cast_04, "FIRE BALL"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_taunt_01, "Taunt"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_omni_cast_05_fadeout, "Teleport"); 
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_omni_cast_01, "Twister");  
                plugin.WarmingMessage.Add(AnimSnoEnum._zoltunkulle_omni_cast_04, "Slow Time");  
                
                
                //Rime
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_lr_boss_morluspellcaster_generic_cast, "Volley\nFrost Ring\nFront Pools");  
                plugin.WarmingMessage.Add(AnimSnoEnum._p2_morluspellcaster_attack_melee_01_uber, "MELEE");  
                
                
                //Blighter
                plugin.WarmingMessage.Add(AnimSnoEnum._creepmob_attack_04_in, "Plagued Line");  
                plugin.WarmingMessage.Add(AnimSnoEnum._creepmob_generic_cast, "Poison Nova");  
                plugin.WarmingMessage.Add(AnimSnoEnum._creepmob_attack_01, "MELEE");  
                plugin.WarmingMessage.Add(AnimSnoEnum._creepmob_attack_04_middle, "Plaguestorm");  
                
                
                //Perdition
                plugin.WarmingMessage.Add(AnimSnoEnum._lordofdespair_attack_energyblast, "Blade Cleave");  
                plugin.WarmingMessage.Add(AnimSnoEnum._lordofdespair_attack_stab, "MELEE");  
                plugin.WarmingMessage.Add(AnimSnoEnum._lordofdespair_attack_teleport_full, "BLINK STRIKE");  
                plugin.WarmingMessage.Add(AnimSnoEnum._lordofdespair_spellcast, "Volley");  
                
                
                //Erethon
                plugin.WarmingMessage.Add(AnimSnoEnum._x1_lr_boss_angel_corrupt_a_cast_01, "POISON BLAST\nPoison Balls");  
                plugin.WarmingMessage.Add(AnimSnoEnum._angel_corrupt_attack_01, "MELEE");  
                plugin.WarmingMessage.Add(AnimSnoEnum._angel_corrupt_attack_dash_in, "Dash");
            });
        }
    }
}