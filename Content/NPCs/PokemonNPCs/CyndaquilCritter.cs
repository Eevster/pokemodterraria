using Pokemod.Content.Items;
using Pokemod.Content.Pets.CyndaquilPet;
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
	public class CyndaquilCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 26;
		public override int[] baseStats => [50,50,50,50,50,50];

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,13];


		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("It has a timid nature. If it is startled, the flames on its back burn more vigorously."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f;
            }

			return 0f;
		}
	}

	public class CyndaquilCritterNPCShiny : CyndaquilCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f * 0.00025f;
            }

			return 0f;
		}
	}
}