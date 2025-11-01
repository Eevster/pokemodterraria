using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class UmbreonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 24;
		
		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

        public override float catchRate => 120;
        public override int minLevel => 40;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.TheHallow, (int)DayTimeStatus.Night, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("The light of the moon changed Eevee's genetic structure. It lurks in the darkness, waiting for prey."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneHallow) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.001f);
			}

			return 0f;
		}
	}

	public class UmbreonCritterNPCShiny : UmbreonCritterNPC{}
}