using Pokemod.Content.Pets.CharmanderPet;
using Pokemod.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharmeleonPet
{
	public class CharmeleonPetItem : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.shoot = ModContent.ProjectileType<CharmeleonPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<CharmeleonPetBuff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}
			// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient<RareCandyItem>(16)
				.AddIngredient<CharmanderPetItem>()
				.AddTile(TileID.WorkBenches)
				.Register();
		}

	}
}
