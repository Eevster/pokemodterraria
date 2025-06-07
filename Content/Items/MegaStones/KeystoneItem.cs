
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class KeyStoneItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;

			Item.value = Item.buyPrice(silver: 1);
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		/*public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.Ruby, 15)
				.AddIngredient(ItemID.Emerald, 15)
				.AddIngredient(ItemID.Sapphire, 15)
				.AddIngredient(ItemID.Amethyst, 15)
				.AddTile(TileID.WorkBenches)
				.Register();
		}*/
	}
}
