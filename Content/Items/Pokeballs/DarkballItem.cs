using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Terraria;

namespace Pokemod.Content.Items.Pokeballs
{
	public class DarkballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<DarkballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<PinkApricorn>(), 50)
                .AddIngredient(ModContent.ItemType<BlueApricorn>(), 50)
                .AddIngredient(ItemID.LunarBar, 100)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class DarkballProj : BallProj
	{
		public override void SetExtraPokemonEffects(ref CaughtPokemonItem pokeItem)
		{
			pokeItem.level = 100;
		}
	}
}
