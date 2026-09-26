
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SolosisCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
        public override int hitboxHeight => 26;
		
		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [6,9];
		public override int[] jumpStartEnd => [10,13];
		public override int[] fallStartEnd => [12,13];
		public override int[] attackStartEnd => [0,5];
        public override float catchRate => 200;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.UndergroundMushroom, (int)DayTimeStatus.All, (int)WeatherStatus.All],
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGlowshroom) {
                return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance * 0.8f);
            }

			return 0f;
		}
	}

	public class SolosisCritterNPCShiny : SolosisCritterNPC { }
}