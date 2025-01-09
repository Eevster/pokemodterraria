using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
	public class BlackBeltItem : ModItem
	{
		// By declaring these here, changing the values will alter the effect, and the tooltip

		// Insert the modifier values into the tooltip localization. More info on this approach can be found on the wiki: https://github.com/tModLoader/tModLoader/wiki/Localization#binding-values-to-localizations

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<PokemonDamageClass>() += 0.15f;
		}
	}

	// Some movement effects are not suitable to be modified in ModItem.UpdateAccessory due to how the math is done.
	// ModPlayer.PostUpdateRunSpeeds is suitable for these modifications.
	
}