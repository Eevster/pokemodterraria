
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class JoltikCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 14;
		public override int hitboxHeight => 10;

		public override int moveStyle => 3;

		public override int totalFrames => 2;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,1];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [1,1];
		public override int[] attackStartEnd => [0,0];
        public override float catchRate => 190;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
				new FlavorTextBestiaryInfoElement("Since it can't generate its own electricity, it sticks onto large-bodied Pokémon and absorbs static electricity."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGranite) {
                return GetSpawnChance(spawnInfo, 0.01f);
            }

			return 0f;
		}
	}

	public class JoltikCritterNPCShiny : JoltikCritterNPC{}
}