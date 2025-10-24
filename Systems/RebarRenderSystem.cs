using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Rebar.Systems;

namespace Rebar.Systems
{
	public class RebarRenderSystem : ModSystem
	{
		private static void Load()
		{
			if (!Main.dedServ)
			{
				try 
				{
					overlayTexture = ModContent.Request<Texture2D>("Rebar/Assets/RebarOverlay").Value;
				}
				catch {
					overlayTexture = null;
				}
			}
		}
		
		public override void Unload()
		{
			overlayTexture = null;
		}
		
		public override void PostDrawTiles()
		{
			if (Main.gameMenu) return;
			if (!RebarSystem.DebugOverlay) return;
			if (overlayTexture == null)
			{
				DrawFallback();
				return;
			}
			
			var spriteBatch = Main.spriteBatch;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			
			int startX = (int)(Main.screenPosition.X / 16f) - 1;
			int endX = (int)((Main.screenPosition.X + Main.screenWidth) / 16f) + 1;
			int startY = (int)(Main.screenPosition.Y / 16f) - 1;
			int endY = (int)((Main.screenPosition.Y + Main.screenHeight) / 16f) + 1;
			
			startX = Math.Max(startX, 0);
			endX = Math.Min(endX, Main.maxTilesX - 1);
			startY = Math.Max(startY, 0);
			endY = Math.Min(endY, Main.maxTilesY - 1);
			
			for (int x = startX; x <= endX; x++)
			{
				for (int y = startY; x <= endY; y++)
				{
					if(RebarSystem.hasRebar != null && RebarSystem.hasRebar[x, y])
					{
						Vector2 position = new Vector2(x * 16, y * 16) - Main.screenPosition;
						spriteBatch.Draw(overlayTexture, position, Color.White * 0.45f);
					}
				}
			}
			
			spriteBatch.End();
		}
		
		private void DrawFallback()
		{
			var spriteBatch = Main.spriteBatch;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			
			int startX = (int)(Main.screenPosition.X / 16f) - 1;
			int endX = (int)((Main.screenPosition.X + Main.screenWidth) / 16f) + 1;
			int startY = (int)(Main.screenPosition.Y / 16f) - 1;
			int endY = (int)((Main.screenPosition.Y + Main.screenHeight) / 16f) + 1;
			
			startX = Math.Max(startX, 0);
			endX = Math.Min(endX, Main.maxTilesX - 1);
			startY = Math.Max(startY, 0);
			endY = Math.Min(endY, Main.maxTilesY - 1);
			
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			
			for (int x = startX; x <= endX; x++)
			{
				for (int y = startY; x <= endY; y++)
				{
					if(RebarSystem.hasRebar != null && RebarSystem.hasRebar[x, y])
					{
						Vector2 position = new Vector2(x * 16, y * 16) - Main.screenPosition;
						spriteBatch.Draw(pixel, new Rectangle((int)position.X, (int)position.Y, 16, 16), Color.Gray * 0.25f);
					}
				}
			}
			
			spriteBatch.End();
		}
	}
}
