using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class MegaEvolution : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<PokemonPlayer>().HasMegaStone <= 0) player.ClearBuff(ModContent.BuffType<MegaEvolution>());
        }
    }
}