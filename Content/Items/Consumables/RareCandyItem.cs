
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Items.Consumables
{
	public class RareCandyItem : PokemonConsumableItem
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

			Item.consumable = true;
		}

        public override bool OnItemUse(Projectile proj){
			PokemonPetProjectile pokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			if(pokemonProj.pokemonLvl < 100){
				pokemonProj.rareCandy = true;
				Item.consumable = true;
				return true;
			}
			Item.consumable = false;
			return false;
		}

		public override bool OnItemInvUse(CaughtPokemonItem item, Player player){
            if(item.level < 100){
                item.exp = item.expToNextLevel;
				ReduceStack(player, Item.type);
                return true;
            }
            return false;
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Silk, 40)
				.AddIngredient(ItemID.GlowingMushroom, 20)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
