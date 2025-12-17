using Pokemod.Content.Items.Tools;
using Pokemod.Content.NPCs.PokemonNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Common.Players
{
	public class PokemonFishingPlayer : ModPlayer
	{
		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition) {
			bool inWater = !attempt.inHoney;

			bool isPokemonFishingRod = attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<OldRod>() ||
			attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<GoodRod>() ||
			attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<SuperRod>();
			
			int[] pokemonList = {
				ModContent.NPCType<MagikarpCritterNPC>(),
				ModContent.NPCType<PoliwagCritterNPC>(),
				ModContent.NPCType<ShellderCritterNPC>(),
				ModContent.NPCType<DratiniCritterNPC>(),
			};

			const int oldRodLimit = 1;
			const int goodRodLimit = 3;

			if (isPokemonFishingRod && inWater)
			{
				int npc = -1;

				if (attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<OldRod>())
				{
					npc = pokemonList[Main.rand.Next(oldRodLimit)];
				}
				if (attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<GoodRod>())
				{
					npc = pokemonList[Main.rand.Next(goodRodLimit)];
				}
				if (attempt.playerFishingConditions.PoleItemType == ModContent.ItemType<SuperRod>())
				{
					npc = pokemonList[Main.rand.Next(pokemonList.Length)];
				}

				if (npc != -1)
				{
					// Make sure itemDrop = -1 when summoning an NPC, as otherwise terraria will only spawn the item
					npcSpawn = npc;
					itemDrop = -1;

					// Also, to make it cooler, we will make a special sonar message for when it shows up
					sonar.Text = "Gotcha!";
					sonar.Color = Color.LimeGreen;
					sonar.Velocity = Vector2.Zero;
					sonar.DurationInFrames = 300;

					// And that text shows up on the player's head, not on the bobber location.
					sonarPosition = new Vector2(Player.position.X, Player.position.Y - 64);

					return; // This is important so your code after this that rolls items will not run
				}
			}
		}
	}
}