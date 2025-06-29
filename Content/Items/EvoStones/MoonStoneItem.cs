
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;

namespace Pokemod.Content.Items.EvoStones
{
	public class MoonStoneItem : PokemonConsumableItem
	{
		public override void SetDefaults() {
			Item.width = 24; // The item texture's width
			Item.height = 24; // The item texture's height

			Item.useTime = 1;
			Item.useAnimation = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item1;

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}

        public override bool OnItemUse(Projectile proj){
			PokemonPetProjectile pokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			if(pokemonProj.UseEvoItem(GetType().Name)){
				Item.consumable = true;
				return true;
			}
			Item.consumable = false;
			return false;
		}
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.MoonCharm, 1)
				.AddIngredient(ItemID.FallenStar, 20)
				.AddTile(TileID.Anvils)
				.Register();
	}
	}
}
