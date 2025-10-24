using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Rebar.Systems;

namespace Rebar.Items
{
	public class RebarItem : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rebar");
			Tooltip.SetDefault("Place behind blocks to make them explosion-proof");
		}
		
		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			// virtual layer system, not a physical tile.
			// if you add a real tile later, uncomment this:
			// // Item.createTile = ModContent.TileType<Tiles.RebarTile>();
		}
		
		public override bool? UseItem(Player player) {
			int i = Player.tileTargetX;
			int j = Player.tileTargetY;
			if (!WorldGen.InWorld(i, j))
				return false;
			
			RebarSystem.SetRebar(i, j, true, sync: true);
			
			if (Main.netMode != NetmodeID.Server)
				CombatText.NewText(new Microsoft.Xna.Framework.Rectangle(i * 16, j * 16, 16, 16), Microsoft.Xna.Framework.Color.Gray, "Rebar!");
			
			return true;
		}
		
		public override void AddRecipes() {
			Recipe ironRecipe = CreateRecipe(10);
			ironRecipe.AddIngredient(ItemID.IronBar);
			ironRecipe.AddTile(TileId.Anvils);
			ironRecipe.Register();
			
			Recipe leadRecipe = CreateRecipe(10);
			leadRecipe.AddIngredient(ItemID.LeadBar);
			leadRecipe.AddTile(TileID.Anvils);
			leadRecipe.Register();
		}
	}
}
