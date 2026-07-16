using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TyphlosionPet
{
	public class TyphlosionPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 34;
        public override int hitboxHeight => 46;

        public override int totalFrames => 11;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [0,5];

		public override float moveDistance1 => 200f;
		public override float moveDistance2 => 140f;

		public override bool canBeMounted => true;
        public override Vector2 playerMountPosition => new Vector2(0,0);

		public override void Visuals()
        {
            base.Visuals();
			Projectile.light = (visualsTimer > 0 || !Main.dayTime)?1f:0f;
        }

		private float animTimer = 0;
		private float visualsTimer = 0;

		public override void DrawOverMainSprite(Color lightColor)
		{
			if ((visualsTimer > 0 || !Main.dayTime) && ModContent.RequestIfExists("Pokemod/Assets/Textures/Pokesprites/Pets/Extras/"+pokemonName+"PetProjectile_Front", out Asset<Texture2D> frontTexture))
			{
				int horizontalFrames = 3;
				int frameDuration = 5;

				Vector2 positionOffset = (frontTexture.Frame(horizontalFrames, totalFrames).Size() * Vector2.UnitY * 0.5f) - Vector2.UnitY * 4f; //Anchors the sprite to the bottom of the hitbox correctly

				Main.EntitySpriteDraw(frontTexture.Value, Projectile.Bottom - Projectile.scale * positionOffset - Main.screenPosition,
					frontTexture.Frame(horizontalFrames, totalFrames, (int)(animTimer/frameDuration), Projectile.frame), Color.White, Projectile.rotation,
					frontTexture.Frame(horizontalFrames, totalFrames).Size() / 2f, Projectile.scale, Projectile.spriteDirection >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

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

	public class TyphlosionPetProjectileShiny : TyphlosionPetProjectile{}
}
