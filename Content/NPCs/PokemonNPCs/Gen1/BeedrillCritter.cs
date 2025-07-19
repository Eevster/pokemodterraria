using System;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BeedrillCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];

        public override float catchRate => 120;

        public override int minLevel => 30;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Its best attack involves flying around at high speed, striking with poison needles, then flying off."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class BeedrillCritterNPCShiny : BeedrillCritterNPC{}
}