using Pokemod.Content.Items.EvoStones;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.DamageClasses;
using Pokemod.Common.Players;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class FireStoneBreastplate : ModItem
	{
		public static readonly int MaxManaIncrease = 40;
		public static readonly int AdditiveGenericDamageBonus = 10;
		public static readonly int MaxMinionIncrease = 1;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxManaIncrease, AdditiveGenericDamageBonus, MaxMinionIncrease);

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 6; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{

			player.GetDamage<PokemonDamageClass>() += AdditiveGenericDamageBonus / 100f; // Increase dealt damage for all weapon classes by 20%
			player.GetModPlayer<PokemonPlayer>().maxPokemon += 1;
			player.statManaMax2 += MaxManaIncrease; // Increase how many mana points the player can have by 20
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.MeteoriteBar, 15)
				.AddIngredient<FireStoneItem>(2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
