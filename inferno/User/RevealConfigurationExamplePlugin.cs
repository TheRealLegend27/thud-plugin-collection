// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
// *.txt files are not loaded automatically by TurboHUD
// you have to change this file's extension to .cs to enable it
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

using Turbo.Plugins.Default;

namespace Turbo.Plugins.inferno
{

    public class RevealConfigurationExamplePlugin : BasePlugin, ICustomizer
    {

        public RevealConfigurationExamplePlugin()
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
            Hud.SceneReveal.MinimapClip = false;
            Hud.SceneReveal.DisplaySceneBorder = false;
			Hud.SceneReveal.BrushKnown = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(64, 180, 180, 250));
        }

    }

}