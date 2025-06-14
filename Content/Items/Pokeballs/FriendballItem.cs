using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class FriendballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<FriendballProj>();
		protected override int BallValue => 4000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<GreenApricorn>(), 2)
				.AddIngredient(ItemID.Ruby, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class FriendballProj : BallProj{
        public override void SetExtraPokemonEffects(ref CaughtPokemonItem pokeItem)
        {
            pokeItem.happiness = 150;
        }
	}
}
