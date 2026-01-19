using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public abstract class MegaStoneItem : ModItem
	{
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Item.ModItem.Name.Replace("MegaStoneItemX", "").Replace("MegaStoneItemY", "").Replace("MegaStoneItemZ", "").Replace("MegaStoneItem", ""));
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;

			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Pink;
			Item.SetShopValues(Terraria.Enums.ItemRarityColor.Pink5, Item.buyPrice(gold: 50));
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<PokemonPlayer>().SetMegaEvolution(Name);
			player.GetModPlayer<PokemonPlayer>().HasMegaStone = 3;
		}

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
			if (incomingItem.ModItem is MegaStoneItem && equippedItem.ModItem is MegaStoneItem) return false;

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
	}
}
