using Pokemod.Content.Projectiles.Tools;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Tools
{
	public class OldRod : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.CanFishInLava[Item.type] = false;
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.WoodFishingPole);

			Item.fishingPole = 10; // Sets the poles fishing power
			Item.shootSpeed = 12f; // Sets the speed in which the bobbers are launched. Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f.
			Item.shoot = ModContent.ProjectileType<PokeBobber>(); // The bobber projectile. Note that this will be overridden by Fishing Bobber accessories if present, so don't assume the bobber spawned is the specified projectile. https://terraria.wiki.gg/wiki/Fishing_Bobbers
		}

		// Grants the High Test Fishing Line bool if holding the item.
		// NOTE: Only triggers through the hotbar, not if you hold the item by hand outside of the inventory.
		public override void HoldItem(Player player) {
			player.accFishingLine = true;
		}

		// Overrides the default shooting method to fire multiple bobbers.
		// NOTE: This will allow the fishing rod to summon multiple Duke Fishrons with multiple Truffle Worms in the inventory.
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, type, 0, 0f, player.whoAmI);

			return false;
		}

		public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor) {
			// Change these two values in order to change the origin of where the line is being drawn.
			// This will make it draw 43 pixels right and 30 pixels up from the player's center, while they are looking right and in normal gravity.
			lineOriginOffset = new Vector2(43, -30);

			// Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
			if (bobber.ModProjectile is PokeBobber pokeBobber) {
				lineColor = pokeBobber.FishingLineColor;
			}
			else {
				// If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
				lineColor = Main.DiscoColor;
			}
		}
	}
}