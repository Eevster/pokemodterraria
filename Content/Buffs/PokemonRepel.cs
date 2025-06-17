using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class PokemonRepel : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
    }
}