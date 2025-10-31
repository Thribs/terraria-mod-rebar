using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Rebar.Systems;
using Terraria.Audio;

namespace Rebar.Content.Items
{
	public class RebarItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 100;
			Item.consumable = true;
			// virtual layer system, not a physical tile.
			// if you add a real tile later, uncomment this:
			// // Item.createTile = ModContent.TileType<Tiles.RebarTile>();
		}
		
		public override bool? UseItem(Player player) {
			
			int positionX = Player.tileTargetX;
			int positionY = Player.tileTargetY;
			if (!WorldGen.InWorld(positionX, positionY))
			{
				return false;
			}
			if (RebarSystem.HasRebarAt(positionX, positionY)) {
				return false;
			}
			Tile tile = Main.tile[positionX, positionY];

			if (!tile.HasTile || tile.WallType <= 0)
			{
				return false;
			}
			RebarSystem.SetRebar(new Point(positionX, positionY), true);
			if (Main.netMode != NetmodeID.Server) {
				Main.NewText($"placed rebar at {positionX},{positionY}", 100, 255, 100);
			}
			SoundEngine.PlaySound(SoundID.Tink);
			Item.stack--;
			return true;
		}
		
		public override void AddRecipes() {
			Recipe ironRecipe = CreateRecipe(10);
			ironRecipe.AddIngredient(ItemID.IronBar);
			ironRecipe.AddTile(TileID.Anvils);
			ironRecipe.Register();
			
			Recipe leadRecipe = CreateRecipe(10);
			leadRecipe.AddIngredient(ItemID.LeadBar);
			leadRecipe.AddTile(TileID.Anvils);
			leadRecipe.Register();
		}
	}
}
