using Pokemod.Content.Items.EvoStones;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.DamageClasses;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class ThunderStoneBreastplate : ModItem
	{
		public static readonly int MaxManaIncrease = 20;
		public static readonly int AdditiveGenericDamageBonus = 150;
		public static readonly int MaxMinionIncrease = 1;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxManaIncrease, MaxMinionIncrease);

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 6; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			
			player.GetDamage<PokemonDamageClass>() += AdditiveGenericDamageBonus / 20f;
            player.GetDamage(DamageClass.Ranged) += AdditiveGenericDamageBonus / 40f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ThunderStoneItem>(30)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
