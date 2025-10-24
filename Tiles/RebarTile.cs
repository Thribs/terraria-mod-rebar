using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RebarMod.Tiles
{
	public class RebarTile : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlendAll[Type] = false;
			
			AddMapEntry(new Color(100, 100, 100), "Rebar");
			DustType = DustID.Iron;
		}
	}
}
