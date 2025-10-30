using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class OnixCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 11;
		public override int animationSpeed => 12;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 9];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [0, 0];
		public override float catchRate => 45;
        public override int minLevel => 8;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.Desert, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("As it digs through the ground, it absorbs many hard objects. This is what makes its body so solid."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground || spawnInfo.Player.ZoneNormalCaverns)
			{
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.2f);
			}
            if (spawnInfo.Player.ZoneDesert)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
            }

            return 0f;
		}
		
	}

	public class OnixCritterNPCShiny : OnixCritterNPC{}
}