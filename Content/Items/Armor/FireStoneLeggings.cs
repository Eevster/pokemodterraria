using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.DamageClasses;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class FireStoneLeggings : ModItem
	{
		public static readonly int MoveSpeedBonus = 5;
		public static readonly int AdditiveGenericDamageBonus = 200;
        public static readonly int MaxManaIncrease = 35;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeedBonus);

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage<PokemonDamageClass>() += AdditiveGenericDamageBonus / 20f; 
            player.statManaMax2 += MaxManaIncrease;
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		
	}
}
