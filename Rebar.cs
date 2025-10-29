using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Rebar
{
	public class Rebar : Mod
	{
		public static ModKeybind ToggleOverlayKey;
		public static List<Point> RebarPositions = new();
		
		public override void Load() {
			ToggleOverlayKey = KeybindLoader.RegisterKeybind(this, "toggle Rebar overlay", "F9");
		}
		
		public override void Unload() {
			ToggleOverlayKey = null;
		}
		
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte messageType = reader.ReadByte();
			if (messageType != 0) {
				return;
			}
			
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			bool set = reader.ReadBoolean();
			Point point = new(x, y);
			
			if (set == true) {
				RebarPositions.Add(point);
			}
			else {
				RebarPositions.Remove(point);
			}
			if (Main.netMode != NetmodeID.Server) {
				return;
			}
			ModPacket packet = ModContent.GetInstance<Rebar>().GetPacket();
			packet.Write((byte)0);
			packet.Write(x);
			packet.Write(y);
			packet.Write(set);
			packet.Send(-1, whoAmI);
		}
	}
}
