using Pokemod.Content.NPCs.MerchantNPCs;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.Systems;

// This class tracks if specific Town NPC have ever spawned in this world. If they have, then their spawn conditions are not required anymore to respawn in the same world. This behavior is new to Terraria v1.4.4 and is not automatic, it needs code to support it.
// Spawn conditions that can't be undone, such as defeating bosses, would not require tracking like this since those conditions will still be true when the Town NPC attempts to respawn. Spawn conditions checking for items in the player inventory like ExamplePerson does, for example, would need tracking.
public class TownNPCRespawnSystem : ModSystem
{
	// Tracks if ExamplePerson has ever been spawned in this world
	public static bool unlockedScientistSpawn = false;
	public static bool ScientistArrived = false;

    public static Condition scientistArrivedCondition = new Condition(
    Language.GetText("Mods.Pokemod.Conditions.ScientistInTown"),
    () => ScientistArrived
    );

    // Town NPC rescued in the world would follow a similar implementation, the only difference being how the value is set to true.
    // public static bool savedExamplePerson = false;

    public override void ClearWorld() {
		unlockedScientistSpawn = false;
        ScientistArrived = false;
    }

	public override void SaveWorldData(TagCompound tag) {
		tag[nameof(unlockedScientistSpawn)] = unlockedScientistSpawn;
        tag[nameof(ScientistArrived)] = ScientistArrived;
    }

	public override void LoadWorldData(TagCompound tag) {
		unlockedScientistSpawn = tag.GetBool(nameof(unlockedScientistSpawn));
        ScientistArrived = tag.GetBool(nameof(ScientistArrived));

        // This line sets unlockedScientistSpawn to true if an ExamplePerson is already in the world. This is only needed because unlockedScientistSpawn was added in an update to this mod, meaning that existing users might have unlockedScientistSpawn incorrectly set to false.
        // If you are tracking Town NPC unlocks from your initial mod release, then this isn't necessary.
        unlockedScientistSpawn |= NPC.AnyNPCs(ModContent.NPCType<PokemonScientist>());
        ScientistArrived |= NPC.AnyNPCs(ModContent.NPCType<PokemonScientist>());
    }

	public override void NetSend(BinaryWriter writer) {
		writer.WriteFlags(unlockedScientistSpawn);
        writer.WriteFlags(ScientistArrived);
    }

	public override void NetReceive(BinaryReader reader) {
		reader.ReadFlags(out unlockedScientistSpawn);
        reader.ReadFlags(out ScientistArrived);
    }

}
