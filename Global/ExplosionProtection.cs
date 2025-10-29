/*
using Terraria;
using Terraria.ModLoader;
using Rebar.Systems;

namespace Rebar.Global
{
	public class ExplosionProtection : GlobalTile
	{
		public override bool CanExplode(int i, int j, int type)
		{
			if (WorldGen.InWorld(i, j) && RebarSystem.HasRebar != null && RebarSystem.HasRebar[i ,j])
				return false;
			return base.CanExplode(i, j, type);
		}
	}
}
*/
