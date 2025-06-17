using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class PokeHealerDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
    }
}