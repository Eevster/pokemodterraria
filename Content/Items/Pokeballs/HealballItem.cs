using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class HealballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<HealballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<PinkApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class HealballProj : BallProj{
        public override void SetExtraPokemonEffects(ref CaughtPokemonItem pokeItem)
        {
            pokeItem.currentHP = pokeItem.GetPokemonStats()[0];;
        }
	}
}
