using System.Collections.Generic;
using Pokemod.Content.Items.Badges;
using Pokemod.Content.Items.Tools;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Pokemod.Content.NPCs.TrainerNPCs.GymLeaders
{
	public class G1T3_LtSurge : BattleTrainer
	{
        
		public override bool GymLeader => true;
		public override void LoadTeam()
		{
			pokemonTeam =
            [
                new EnemyPokemonInfo("Voltorb", 21, ["ShockWave", "Tackle", "Screech", "SonicBoom"]),
                new EnemyPokemonInfo("Pikachu", 18, ["ShockWave", "Thunderwave", "QuickAttack", "DoubleTeam"]),
                new EnemyPokemonInfo("Raichu", 24, ["Thunderbolt", "QuickAttack", "ElectroBall", "Discharge"]),
            ];
		}

		public override void GiveRewards(Player opponent)
        {
			opponent.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<ThunderBadge>(), 1);
			opponent.QuickSpawnItem(NPC.GetSource_FromThis(), ItemID.GoldCoin, 6);
            base.GiveRewards(opponent);
        }

		private static Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1)
			// new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);
        }
		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.Pokemod.Bestiary.{GetType().Name}"),
			});
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string> {
				"Lt. Surge",
			};
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if ((spawnInfo.Player.ZoneMarble || spawnInfo.Player.ZoneGranite) && !NPC.AnyNPCs(Type))
			{
				return 0.01f;
			}

			return 0f;
		}
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			if(!WasDefeatedBy(Main.player[Main.myPlayer])){
				chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.StandardDialogue1"));
			}
			else
			{
				chat.Add(Language.GetTextValue($"Mods.Pokemod.Dialogue.{GetType().Name}.DefeatedDialogue"));
			}

			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = "Gym Battle";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			Player opponent = Main.player[Main.myPlayer];

			if (firstButton && !WasDefeatedBy(opponent))
			{
				StartBattle(opponent);
			}
		}
	}
}
