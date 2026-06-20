using Microsoft.Xna.Framework;
using Pokemod.Content.Mounts;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs.MountBuffs
{
    public class PokeMountBuff : ModBuff
	{
        public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.mount.SetMount(ModContent.MountType<PokeMount>(), player);
			player.buffTime[buffIndex] = 10; // reset buff time
		}
    }
}