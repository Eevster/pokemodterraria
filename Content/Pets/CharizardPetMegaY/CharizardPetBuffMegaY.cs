using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharizardPetMegaY
{
	public class CharizardPetBuffMegaY : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<CharizardPetProjectileMegaY>());

            // Apply buffs only if the pet is active
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
                player.AddBuff(BuffID.Featherfall, 60); // Apply the first buff
                player.AddBuff(BuffID.Rage, 60);
                player.AddBuff(BuffID.WeaponImbueFire, 60);
            }
        }
	}
}
