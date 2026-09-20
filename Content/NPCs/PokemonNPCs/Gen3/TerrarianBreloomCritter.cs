using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianBreloomCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 34;

		public override int totalFrames => 20;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [6,9];
		public override int[] walkStartEnd => [14,19];
		public override int[] jumpStartEnd => [10,13];
		public override int[] fallStartEnd => [12,13];
        public override int[] attackStartEnd => [0, 6];
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

	public class TerrarianBreloomCritterNPCShiny : TerrarianBreloomCritterNPC{}
}
