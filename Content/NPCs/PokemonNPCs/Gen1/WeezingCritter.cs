using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;
using Microsoft.Xna.Framework;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WeezingCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 34;
		public override int hitboxHeight => 40;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;
        public override bool sideDiff => true;

        public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] attackStartEnd => [1,1];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [1,1];

		public override float catchRate => 60;
        public override int minLevel => 35;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.TheCorruption, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.TheCrimson, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Crimson.Chance+SpawnCondition.Corruption.Chance) * 0.2f);
			}

			return 0f;
		}

		public override void ExtraEffects()
		{
			if (!NPC.hide)
			{

				if (Main.rand.NextBool(10))
				{
					int goreIndex = Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Vector2.Zero, Main.rand.Next(220, 223), 1f);
					Main.gore[goreIndex].scale = 0.5f;
					Main.gore[goreIndex].position = NPC.position + 0.5f * hitboxWidth * Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					Main.gore[goreIndex].velocity = 0.05f * hitboxWidth * (Main.gore[goreIndex].position - NPC.position).SafeNormalize(Vector2.UnitX);
				}
			}
		}
	}

	public class WeezingCritterNPCShiny : WeezingCritterNPC{}
}