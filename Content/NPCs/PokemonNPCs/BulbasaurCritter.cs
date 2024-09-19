using Pokemod.Content.Items;
using Pokemod.Content.Pets.BulbasaurPet;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BulbasaurCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;
		public override int[] baseStats => [50,50,50,50,50,50];

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,19];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("For some time after its birth, it grows by taking nourishment from the seed on its back."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
                return SpawnCondition.OverworldDay.Chance * 0.5f;
            }

			return 0f;
		}
	}

	public class BulbasaurCritterNPCShiny : BulbasaurCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
                return SpawnCondition.OverworldDay.Chance * 0.5f * 0.00025f;
            }

			return 0f;
		}
	}
}