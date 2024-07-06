using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ChikoritaPet
{
	public class ChikoritaPetBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<ChikoritaPetProjectile>());

            // Apply buffs only if the pet is active
            if (player.HasBuff(Type))
            {
				player.AddBuff(BuffID.Lifeforce, 60); // Apply the first buff
            }
        }
	}
}
