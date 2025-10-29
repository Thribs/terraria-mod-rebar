using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent;

namespace Rebar.Systems
{
	
	public class RebarSystem : ModSystem
	{
		public static List<Point> RebarPositions = new List<Point>();
		public static bool DebugOverlayEnabled;
		/*
		private const byte PacketRebarPlacement = 0;
		
		public static bool[,] hasRebar;
		
		public static bool DebugOverlay = false;
		*/
		
		public override void OnWorldLoad()
		{
			RebarPositions.Clear();
		}
		
		public override void OnWorldUnload()
		{
			RebarPositions.Clear();
		}
		
		public override void SaveWorldData(TagCompound tagCompound) {
			List<int> xPositions = new List<int>();
			List<int> yPositions = new List<int>();
			foreach (Point point in RebarPositions)
				{
					xPositions.Add(point.X);
					yPositions.Add(point.Y);
				}
			tagCompound["rebarX"] = xPositions;
			tagCompound["rebarY"] = yPositions;
		}
		
		public override void LoadWorldData(TagCompound tagCompound) {
			RebarPositions.Clear();
			if (!tagCompound.ContainsKey("rebarX") || !tagCompound.ContainsKey("rebarY")) {
				return;
			}
			List<int> xPositions = tagCompound.Get<List<int>>("reBarX");
			List<int> yPositions = tagCompound.Get<List<int>>("reBarY");
			for (int index = 0; index < xPositions.Count && index < yPositions.Count; index++) {
				RebarPositions.Add(new Point(xPositions[index], yPositions[index]));
			}
		}
		
		public override void Load() {
			On_WorldGen.KillTile += PreventTileBreakOnExplosion;
			On_WorldGen.KillWall += PreventWallBreakOnExplosion;
			On_Main.DrawWalls += DrawRebarOverlay;
		}
		
		public override void Unload() {
			On_WorldGen.KillTile -= PreventTileBreakOnExplosion;
			On_WorldGen.KillWall -= PreventWallBreakOnExplosion;
			On_Main.DrawWalls -= DrawRebarOverlay;
			RebarPositions = null;
		}
		
		private void PreventTileBreakOnExplosion(On_WorldGen.orig_KillTile orig, int i, int j, bool fail, bool effectOnly, bool noItem)
		{
			if (HasRebarAt(i, j)) {
				return;
			}
			orig(i, j, fail, effectOnly, noItem);
		}
		
		private void PreventWallBreakOnExplosion(On_WorldGen.orig_KillWall orig, int i, int j, bool fail)
		{
			if (HasRebarAt(i, j)) {
				return;
			}
			orig(i, j, fail);
		}
		
		public static bool HasRebarAt(int i, int j) {
			return RebarPositions.Contains(new Point(i, j));
		}
		
		public static void PlaceRebar(Point tilePosition)
		{
			if (!RebarPositions.Contains(tilePosition))
			{
				RebarPositions.Add(tilePosition);
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					ModContent.GetInstance<RebarSystem>().SendRebarSync(tilePosition, true);
				}
			}
		}
		
		public static void RemoveRebar(Point tilePosition)
		{ 
			if (RebarPositions.Contains(tilePosition))
			{
				RebarPositions.Remove(tilePosition);
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					ModContent.GetInstance<RebarSystem>().SendRebarSync(tilePosition, true);
				}
			}
		}
		
		public static void SetRebar(Point tilePosition, bool set)
		{
			if (set == true)
			{
				if (!RebarPositions.Contains(tilePosition))
				{
					RebarPositions.Add(tilePosition);
				}
			}
			else {
				if (!RebarPositions.Contains(tilePosition)) {
					RebarPositions.Remove(tilePosition);
				}
			}
			/*
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				ModContent.GetInstance<RebarSystem>().SendRebarSync(tilePosition, set);
			}
			*/
			ModContent.GetInstance<RebarSystem>().SendRebarSync(tilePosition, set);
		}
		
		public void SendRebarSync(Point position, bool set) {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				return;
			}
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)0);
			packet.Write((short)position.X);
			packet.Write((short)position.Y);
			packet.Write(set);
			packet.Send();
		}
		
		/*
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			byte messageType = reader.ReadByte();
			if (messageType != 0) {
				return;
			}
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			bool set = reader.ReadBoolean();
			Point point = newPoint(x, y);
			
			if (set) {
				RebarPositions.Add(point);
			}
			else {
				RebarPositions.Remove(point);
			}
			if (Main.netMode != NetmodeID) {
				return;
			}
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)0);
			packet.Write(x);
			packet.Write(y);
			packet.Write(set);
			packet.Send(-1, whoAmI);
		}
		*/
		
		private void DrawRebarOverlay(On_Main.orig_DrawWalls orig, Main self) {
		// private void DrawRebarOverlay(On_Main.orig_DrawWalls orig, Main self, bool solidLayer, bool behindWalls, bool showInvisibleWalls) {
			orig(self);
			// orig(self, solidLayer, behindWalls, showInvisibleWalls);
			
			if (DebugOverlayEnabled != true) {
				return;
			}
			
			foreach (Point point in RebarPositions) {
				Vector2 screenPosition = new Vector2(
					point.X * 16 - (int)Main.screenPosition.X,
					point.Y * 16 -(int)Main.screenPosition.Y
				);
				Main.spriteBatch.Draw(
					TextureAssets.MagicPixel.Value,
					new Rectangle ((int)screenPosition.X, (int)screenPosition.Y, 16, 16),
					Color.Lime * 0.35f
				);
			}
		}
	}
}

/*
		
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
				/*
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					var packet = ModContent.GetInstance<Rebar>().GetPacket();
					packet.Write(PacketRebarPlacement);
					packet.Write((short)i);
					packet.Write((short)j);
					packet.Write(state);
					packet.Send();
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ModContent.GetInstance<Rebar>().Logger.Debug($"Server SetRebar {i}, {j} -> {state}");
					ModContent.GetInstance<Rebar>().GetPacket()
					.Write(PacketRebarPlacement)
					.Write((short)i)
					.Write((short)j)
					.Write(state)
					.Send(-1, -1);
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
			/*
			byte[] bytes = new byte[(bits.length + 7) / 8];
			bits.CopyTo(bytes, 0);
			
			tag["rebarData"] = bytes;
			tag["rebarWidth"] = width;
			tag["rebarHeight"] = height;
		}
		
		public override void LoadWorldData(TagCompound tag) {
			Initialize();
			/*
			if (!tag.containsKey("rebarData"))
				return;
			
			] bytes = tag.GetByteArray("rebarData");
			/*
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
		/*
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

*/