using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PsyduckCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 24;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0, 7];
		public override int[] walkStartEnd => [8, 13];
		public override int[] jumpStartEnd => [11, 11];
		public override int[] fallStartEnd => [0, 0];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [15,18];
		public override int[] walkSwimStartEnd => [19,22];
		public override int[] attackSwimStartEnd => [23,23];
		public override float catchRate => 190;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All],
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("As Psyduck gets stressed out, its headache gets progressively worse. It uses intense psychic energy to overwhelm those around it."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach || spawnInfo.Player.ZoneForest)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
		
	}

	public class PsyduckCritterNPCShiny : PsyduckCritterNPC{}
}