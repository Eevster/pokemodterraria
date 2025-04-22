using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;
using Pokemod.Common.Systems;

namespace Pokemod.Content.Items.Pokeballs
{
	public class NestballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<NestballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 1)
				.AddIngredient(ModContent.ItemType<BlackApricorn>(), 1)
				.AddIngredient(ItemID.Ruby, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class NestballProj : BallProj{
		public override bool FailureProb(float catchRate){
			float pokemonLvl = targetPokemon.GetGlobalNPC<PokemonNPCData>().lvl;

			if(pokemonLvl < 30){
				catchRate *= (41f-pokemonLvl)*0.1f;
			}

			return RegularProb(catchRate);
		}
	}
}
