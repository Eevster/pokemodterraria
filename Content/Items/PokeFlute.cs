using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items
{
	public class PokeFlute : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.DrinkLong;
			Item.UseSound = SoundID.Item128;
			Item.value = Item.sellPrice(silver: 50);
		}
		
		public override bool? UseItem(Player player)
		{
            if (player.whoAmI != Main.myPlayer) {
				return true;
			}

			//

            return true;
        }
	}
}