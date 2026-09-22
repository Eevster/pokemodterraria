using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BreloomCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 42;

		public override int totalFrames => 24;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [13,17];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [18,23];
		public override int[] fallStartEnd => [22,23];
        public override int[] attackStartEnd => [6, 12];
		public override float catchRate => 90;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.TheHallow, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGlowshroom) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
			}

			return 0f;
		}
	}

	public class BreloomCritterNPCShiny : BreloomCritterNPC{}
}
