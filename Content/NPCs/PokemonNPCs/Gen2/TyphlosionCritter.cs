using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TyphlosionCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 34;
		public override int hitboxHeight => 46;

		public override int totalFrames => 11;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [0,5];

		public override int minLevel => 36;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Desert, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.02f);
            }

			return 0f;
		}

		private float animTimer = 0;

		public override void DrawOver(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if ((hostilePokemon || !Main.dayTime) && ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Pets/Extras/"+pokemonName+"PetProjectile_Front", out Asset<Texture2D> frontTexture))
			{
				int horizontalFrames = 3;
				int frameDuration = 5;

				Vector2 positionOffset = (frontTexture.Frame(horizontalFrames, totalFrames).Size() * Vector2.UnitY * 0.5f) - Vector2.UnitY * 4f;

				spriteBatch.Draw(frontTexture.Value, NPC.Bottom - NPC.scale * positionOffset - screenPos,
					frontTexture.Frame(horizontalFrames, totalFrames, (int)(animTimer/frameDuration), (int)currentFrame), Color.White, NPC.rotation,
					frontTexture.Frame(horizontalFrames, totalFrames).Size() / 2f, NPC.scale, NPC.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

				if(++animTimer >= frameDuration * horizontalFrames)
				{
					animTimer = 0;
				}
			}
		}
	}

	public class TyphlosionCritterNPCShiny : TyphlosionCritterNPC{}
}