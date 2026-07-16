using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CyndaquilPet
{
	public class CyndaquilPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,15];

		public override string[] evolutions => ["Quilava"];
		public override int levelToEvolve => 14;
		public override int levelEvolutionsNumber => 1;

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);

		public override void Visuals()
        {
            base.Visuals();
			Projectile.light = (visualsTimer > 0 || !Main.dayTime)?1f:0f;
        }

		private float animTimer = 0;
		private float visualsTimer = 0;

		public override void DrawBehindMainSprite(Color lightColor)
		{
			if ((visualsTimer > 0 || !Main.dayTime) && ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Pets/Extras/"+pokemonName+"PetProjectile_Back", out Asset<Texture2D> backTexture))
			{
				int horizontalFrames = 3;
				int frameDuration = 5;

				Vector2 positionOffset = (backTexture.Frame(horizontalFrames, totalFrames).Size() * Vector2.UnitY * 0.5f) - Vector2.UnitY * 4f; //Anchors the sprite to the bottom of the hitbox correctly

				Main.EntitySpriteDraw(backTexture.Value, Projectile.Bottom - Projectile.scale * positionOffset - Main.screenPosition,
					backTexture.Frame(horizontalFrames, totalFrames, (int)(animTimer/frameDuration), Projectile.frame), Color.White, Projectile.rotation,
					backTexture.Frame(horizontalFrames, totalFrames).Size() / 2f, Projectile.scale, Projectile.spriteDirection >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

				if(++animTimer >= frameDuration * horizontalFrames)
				{
					animTimer = 0;
				}

				if(visualsTimer > 0) visualsTimer--;
			}
		}

        public override void Attack(float distanceFromTarget, Vector2 targetCenter)
        {
            base.Attack(distanceFromTarget, targetCenter);
			visualsTimer = 10*60;
        }
	}

	public class CyndaquilPetProjectileShiny : CyndaquilPetProjectile{}
}
