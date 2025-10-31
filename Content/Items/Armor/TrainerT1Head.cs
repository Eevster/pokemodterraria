using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.EvoStones;
using Pokemod.Content.Items.Armor;
using Pokemod.Content.DamageClasses;
using Pokemod.Common.Players;

namespace Pokemod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class TrainerT1Head : ModItem
	{
		public static readonly int AdditivePokemonCritBonus = 4;
		//public static readonly int MaxPokemonIncrease = 1;

		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;

			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditivePokemonCritBonus);
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 1; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<TrainerT1Body>() && legs.type == ModContent.ItemType<TrainerT1Legs>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.GetCritChance<PokemonDamageClass>() += AdditivePokemonCritBonus;
			//player.GetModPlayer<PokemonPlayer>().maxPokemon += MaxPokemonIncrease;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
