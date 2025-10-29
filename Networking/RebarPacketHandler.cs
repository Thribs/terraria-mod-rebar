/*
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Rebar.Networking {
	public class RebarPacketHandler : ModSystem
	{
	}
}
/*
		private const byte PacketRebarPlacement = 0;
		
		public static void SendRebarPlacement(int i, int j, bool state) {
			/*
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;
			
			ModPacket packet = ModContent.GetInstance<Rebar>().GetPacket();
			packet.Write(PacketRebarPlacement);
			packet.Write(i);
			packet.Write(j);
			packet.Write(state);
			packet.Send();
		}
		
		public static void Handle(BinaryReader reader, int whoAmI) {
			/* byte type = reader.readByte();
			if (type == PacketRebarPlacement)
			{
				int i = reader.ReadInt16();
				int j = reader.ReadInt16();
				bool state = reader.ReadBoolean();
				
				if (!WorldGen.InWorld(i, j)) return;
				
				RebarSystem.hasRebar[i, j] = state;
				
				/*
				if (Main.netMode == NetmodeID.Server)
				{
					ModPacket packet = ModContent.GetInstance<Rebar>().GetPacket();
					packet.Write(PacketRebarPlacement);
					packet.Write((short)i);
					packet.Write((short)j);
					packet.Write(state);
					packet.Send(-1, whoAmI);
				}
			}
		}
	}
}
*/
