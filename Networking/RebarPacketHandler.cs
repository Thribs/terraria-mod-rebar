using System.IO;
using Terraria;
using Terraria.ModLoader;
using Rebar.Systems;

namespace Rebar.Networking {
	public class RebarPacketHandler
	{
		private const byte PacketRebarPlacement = 0;
		
		public static void SendRebarPlacement(int i, int j, bool state) {
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;
			
			ModPacket packet = RebarMod.Instance.GetPacket();
			packet.Write(PacketRebarPlacement);
			packet.Write(i);
			packet.Write(j);
			packet.Write(state);
			packet.Send();
		}
		
		public static void Handle(BinaryReader reader, int whoAmI) {
			byte type = reader.readByte();
			if (type == PacketRebarPlacement)
			{
				int i = reader.readInt16();
				int j = reader.readInt16();
				bool state = reader.readBoolean();
				
				if (!WorldGen.InWorld(i, j)) return;
				
				RebarSystem.hasRebar[i, j] = state;
				
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
