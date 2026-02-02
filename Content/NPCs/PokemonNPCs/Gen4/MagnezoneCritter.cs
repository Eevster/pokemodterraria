using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MagnezoneCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 72;
		public override int hitboxHeight => 60;

		public override int totalFrames => 16;
		public override int animationSpeed => 6;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [0,7];

		public override int[] idleFlyStartEnd => [0,7];
		public override int[] walkFlyStartEnd => [0,7];
		public override int[] attackFlyStartEnd => [8,15];

        public override int minLevel => 40;

		public override float catchRate => 30;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Sky, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky); 
			base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneSkyHeight) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Sky.Chance * 0.005f);
			}

			return 0f;
		}
	}

	public class MagnezoneCritterNPCShiny : MagnezoneCritterNPC{}
}