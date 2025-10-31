using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Rebar.Systems;

namespace Rebar.Content.Items

{
	public class RebarGlobalTile : GlobalTile {
		public override bool CanExplode(
			int i,
			int j,
			int type
		)
		{
			if (RebarSystem.HasRebarAt(i, j))
			{
				return false;
			}
			return base.CanExplode(i, j, type);
		}
	}
	public class RebarGlobalWall : GlobalWall {
		public override bool CanExplode(
			int i,
			int j,
			int type
		)
		{
			if (RebarSystem.HasRebarAt(i, j))
			{
				return false;
			}
			return base.CanExplode(i, j, type);
		}
	}
}
		