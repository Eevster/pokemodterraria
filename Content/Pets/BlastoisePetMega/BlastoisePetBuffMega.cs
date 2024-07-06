using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BlastoisePetMega
{
	public class BlastoisePetBuffMega : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<BlastoisePetProjectileMega>());

            // Apply buffs only if the pet is active
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Flipper, 60); // Apply the first buff
                player.AddBuff(BuffID.Gills, 60); // Apply the first buff
                player.AddBuff(BuffID.Endurance, 60);
                player.AddBuff(BuffID.WaterWalking, 60);
            }
        }
	}
}
