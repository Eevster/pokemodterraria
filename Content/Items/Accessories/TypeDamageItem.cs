using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;

namespace Pokemod.Content.Items.Accessories
{
    public abstract class TypeDamageItem : ModItem
	{
		public virtual int pokemonType => 0;
		public virtual float damageMult => 0.1f;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonTypes."+(TypeIndex)pokemonType), (int)(100f*damageMult));
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 50);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().typeMult[pokemonType] += damageMult;
		}
	}
}