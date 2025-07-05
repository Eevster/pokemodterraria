using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class PsychoCut : PokemonAttack
	{

		public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 112;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 30;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 10;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 25;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.Opacity = 0.6f;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						pokemonOwner.canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer%7 == 0 && pokemonOwner.timer <= 21)
                {
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
							pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 25f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<PsychoCut>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
							SoundEngine.PlaySound(SoundID.Item34, pokemon.position);
							break;
						}
					} 
				}
			}
		}

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.OnSpawn(source);
        }

        public override void AI()
        {
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.width, DustID.PinkTorch);

            if(Projectile.timeLeft < 10){
                Projectile.Opacity = 0.6f*Projectile.timeLeft*0.1f;
            }

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {

			Vector2 start = Projectile.Center - 56*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + 56*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 32f, ref collisionPoint);
		}
    }
}
