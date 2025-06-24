using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items
{
	public class SynchroMachine : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.MenuTick;
			Item.value = Item.buyPrice(platinum: 1);
		}
		
		public override bool? UseItem(Player player)
		{
            if (player.whoAmI != Main.myPlayer) {
				return true;
			}

			player.GetModPlayer<PokemonPlayer>().SetManualControl();

            return true;
        }
	}
}