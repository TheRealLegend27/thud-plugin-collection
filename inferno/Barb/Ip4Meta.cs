using System;
using Turbo.Plugins.Default;
using System.Linq;
using System.Threading;
 
using System.Text;
 
namespace Turbo.Plugins.inferno {
    public class ip4meta : BasePlugin, IInGameWorldPainter  {
        private IFont TextFont { get; set; }
        private IFont TextFontRedOk { get; set; }
        private IFont TextFontRedNo { get; set; }
        private IFont TextFontRedNull { get; set; }
        private IFont TextFontDebug { get; set; }
        private TopLabelDecorator RedDecorator { get; set; }
        private TopLabelDecorator GreenDecorator { get; set; }
        private IWatch [] PlayerIni { get; set; }
        private string [] PlayerName { get; set; }
        private string Tiempo  { get; set; }
        private string IPrS { get; set; }
       
        public bool OnlyGR { get; set; }
        public bool OnlyTrusted { get; set; }
 
        public float Offx { get; set; }
        public float Offy { get; set; }    
 
        public ip4meta()    {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
           
            Offx = 0.32f;
            Offy = 0.60f;
           
            OnlyGR = true;
            OnlyTrusted = true;
       
            TextFontDebug = Hud.Render.CreateFont("tahoma", 8, 150, 43, 231, 6, true, false, false);
            TextFontRedOk = Hud.Render.CreateFont("tahoma", 6, 255, 255, 100, 100, true, false, false);
            TextFontRedNo = Hud.Render.CreateFont("tahoma", 6, 255, 255, 0, 0, true, false, false);
            TextFontRedNull = Hud.Render.CreateFont("tahoma", 6, 255, 150, 150, 150, true, false, false);
           
            Tiempo = "0:00";
            IPrS = "IP";
            PlayerIni = new IWatch [4] ;
            for (var i = 0; i < 4; i++) { PlayerIni[i] = Hud.Time.CreateWatch(); }
            PlayerName = new string [4] { string.Empty, string.Empty, string.Empty, string.Empty};
 
            RedDecorator = new TopLabelDecorator(Hud)  {
                TextFont = TextFontRedOk,
                BackgroundTexture1 = Hud.Texture.ButtonTextureBlue,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureBlue,
                BackgroundTextureOpacity1 = 10.0f,
                BackgroundTextureOpacity2 = 10.0f,
                TextFunc = () =>  Tiempo,
                HintFunc = () => "Sin IP",
            };
            GreenDecorator = new TopLabelDecorator(Hud)   {
                TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 0, true, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureGreen,
                BackgroundTextureOpacity1 = 10.0f,
                BackgroundTextureOpacity2 = 10.0f,
                TextFunc = () => IPrS,
                HintFunc = () => "Con IP",
               
            };         
        }
   
        public void PaintWorld(WorldLayer layer)  {
            if (!Hud.Game.Me.IsInGame) return;
            if ((Hud.Game.Me.InGreaterRift) || !OnlyGR )  {  
                var BarbNum = 0 ; var BarbNumR = 0; bool BarbOnlyMe = false;  // BarNum nº barb con skill ip , BarbNumR nº barb con skill ip y runa codo con codo
                var players = Hud.Game.Players.Where(p => (p.BattleTagAbovePortrait != string.Empty) );
                foreach (var player in players) {
                    if (player.HeroClassDefinition.HeroClass == HeroClass.Barbarian) //Si hay barbaro en la partida
                    {
                        foreach (var skill in player.Powers.CurrentSkills) {
                            if (skill.SnoPower.Sno == 79528) {  
                                if (skill.Rune == 2) { //Si el barbaro lleva ip con la runa codo con codo
                                    BarbNumR++; if ( player.IsMe ) BarbOnlyMe = true;
                                }
                                BarbNum++; // Número de bárbaros que dan ip, aunque sólo sea a sí mismos.
                            }
                           
                        }  
                    }
                }
                if (BarbNum > 1) { BarbOnlyMe = false; } //Será true cuando yo sea el único Bárbaro que da Ip en la partida. Pensado para GR Meta               
                if (BarbNumR > 0) {
                    foreach (var player in  players) {                     
                         if (PlayerName[player.Index] != player.BattleTagAbovePortrait) {
                            PlayerName[player.Index] = player.BattleTagAbovePortrait;
                            if (PlayerIni[player.Index].IsRunning) PlayerIni[player.Index].Restart();
                            else PlayerIni[player.Index].Start();
                        }
                        var portrait = player.PortraitUiElement.Rectangle;
                        var MiraIp = player.Powers.GetBuff(79528);
                        long ultima = 0; long queda = 0 ; int fiable = 0; long IPms = 0; int pos = 0; bool ShowNoIp = false;
                        if (MiraIp != null) {
                            pos = (player.Powers.GetBuff(79528).TimeLeftSeconds[1] > player.Powers.GetBuff(79528).TimeLeftSeconds[0])? 1:0; //[0] ip dada por mi, [1] ip dada por otro . Nos quedaremos con la de mayor duración
                            ultima = player.Powers.GetBuff(79528).LastActive.ElapsedMilliseconds; queda = (long) (1000 * player.Powers.GetBuff(79528).TimeLeftSeconds[pos]) ;                          
                            IPms = queda - ultima ;
                            if ( IPms > 0 ) { if (PlayerIni[player.Index].IsRunning) PlayerIni[player.Index].Reset(); }
                            else { if (!PlayerIni[player.Index].IsRunning) PlayerIni[player.Index].Start(); }
                            fiable = (player.IsMe || BarbOnlyMe || (ultima < 10))? 1: ( ( (player.SnoArea.Sno == Hud.Game.Me.SnoArea.Sno) && (player.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 75) )? 2 : 0 );
                            if (fiable > 0) {
                                if ( IPms > 0 ) {  
                                    IPrS = (IPms < 5001)? string.Format("{0:0}s",(int) (IPms + 999) /1000) : "IP";
                                    GreenDecorator.Paint(portrait.Left + portrait.Width * Offx , portrait.Top + portrait.Height*  Offy, portrait.Width * 0.34f, portrait.Height * 0.12f, HorizontalAlign.Center);                          
                                }
                                else {                             
                                    ShowNoIp = true; RedDecorator.TextFont = TextFontRedOk ;
                                }
                            }
                            else if (!OnlyTrusted) {
                                ShowNoIp = true; RedDecorator.TextFont = TextFontRedNo;
                            }
                        }  
                        else if (!OnlyTrusted)  {
                            ShowNoIp = true; RedDecorator.TextFont = TextFontRedNull;
                        }
                        if (ShowNoIp) {
                            long time = PlayerIni[player.Index].ElapsedMilliseconds / 1000; Tiempo = String.Format("{0:0}:{1:00}", (int) time / 60 , time % 60 ) ;
                            RedDecorator.Paint(portrait.Left + portrait.Width * Offx, portrait.Top + portrait.Height*  Offy, portrait.Width * 0.34f, portrait.Height * 0.12f, HorizontalAlign.Center);                     
                        }
                                                                                       
                    }                            
                }                        
            }
        }
    }
}