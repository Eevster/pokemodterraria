using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<CharizardPetProjectile>());

            // Apply buffs only if the pet is active
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Featherfall, 60); // Apply the first buff
				player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
            }
        }
	}
}
