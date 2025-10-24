using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Rebar
{
	public class Rebar : Mod
	{
		public static ModKeybind ToggleOverlayKey;
		
		public override void Load() {
			Systems.RebarSystem.Initialize();
			ToggleOverlayKey = KeybindLoader.RegisterKeybind(this, "Toggle Rebar Overlay", "F9");
		}
		
		public override void Unload() {
			ToggleOverlayKey = null;
			Systems.RebarSystem.Unload();
		}
	}
}
