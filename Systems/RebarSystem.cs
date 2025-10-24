using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Rebar.Systems
{
	
	public class RebarSystem : ModSystem
	{
		public static bool[,] hasRebar;
		
		public static bool DebugOverlay = false;
		
		public static void Initialize() {
			int width = Main.maxTilesX;
			int height = Main.maxTilesY;
			hasRebar = new bool[width, height];
		}
		
		public override void Unload() {
			hasRebar = null;
		}
		
		public static void SetRebar(int i, int j, bool state, bool sync = false) {
			if (!WorldGen.InWorld(i, j) || hasRebar == null)
				return;
			
			hasRebar[i, j] = state;
			
			if (sync)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					var packet = ModContent.GetInstance<Rebar>().GetPacket();
					packet.Write((byte)MessageType.SyncRebar);
					packet.Write((short)i);
					packet.Write((short)j);
					packet.Write(state);
					packet.Send();
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ModContent.GetInstance<Rebar>().Logger.Debug($"Server SetRebar {i}, {j} -> {state}");
					ModContent.GetInstance<Rebar>().GetPacket().Write((byte)MessageType.SyncRebar)
						.Write((short)i).Write((short)j).Write(state).Send(-1, -1);
				}
			}
		}
		
		
		public override void ClearWorld() {
			Initialize();
		}
		
		public override void SaveWorldData(TagCompound tag) {
			
			if (hasRebar == null)
				Initialize();
			
			int width = Main.maxTilesX;
			int height = Main.maxTilesY;
			BitArray bits = new BitArray(width * height);
			
			int index = 0;
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					bits[index++] = hasRebar[x, y];
				}
			}
			
			byte[] bytes = new byte[(bits.length + 7) / 8];
			bits.CopyTo(bytes, 0);
			
			tag["rebarData"] = bytes;
			tag["rebarWidth"] = width;
			tag["rebarHeight"] = height;
		}
		
		public override void LoadWorldData(TagCompound tag) {
			Initialize();
			
			if (!tag.containsKey("rebarData"))
				return;
			
			byte[] bytes = tag.GetByteArray("rebarData");
			int width = tag.getInt("rebarWidth");
			int height = tag.getInt("rebarHeight");
			
			BitArray bits = new BitArray(bytes);
			int index = 0;
			
			for (int x = 0; x < width && x < Main.maxTilesX; x++) {
				for (int y = 0; y < height && y < Main.maxTilesY; y++) {
					hasRebar[x, y] = bits[index++];
				}
			}
		}
		
		public override void PostAddRecipes() {
			On.Terraria.WorldGen.KillTile_DropItems += ProtectTilesFromExplosions;
			On.Terraria.WorldGen.KillWall += ProtectWallsFromExplosions;
		}
		
		private void ProtectTilesFromExplosions(On.Terraria.WorldGen.orig_KillTile_DropItems orig, int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
			if (hasRebar != null && hasRebar[i, j]) {
				fail = true;
				noItem = true;
				return;
			}
			orig(i, j, ref fail, ref effectOnly, ref noItem);
		}
		
		private bool ProtectWallsFromExplosions(On.Terraria.WorldGen.orig_KillWall orig, int i, int j, bool fail) {
			if (hasRebar != null && hasRebar[i, j])
				return true;
			return orig(i, j, fail);
		}
	}
}
