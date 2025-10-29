using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ID;
using Rebar.Systems;

namespace Rebar.Players
{
	public class RebarPlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (Rebar.ToggleOverlayKey.JustPressed)
			{
				RebarSystem.DebugOverlayEnabled = !RebarSystem.DebugOverlayEnabled;
				if (Main.netMode != NetmodeID.Server)
				{
					string message = RebarSystem.DebugOverlayEnabled ? "Rebar overlay ON" : "Rebar overlay OFF";
					Main.NewText(message, 200, 200, 200);
				}
			}
		}
	}
}
