using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class BrickBreak : PokemonAttack
	{
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 2;
        }
		public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 30;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter - 18*Vector2.Normalize(targetCenter-pokemon.Center), Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<BrickBreak>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 3, 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item20, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = texture.Frame(1, 1).Size() / 2f;
			float rotationAux = MathHelper.ToRadians(60)*(1f-1.2f*(float)Math.Pow(Math.Clamp((25f-Projectile.timeLeft)/20f,0f,1f),3));
            int direction = (Projectile.velocity.X < 0) ? 1 : -1;
            SpriteEffects flipEffect = (Projectile.velocity.X < 0) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            lightColor.A += 170;
			Main.EntitySpriteDraw(texture, Projectile.Center-Main.screenPosition, texture.Frame(1, 1, 0, 0), lightColor, Projectile.rotation + (rotationAux * direction), drawOrigin, Projectile.scale, flipEffect, 0);

			return false;
		}

        public override void OnSpawn(IEntitySource source)
        {
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 0.001f;

            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89 with { Pitch = -0.5f }, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.Stone, Main.rand.NextFloat(-6,6), Main.rand.NextFloat(-7,6), 50, default(Color), 1f);
            }
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center-10f*Projectile.scale*new Vector2(1,0).RotatedBy(Projectile.rotation);
            Vector2 end = Projectile.Center+36f*Projectile.scale*new Vector2(1,0).RotatedBy(Projectile.rotation);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 46f*Projectile.scale, ref collisionPoint);
        }
    }
}
