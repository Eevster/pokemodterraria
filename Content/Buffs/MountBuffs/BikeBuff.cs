using Pokemod.Content.Mounts.Bikes;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs.MountBuffs
{
    public class BikeBuff : ModBuff
	{
        public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.mount.SetMount(ModContent.MountType<Bike>(), player);
			player.buffTime[buffIndex] = 10; // reset buff time
		}
    }
}