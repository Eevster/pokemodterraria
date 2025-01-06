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
		public static readonly int AdditiveDamageBonus = 25;
		
		public static readonly int MeleeCritBonus = 10;
		public static readonly int ExampleKnockback = 100;
		public static readonly int AdditiveCritDamageBonus = 20;

		// Insert the modifier values into the tooltip localization. More info on this approach can be found on the wiki: https://github.com/tModLoader/tModLoader/wiki/Localization#binding-values-to-localizations
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AdditiveDamageBonus, MeleeCritBonus, ExampleKnockback, AdditiveCritDamageBonus);

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			// GetDamage returns a reference to the specified damage class' damage StatModifier.
			// Since it doesn't return a value, but a reference to it, you can freely modify it with mathematics operators (+, -, *, /, etc.).
			// StatModifier is a structure that separately holds float additive and multiplicative modifiers, as well as base damage and flat damage.
			// When StatModifier is applied to a value, its additive modifiers are applied before multiplicative ones.
			// Base damage is added directly to the weapon's base damage and is affected by damage bonuses, while flat damage is applied after all other calculations.
			// In this case, we're doing a number of things:
			// - Adding 25% damage, additively. This is the typical "X% damage increase" that accessories use, use this one.
			// - Adding 12% damage, multiplicatively. This effect is almost never used in Terraria, typically you want to use the additive multiplier above. It is extremely hard to correctly balance the game with multiplicative bonuses.
			// - Adding 4 base damage.
			// - Adding 5 flat damage.
			// Since we're using DamageClass.Generic, these bonuses apply to ALL damage the player deals.
			player.GetDamage<PokemonDamageClass>() += AdditiveDamageBonus / 100f;

			// GetCrit, similarly to GetDamage, returns a reference to the specified damage class' crit chance.
			// In this case, we're adding 10% crit chance, but only for the melee DamageClass (as such, only melee weapons will receive this bonus).
			// NOTE: Once all crit calculations are complete, a weapon or class' total crit chance is typically cast to an int. Plan accordingly.
			player.GetCritChance<PokemonDamageClass>() += MeleeCritBonus;


			// GetKnockback is functionally identical to GetDamage, but for the knockback stat instead.
			// In this case, we're adding 100% knockback additively, but only for our custom example DamageClass (as such, only our example class weapons will receive this bonus).
			player.GetKnockback<PokemonDamageClass>() += ExampleKnockback / 100f;

			
			// Some effects are applied in ExampleStatBonusAccessoryPlayer below.
			player.GetModPlayer<PokemodStatBonusAccessoryPlayer>().pokemodStatBonusAccessory = true;
		}
	}

	// Some movement effects are not suitable to be modified in ModItem.UpdateAccessory due to how the math is done.
	// ModPlayer.PostUpdateRunSpeeds is suitable for these modifications.
	public class PokemodStatBonusAccessoryPlayer : ModPlayer {
		public bool pokemodStatBonusAccessory = false;

		public override void ResetEffects() {
			pokemodStatBonusAccessory = false;
		}

		public override void PostUpdateRunSpeeds() {
			// We only want our additional changes to apply if ExampleStatBonusAccessory is equipped and not on a mount.
			if (Player.mount.Active || !pokemodStatBonusAccessory) {
				return;
			}

			// The following modifications are similar to Shadow Armor set bonus
			Player.runAcceleration *= 1.75f; // Modifies player run acceleration
			Player.maxRunSpeed *= 1.15f;
			Player.accRunSpeed *= 1.15f;
			Player.runSlowdown *= 1.75f;
		}
	}
}