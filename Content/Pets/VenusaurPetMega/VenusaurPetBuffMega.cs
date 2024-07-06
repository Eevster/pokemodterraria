using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VenusaurPetMega
{
	public class VenusaurPetBuffMega : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<VenusaurPetProjectileMega>());

            // Apply buffs only if the pet is active
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Heartreach, 60); // Apply the first buff
                player.AddBuff(BuffID.RapidHealing, 60); // Apply the first buff
                player.AddBuff(BuffID.Regeneration, 60);
                player.AddBuff(BuffID.LeafCrystal, 60);
            }
        }
	}
}
