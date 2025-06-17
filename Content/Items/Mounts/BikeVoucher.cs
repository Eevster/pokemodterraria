using Pokemod.Content.Mounts.Bikes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Mounts
{
    public class BikeVoucher : ModItem
	{
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item52; // What sound should play when using the item
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.mountType = ModContent.MountType<Bike>();
		}
	}
}