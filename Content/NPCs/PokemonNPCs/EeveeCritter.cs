using Pokemod.Content.Items;
using Pokemod.Content.Pets.EeveePet;
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
	public class EeveeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 22;
		public override int hitboxHeight => 30;
		public override int[] baseStats => [50,50,50,50,50,50];

		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Its ability to evolve into many forms allows it to adapt smoothly and perfectly to any environment."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.1f;
			}

			return 0f;
		}
	}

	public class EeveeCritterNPCShiny : EeveeCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.1f * 0.00025f;
			}
			
			return 0f;
		}
	}
}